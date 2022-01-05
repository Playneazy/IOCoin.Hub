using IOCoin.Headless.Processes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IOCoin.Headless.Helpers;
using System.Threading;
using Serilog;
using System.Diagnostics;
using IOCoin.Headless;
using IOCoin.Console.Helpers;
using System.IO;
using IOCoin.Wallet;

namespace IOCoin.Console.Functions.Wallet
{
    public class LoadWallet
    {

        private Daemon daemon { get; set; }

        private int attempts { get; set; } = 0;

        public async Task<bool> Run(Daemon Daemon, Info wallet)
        {
            daemon = Daemon;

            var dh = new DaemonHelpers(daemon.settings.appDataDir);
            var t = File.Exists(daemon.settings.configFilepath);

            if (!ReadConfigFile() || string.IsNullOrEmpty(daemon.settings.configFilepath) || !File.Exists(daemon.settings.configFilepath))
            {
                // Default RPC
                daemon.settings.rpcUser = "iocoinrpc";
                daemon.settings.rpcPassword = GetRandomPassword.Generate(44);


                daemon.settings.AddOrUpdateSettings("configfilepath", dh.FormatDirPath() + "iocoin.conf");
                await WriteConfigFile(); // Save a default iocoin.conf file with nodes
            }

            daemon.settings.daemonArgBase = "-datadir=" + dh.FormatDirPath() + " -rpcuser=" + daemon.settings.rpcUser + " -rpcpassword=" + daemon.settings.rpcPassword + " ";


            await daemon.DaemonProcess.Run();
            await UpdateWallet(wallet);

            return true;
        }

        private async Task UpdateWallet(Info wallet)
        {

            attempts += 1;
            Log.Debug($"Checking wallet... Please Wait. [{attempts}]");
            

            var getInfo = new GetInfo(daemon.settings, wallet);
            var res = await getInfo.Run();

            if (res.Rpc.Result == null)
            {

                Thread.Sleep(10000);

                await daemon.DaemonProcess.Run();
                await UpdateWallet(wallet);
            }


        }

        public bool ReadConfigFile()
        {
            // Handle any problems that might arise when reading the text
            try
            {
                string line;
                // Create a new StreamReader, tell it which file to read and what encoding the file was saved as
                StreamReader theReader = new StreamReader(daemon.settings.configFilepath, Encoding.Default);

                // Immediately clean up the reader after this block of code is done.
                // You generally use the "using" statement for potentially memory-intensive objects instead of relying on garbage collection.
                // (Do not confuse this with the using directive for namespace at the beginning of a class!)
                using (theReader)
                {
                    // While there's lines left in the text file, do this:
                    do
                    {
                        line = theReader.ReadLine();
                        if (line != null)
                        {
                            // Split the string it into arguments based on comma deliniators
                            string[] entries = line.Split('=');
                            if (entries.Length > 1)
                            {
                                if (entries[0] == "rpcuser") daemon.settings.rpcUser = entries[1];
                                if (entries[0] == "rpcpassword") daemon.settings.rpcPassword = entries[1];
                            }
                        }
                    }
                    while (line != null);

                    // Done reading, close the reader and return true to broadcast success    
                    theReader.Close();

                    if (daemon.settings.rpcUser.Length == 0 || daemon.settings.rpcPassword.Length == 0)
                    {
                        return false;
                    }

                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public async Task WriteConfigFile()
        {
            List<string> lines = new List<string>();
            lines.Add("rpcuser=" + daemon.settings.rpcUser);
            lines.Add("rpcpassword=" + daemon.settings.rpcPassword);

            foreach (var node in daemon.settings.addNodes)
            {
                lines.Add("addnode=" + node);
            }


            var dh = new DaemonHelpers(daemon.settings.appDataDir);
            await File.WriteAllLinesAsync(dh.FormatDirPath() + "iocoin.conf", lines.ToArray());
        }


    }
}
