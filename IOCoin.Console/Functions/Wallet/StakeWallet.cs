using IOCoin.Headless;
using IOCoin.Headless.Processes;
using IOCoin.Console.Functions.Blockchain;
using IOCoin.Console.Helpers;
using IOCoin.Console.Logic;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IOCoin.Wallet;

namespace IOCoin.Console.Functions.Wallet
{
    public class StakeWallet
    {
        public async Task Run(Daemon Daemon, Info wallet, bool stakeOnly, string walletName)
        {
            // 1.) Check sync status
            ConsoleWriter.Info($"Checking sync status...");
            var syncStats = await new SyncStatus().Run(Daemon, wallet, walletName);

            // 2.) Check if wallet is Encrypted
            ConsoleWriter.Info($"Checking wallet encryption...");
            var lockStats = await new WalletLockStatus(Daemon.settings(walletName), wallet).Run();

            if (!lockStats.Rpc.Result.isEncrypted)
            {
                // 2a.) If not, Encrypt Wallet with passphrase
                ConsoleWriter.Important($"No encryption found, encrypting wallet.");
                EncryptWallet encryptWallet = new EncryptWallet(Daemon.settings(walletName), wallet);
                await encryptWallet.Run();
                lockStats.Rpc.Result.isEncrypted = true;

            }

            // #### Wait Until Sync is complete ####
            if (syncStats.Difference > 0)
            {
                ConsoleWriter.Info($"Waiting for blockchain sync...");
                do
                {
                    syncStats = await new SyncStatus().Run(Daemon, wallet, walletName);
                    ConsoleWriter.Response($"Syncing - [{syncStats.BlockCount}/{syncStats.BlockCountOfPeers}] ({syncStats.Difference})");

                } while (syncStats.Difference > 0);
                ConsoleWriter.Info($"Wallet synced with chain.");
            }
            // #### End Wait ####

            // Wallet should now be encrypted, synced, and locked.
            if (lockStats.Rpc.Result.isEncrypted && lockStats.Rpc.Result.isLocked)
            {
                // 3.) Unlock wallet for Staking
                if (stakeOnly)
                    ConsoleWriter.Info($"Unlocking wallet for staking...");
                else
                    ConsoleWriter.Info($"Unlocking wallet...");

                var walletPassPhrase = new WalletPassPhrase(Daemon.settings(walletName), wallet).Run(3155695200000, stakeOnly); // 100 years

                // 3a.) Check if Ecrypted and Unlocked again
                do
                {
                    lockStats = await new WalletLockStatus(Daemon.settings(walletName), wallet).Run();
                    if (lockStats.Rpc.Result.isEncrypted && !lockStats.Rpc.Result.isLocked)
                    {
                        // Daemon is Staking!
                        if (stakeOnly)
                            ConsoleWriter.Response($"Wallet unlocked and should be staking...");
                        else
                            ConsoleWriter.Info($"Wallet unlocked.");

                    }
                } while (lockStats.Rpc.Result.isLocked);

            }
            else if (!lockStats.Rpc.Result.isLocked)
            {
                if (stakeOnly)
                    ConsoleWriter.Response($"Wallet unlocked and should be staking...");
                else
                    ConsoleWriter.Info($"Wallet unlocked.");
            }

            if (!lockStats.Rpc.Result.isLocked)
            {
                // 4.) Check if staking is enabled
                ConsoleWriter.Info($"Checking if staking is enabled...");
                var stakingStats = await new GetStakingInfo(Daemon.settings(walletName), wallet).Run();

                if (stakingStats.Rpc.Result.Enabled)
                {
                    ConsoleWriter.Response($"Staking enabled.");
                } else
                {
                    await Run(Daemon, wallet, stakeOnly, walletName);
                }
            }
            

        }
    }
}
