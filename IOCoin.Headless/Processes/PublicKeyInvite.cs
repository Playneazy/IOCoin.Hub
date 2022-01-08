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
    public class PublicKeyInvite : ProcessBase<string>
    {
        public PublicKeyInvite(WalletConfig settings, IWallet wallet) : base(settings, wallet)
        {

        }
        public override string CmdName => "publickeyinvite";

        public override string CmdDesc => "To send or receive an alias users have to send an invite from public alias (A) to public alias (B) as in a RSA key exchange. This initiates an encrypted tunnel giving the ability to transfer aliases and message between users.";
        public override int TimeoutSec { get; set; } = 30;


        public async Task<PublicKeyInvite> Run(string AddressInviteFrom, string AddressInviteTo)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Arguments = settings.daemonArgBase + "sendPublicKey " + AddressInviteFrom + " " + AddressInviteTo;
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
