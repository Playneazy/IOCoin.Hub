using IOCoin.Console.Functions.Blockchain;
using IOCoin.Console.Helpers;
using IOCoin.Headless;
using IOCoin.Headless.Processes;
using IOCoin.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOCoin.Console
{
    public static class TimedUpdates
    {
        private static Timer timer { get; set; }
        private static Settings settings { get; set; }
        private static Info wallet { get; set; }
        private static Daemon Daemon { get; set; }

        private static bool ProcessingUpdate { get; set; } = false;


        private static int runtime_timerTicks { get; set; } = 0;
        private static int update_timerTicks { get; set; } = 0;

        private static string walletName { get; set; }


        public static async void Start(Settings Settings, Info Wallet, Daemon daemon, string WalletName)
        {
            walletName = WalletName;
            wallet = Wallet;
            Daemon = daemon;
            settings = Settings;
            timer = new Timer(
                new TimerCallback(TimerTick),
                null,
                1000,
                1000);
        }

        public static async void TimerTick(object state)
        {
            // Timer ticks at 1 sec intervals
            update_timerTicks += 1;
            runtime_timerTicks += 1;

            var runtimeSecToMin = Math.Round((Double)runtime_timerTicks / 60, 2);
            var updateSecToMin = Math.Round((Double)update_timerTicks / 60, 2);

            if (updateSecToMin >= settings.updateintervalMin && !ProcessingUpdate && wallet.isSynced)
            {
                ProcessingUpdate = true;        // Makes sure another thread at a new tick doesn't execute while processing.

                // Update Core Wallet Variables
                _ = await new SyncStatus().Run(Daemon, wallet, walletName);
                _ = await new WalletLockStatus(Daemon.settings(walletName), wallet).Run();
                _ = await new GetStakingInfo(Daemon.settings(walletName), wallet).Run();
                _ = await new GetInfo(Daemon.settings(walletName), wallet).Run();
                _ = await new ListTransactions(Daemon.settings(walletName), wallet).Run();
                _ = await new AliasList(Daemon.settings(walletName), wallet).Run();

                // Output update
                ConsoleWriter.Update($"{DateTime.Now})");
                ConsoleWriter.Update($"Versioning: {wallet.Version} [{wallet.ProtocolVersion}] ({wallet.WalletVersion})");
                ConsoleWriter.Update($"Online: {wallet.isOnline}, IP: [{wallet.IP}]");
                ConsoleWriter.Update($"Synced: {wallet.isOnline}");
                if (wallet.isWalletInitialized)
                {
                    ConsoleWriter.Update($"Encrypted: {wallet.isEncrypted}");
                    ConsoleWriter.Update($"Locked: {wallet.isLocked}");
                    ConsoleWriter.Update($"Staking: {wallet.isStaking}");
                }
                ConsoleWriter.Update($"------------------------------");
                ConsoleWriter.Update($"Balance: {wallet.Balance}");
                ConsoleWriter.Update($"Pending: {wallet.Pending}");
                ConsoleWriter.Update($"New Mint: {wallet.NewMint}");
                ConsoleWriter.Update($"Stake: {wallet.Stake}");
                ConsoleWriter.Update($"Transactions: {wallet.Transactions?.Count}");
                ConsoleWriter.Update($"Aliases: {wallet.Aliases?.Count}");
                ConsoleWriter.Update($"------------------------------");
                ConsoleWriter.Update($"Block Height: {wallet.BlockHeight}");
                ConsoleWriter.Update($"Keypool Size: {wallet.KeypoolSize}");
                ConsoleWriter.Update($"Keypool Oldest: {wallet.KeypoolOldest}");


                update_timerTicks = 0;      // Reset elapsed interval
                ProcessingUpdate = false;
                return;
            }


        }
        public static void Stop() => timer.Change(Timeout.Infinite, Timeout.Infinite);
    }
}
