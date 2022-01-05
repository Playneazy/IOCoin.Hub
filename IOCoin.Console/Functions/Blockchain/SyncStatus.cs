using IOCoin.Headless;
using IOCoin.Headless.Processes;
using IOCoin.Console.Helpers;
using IOCoin.Console.Logic.Results;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IOCoin.Wallet;

namespace IOCoin.Console.Functions.Blockchain
{
    public class SyncStatus
    {
        public async Task<Logic.Results.SyncStatus> Run(Daemon Daemon, Info wallet)
        {
            var getBlockCount = new GetBlockCount(Daemon.settings, wallet);
            var blockCount = await getBlockCount.Run();
            wallet.BlockHeight = blockCount.Rpc.Result;

            var getNumBlocksOfPeers = new GetNumBlocksOfPeers(Daemon.settings, wallet);
            var blockCountOfPeers = await getNumBlocksOfPeers.Run();
            wallet.PeerBlockCount = blockCountOfPeers.Rpc.Result;

            var diff = blockCountOfPeers.Rpc.Result - blockCount.Rpc.Result;

            var res = new Logic.Results.SyncStatus();
            res.BlockCount = blockCount.Rpc.Result;
            res.BlockCountOfPeers = blockCountOfPeers.Rpc.Result;
            res.Difference = diff;

            if (res.Difference >= 0) 
                wallet.isSynced = false;
            else
                wallet.isSynced = true;

            return res;
        }
    }
}
