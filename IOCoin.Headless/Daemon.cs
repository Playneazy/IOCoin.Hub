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
using IOCoin.Headless.Processes.Interfaces;

namespace IOCoin.Headless
{
    public class Daemon
    {


        public DaemonHelpers daemonHelpers { get; set; }

        public Settings settings { get; set; } 


        public StartDaemon DaemonProcess { get; set; }

        public Daemon(IWallet wallet)
        {

            SetupStaticLogger();
            var stestd = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Headless.config");

            // Set the settings file to App.config since were loading from DLL
            AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Headless.config"));
            settings = new Settings();

            Task.Run(LoadSettings).Wait();      // Load DaemonPath, AppDataDir, and ConfigFilePath from App.config
            DaemonProcess = new StartDaemon(settings, wallet);

            Task.Run(InitDaemon).Wait();        // Validate Directories and Files for Daemon Launch
        }

        private static void SetupStaticLogger()
        {
   
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(outputTemplate: "{Timestamp:yyyy/MM/dd h:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
        }


        public async Task LoadSettings()
        {
            settings.daemonPath = settings.ReadSetting("daemonpath");
            settings.appDataDir = settings.ReadSetting("appdatadir");
            settings.configFilepath = settings.ReadSetting("configfilepath");
            settings.walletPasshrase = settings.ReadSetting("walletpassphrase");
            settings.notificationAddress = settings.ReadSetting("notificationaddress");

            settings.addNodes = settings.ReadSetting("addnodes").Split(",").ToArray();

            daemonHelpers = new DaemonHelpers(settings.appDataDir);
        }

        public async Task InitDaemon()
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
