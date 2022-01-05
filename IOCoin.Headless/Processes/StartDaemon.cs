using IOCoin.Headless.Interfaces;
using IOCoin.Headless.Responses.RPC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCoin.Headless.Processes
{
    public class StartDaemon : ProcessBase<bool>
    {
        public StartDaemon(Settings settings, IWallet wallet) : base(settings, wallet)
        {

        }

        public override string CmdName => "startdaemon";

        public override string CmdDesc => "Starts the daemon to either create wallet directory files OR run as a Server.";
        public override int TimeoutSec { get; set; } = 30;

        public async Task<StartDaemon> Run(bool createFiles = false)
        {
            string args = settings.daemonArgBase;
            args += createFiles ? "" : "-blocknotify=\"curl http://localhost:8000/notify/block?trx=%s\" -walletnotify=\"curl http://localhost:8000/notify/wallet?trx=%s\" -daemon";

            var startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.Arguments = args;
            await StartProcess(createFiles, startInfo);

            await HandleResult(true, startInfo);

            return this;
        }
    }
}
