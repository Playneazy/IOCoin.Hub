﻿using IOCoin.Headless;
using IOCoin.Headless.Processes;
using IOCoin.Console.Functions;
using IOCoin.Console.Functions.Blockchain;
using IOCoin.Console.Functions.Wallet;
using IOCoin.Console.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IOCoin.Wallet;
using IOCoin.Headless.Events;

namespace IOCoin.Console
{
    public static class CommandLine
    {
        private static string walletName { get; set; }
        public static async Task ReadLoop(Daemon Daemon, Info wallet, string[] args)
        {
            while (true)
            {
                ConsoleWriter.Normal("Enter a command or <ENTER> to show update: ");

                var cmd = System.Console.ReadLine();
                // Calls to Processes
                if (cmd.StartsWith("-"))
                {
                    cmd = cmd.Substring(1);
                    string command = string.Empty;
                    IEnumerable<string> cmdArgs = null;
                    if (cmd.Contains(" "))
                    {
                        command = cmd.Split(' ').First();
                    } else
                    {
                        command = cmd;
                    }
  
                    switch (command)
                    {
                        case "registeralias":
                            var alias = cmdArgs?.ElementAtOrDefault(1);
                            if (!string.IsNullOrEmpty(alias))
                            {
                                var aliasHash = await new RegisterAlias(Daemon.settings(walletName), wallet).Run(cmdArgs.ElementAtOrDefault(1));
                                await new AliasList(Daemon.settings(walletName), wallet).Run();
                            }
                                
                            break;
                        case "aliaslist":
                            var aliases = await new AliasList(Daemon.settings(walletName), wallet).Run();
                            break;
                        case "listtransactions":
                            var transactions = await new ListTransactions(Daemon.settings(walletName), wallet).Run();
                            break;
                        case "stakinginfo":
                            var stakeInfo = await new GetStakingInfo(Daemon.settings(walletName), wallet).Run();
                            break;
                        case "loadwallet":
                            walletName = cmdArgs?.ElementAt(1);
                            if (string.IsNullOrEmpty(walletName))
                            {
                                // 1.) Initialize Daemon and Console settings
                                ConsoleWriter.Info($"Initializing daemon and files...");
                                Daemon = new Daemon(wallet, walletName);
                                // TODO: Write a method to load console settings from JSON
                                //LoadSettings();     
                                // TODO: Load the interval for the TimedUpdates in step 3

                                // 2.) Setup webserver for Block and Wallet Notifications from Daemon
                                WebServer ws = new WebServer(Daemon.settings(walletName));
                                ws.BlockNotification += c_BlockNotification;
                                ws.WalletNotification += c_WalletNotification;

                                // 3.) Start Update Timer
                                //TimedUpdates.Start(useWallet, wallet, Daemon);

                            }
                            break;
                        default:
                            break;
                    }
                }
                else
                {

                    // Functions
                    switch (cmd)
                    {
                        case "exit":
                            await new Exit().Run(Daemon, wallet, walletName);
                            break;
                        case "initwallet":
                            await new InitWallet().Run(Daemon, wallet);
                            break;
                        case "unlock":
                            await new StakeWallet().Run(Daemon, wallet, false, walletName);
                            break;
                        case "stake":
                            await new StakeWallet().Run(Daemon, wallet, true, walletName);
                            break;
                        case "":
                            // Write out Balance and Sync Status
                            ConsoleWriter.Info($"Getting balance and sync status update...");

                            var getInfoProc = new GetInfo(Daemon.settings(walletName), wallet);
                            await getInfoProc.Run();
                            if (!getInfoProc.Rpc.ErrorMsg.Contains("You must set rpcpassword=<password> in the configuration file:"))
                            {
                                var syncStats = await new SyncStatus().Run(Daemon, wallet, walletName);
                                ConsoleWriter.Response($"Balance: {getInfoProc.Rpc.Result?.Balance}, [{syncStats.BlockCount}/{syncStats.BlockCountOfPeers}] ({syncStats.Difference})");
                            }
                            break;
                        default:
                            if (!string.IsNullOrEmpty(cmd))
                            {
                                CustomCommand cstmCmd = new CustomCommand(Daemon.settings(walletName), wallet);
                                await cstmCmd.Run(cmd);

                                if (!cstmCmd.TimedOut)
                                {
                                    ConsoleWriter.Response($"ExitCode: {cstmCmd.Rpc.ExitCode}");
                                    ConsoleWriter.Response($"ErrorMsg: {cstmCmd.Rpc.ErrorMsg}");
                                    ConsoleWriter.Response($"Output: {cstmCmd.Rpc.OutputMsg}");
                                }

                            }
                            break;

                    }

                }



            }
        }

        private static void c_WalletNotification(object sender, NotificationEventArgs e)
        {
            // e.Tx is a Blockhash
            ConsoleWriter.Important($"Wallet Notification: {e.Tx}");
        }

        private static void c_BlockNotification(object sender, NotificationEventArgs e)
        {
            // e.Tx is a Blockhash
            ConsoleWriter.Notification($"Block Notification: {e.Tx}");
        }


    }
}
