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

        
        // TODO: Write a global error handling event for Headless
        static async Task Main(string[] args)
        {          
            //TODO: Put Try Catch blocks around file handling and test for issues in various scenarios
            //############################
            // 4.) Listen for commands ###
            //############################
            await CommandLine.ReadLoop(Daemon, Wallet, args);

        }





        

        
    }
}
