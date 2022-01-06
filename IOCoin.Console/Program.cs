using IOCoin.Headless;
using IOCoin.Headless.Helpers;
using IOCoin.Headless.Processes;
using IOCoin.Console.Functions;
using IOCoin.Console.Functions.Blockchain;
using IOCoin.Console.Functions.Wallet;
using IOCoin.Console.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;
using IOCoin.Wallet;
using IOCoin.Headless.Events;

namespace IOCoin.Console
{
    internal class Program
    {

        static Daemon Daemon { get; set; }
        static Settings settings { get; set; } = new Settings();
        static Info Wallet { get; set; } = new Info();

        //TODO: Enable logging by running file
        static async Task Main(string[] args)
        {          

            //############################
            // 4.) Listen for commands ###
            //############################
            await CommandLine.ReadLoop(Daemon, Wallet, args);

        }

        private static void LoadSettings()
        {
            // This uses the Settings class from the IOCoin.Headless project to read/write into 'Headless.Config'
            //settings.updateintervalMin = Int32.Parse(Daemon.settings(walletName).ReadSetting("updateintervalMin"));
            //if (settings.updateintervalMin == 0) settings.updateintervalMin = 20;       // Default of 20 minute updates
        }




        

        
    }
}
