using IOCoin.Headless;
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

namespace IOCoin.Console
{
    public static class CommandLine
    {
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
                            if (string.IsNullOrEmpty(alias))
                            {
                                var aliasHash = await new RegisterAlias(Daemon.settings, wallet).Run(cmdArgs.ElementAtOrDefault(1));
                                await new AliasList(Daemon.settings, wallet).Run();
                            }
                                
                            break;
                        case "aliaslist":
                            var aliases = await new AliasList(Daemon.settings, wallet).Run();
                            break;
                        case "listtransactions":
                            var transactions = await new ListTransactions(Daemon.settings, wallet).Run();
                            break;
                        case "stakinginfo":
                            var stakeInfo = await new GetStakingInfo(Daemon.settings, wallet).Run();
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
                            await new Exit().Run(Daemon, wallet);
                            break;
                        case "initwallet":
                            await new InitWallet().Run(Daemon, wallet);
                            break;
                        case "unlock":
                            await new StakeWallet().Run(Daemon, wallet, false);
                            break;
                        case "stake":
                            await new StakeWallet().Run(Daemon, wallet, true);
                            break;
                        case "":
                            // Write out Balance and Sync Status
                            ConsoleWriter.Info($"Getting balance and sync status update...");

                            var getInfoProc = new GetInfo(Daemon.settings, wallet);
                            await getInfoProc.Run();
                            if (!getInfoProc.Rpc.ErrorMsg.Contains("You must set rpcpassword=<password> in the configuration file:"))
                            {
                                var syncStats = await new SyncStatus().Run(Daemon, wallet);
                                ConsoleWriter.Response($"Balance: {getInfoProc.Rpc.Result?.Balance}, [{syncStats.BlockCount}/{syncStats.BlockCountOfPeers}] ({syncStats.Difference})");
                            }
                            break;
                        default:
                            if (!string.IsNullOrEmpty(cmd))
                            {
                                CustomCommand cstmCmd = new CustomCommand(Daemon.settings, wallet);
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
    }
}
