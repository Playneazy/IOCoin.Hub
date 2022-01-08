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
    public class DownloadDecryptAliasFile : ProcessBase<bool>
    {
        public DownloadDecryptAliasFile(WalletConfig settings, IWallet wallet) : base(settings, wallet)
        {

        }
        public override string CmdName => "downloaddecryptaliasfile";

        public override string CmdDesc => "Download a file from an alias and save it to a local file.";
        public override int TimeoutSec { get; set; } = 30;


        public async Task<DownloadDecryptAliasFile> Run(string alias, string filepath)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Arguments = settings.daemonArgBase + "downloadDecrypt " + alias + " " + filepath;
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            await StartProcess(true, startInfo);

            // Create the Rpc.Result
            if (Rpc.ExitCode != 0)
            {
                await HandleResult(false, startInfo);
                return this;
            }


            await HandleResult(Convert.ToBoolean(Rpc.OutputMsg), startInfo);
            return this;
            
        }
    }
}
