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
    public class SendMessage : ProcessBase<string>
    {
        public SendMessage(Settings settings, IWallet wallet) : base(settings, wallet)
        {

        }
        public override string CmdName => "sendmessage";

        public override string CmdDesc => "Send an encrypted message over an established channel. First setup using PublicKeyInvite.";
        public override int TimeoutSec { get; set; } = 30;


        public async Task<SendMessage> Run(string fromAddress, string message, string toAddress)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Arguments = settings.daemonArgBase + "sendMessage " + fromAddress + " " + message + " " + toAddress;
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
