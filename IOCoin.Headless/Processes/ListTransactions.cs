using IOCoin.Headless.Helpers;
using IOCoin.Headless.Processes.Interfaces;
using IOCoin.Headless.Responses;
using IOCoin.Headless.Responses.RPC;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCoin.Headless.Processes
{
    public class ListTransactions : ProcessBase<List<ListTransactionsResponse>>
    {
        public ListTransactions(Settings settings, IWallet wallet) : base(settings, wallet)
        {

        }
        public override string CmdName => "listtransactions";

        public override string CmdDesc => "Retrieves an array of transactions. Can provide a specific address and number of results.";
        public override int TimeoutSec { get; set; } = 30;


        public async Task<ListTransactions> Run()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Arguments = settings.daemonArgBase + "listtransactions";
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            await StartProcess(true, startInfo);

            // Create the Rpc.Result
            if (Rpc.ExitCode != 0)
            {
                await HandleResult(null, startInfo);
                return this;
            }

            var res = JsonConvert.DeserializeObject<List<ListTransactionsResponse>>(Rpc.OutputMsg);
            Wallet.Transactions = res;
            await HandleResult(res, startInfo, true);

            return this;
        }
    }
}
