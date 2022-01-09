using IOCoin.Headless.Interfaces;
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
    public class SendMany : ProcessBase<string>
    {
        public SendMany(WalletConfig settings, IWallet wallet) : base(settings, wallet)
        {

        }
        public override string CmdName => "sendmany";

        public override string CmdDesc => "Send from one account to multiple accounts.";
        public override int TimeoutSec { get; set; } = 30;


        public async Task<SendMany> Run(string fromAccount, IEnumerable<SendToAccount> toAccounts)
        {
            var accountsCombine = new List<string>();
            foreach (var account in toAccounts)
                accountsCombine.Add(account.Account + ":" + account.Amount);

            if (accountsCombine.Count == 0) return this;

            var joinedAccounts = string.Join(",", accountsCombine);

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Arguments = settings.daemonArgBase + "sendmany " + fromAccount + " " + joinedAccounts;
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            await StartProcess(true, startInfo);

            // Create the Rpc.Result
            if (Rpc.ExitCode != 0)
            {
                await HandleResult(String.Empty, startInfo);
                return this;
            }


            await HandleResult(Rpc.OutputMsg, startInfo);

            return this;
            
        }

        public class SendToAccount
        {
            public string Account { get; set; }
            public decimal Amount { get; set; }
        }
    }
}
