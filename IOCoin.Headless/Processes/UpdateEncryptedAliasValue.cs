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
    public class UpdateEncryptedAliasValue : ProcessBase<string>
    {
        public UpdateEncryptedAliasValue(Settings settings, IWallet wallet) : base(settings, wallet)
        {

        }
        public override string CmdName => "updatencryptedaliasvalue";

        public override string CmdDesc => "Updates an encrypted alias's value. Fee is 0.01 per kb.";
        public override int TimeoutSec { get; set; } = 30;
        public override double TxFee { get; set; } = 0.02;


        public async Task<UpdateEncryptedAliasValue> Run(string alias, string value, string aliasAddress)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Arguments = settings.daemonArgBase + "updateEncryptedAlias " + alias + " " + value + " " + aliasAddress;
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
    }
}
