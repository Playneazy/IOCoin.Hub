using IOCoin.Headless.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IOCoin.Headless.Helpers;
using System.Linq;
using IOCoin.Headless.Processes;
using Serilog;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using IOCoin.Headless.Interfaces;

namespace IOCoin.Headless
{
    public class Daemon
    {

        public DaemonHelpers daemonHelpers { get; set; }

        public Settings wallets { get; set; } = new Settings();

        private Settings setting { get; set; } 

        public string WalletsPath { get; set; }

        public StartDaemon DaemonProcess { get; set; }

        public Daemon(IWallet wallet, string walletName)
        {

            SetupStaticLogger();

            // Setup multi-wallet
            WalletsPath = Directory.GetCurrentDirectory() + "\\Wallets.json";
            if (!File.Exists(WalletsPath))
            {
                Log.Error("Could not find wallets file: " + WalletsPath);
                return;
            }
            try
            {            
                wallets = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(WalletsPath));
            }
            catch (Exception ex)
            {
                Log.Error("Error Loading Wallets: " + ex.Message);
            }

            
            if (string.IsNullOrEmpty(walletName) && !string.IsNullOrEmpty(wallets.lastusedwallet))
            {
                walletName = wallets.lastusedwallet;        // Loads the last used wallet if one is there and no wallet was fed into the daemon.
            }


            wallets.lastusedwallet = walletName;

            SaveSettings().Wait();

            if (!string.IsNullOrEmpty(walletName))
            {
                daemonHelpers = new DaemonHelpers(settings(walletName).appDataDir);

                DaemonProcess = new StartDaemon(settings(walletName), wallet);

                InitDaemon(settings(walletName)).Wait();        // Validate Directories and Files for Daemon Launch
            }
                
        }

        public WalletConfig settings(string walletName) => wallets?.walletconfigs.Where(w => w.WalletName == walletName).FirstOrDefault();

        private static void SetupStaticLogger()
        {
   
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(outputTemplate: "{Timestamp:yyyy/MM/dd h:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
        }


        public async Task SaveSettings()
        {
            string walletJson = JsonConvert.SerializeObject(wallets, Formatting.Indented);
            await File.WriteAllTextAsync(WalletsPath, walletJson);
        }
        public async Task<JsonObjType> LoadSettings<JsonObjType>(string filePath)
        {
            
            if (!File.Exists(filePath))
            {
                Log.Error("Could not find file: " + filePath);
                return default;
            }
            try
            {
                var fullConfig = await File.ReadAllTextAsync(filePath);
                fullConfig = fullConfig.Replace(@"\", @"\\");

                return JsonConvert.DeserializeObject<JsonObjType>(fullConfig);
            }
            catch (Exception ex)
            {
                Log.Error("Error Loading File: " + ex.Message);
                return default;
            }
        }

        public async Task InitDaemon(WalletConfig settings)
        {

            // Ensure datadir format has ending slash
            //if (!settings.appDataDir.EndsWith("\\")) settings.appDataDir += "\\";

            // Check if Daemon exists
            if (!File.Exists(settings.daemonPath))
            {
                return;
            }

            // Check if appData Dir exists
            if (!daemonHelpers.isDataDirValid())
            {
                // New.
                Directory.CreateDirectory(settings.appDataDir);
                await DaemonProcess.Run(true);
            } else if (!daemonHelpers.isWalletFileValid() && !daemonHelpers.isTXDatabaseDirValid())
            {
                // Existing.
                // Only create dirs and files if Wallet.Dat and TxDB are NOT valid.
                // This is a safety measure as to not overwrite important files.
                await DaemonProcess.Run(true);
                
            }

            // Wait for file structure to be created and validated
            bool isReady = false;
            while (!isReady)
            {
                if (daemonHelpers.isDefaultDirStructureValid()
                && daemonHelpers.isDefaultFileStructureValid())
                {
                    isReady = true;
                }
                Thread.Sleep(2000);
            }

        }



        


        
        
        
        
       
        
    }
}
