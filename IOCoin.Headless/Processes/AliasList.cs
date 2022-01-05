﻿using IOCoin.Headless.Helpers;
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
    public class AliasList : ProcessBase<List<AliasResponse>>
    {
        public AliasList(Settings settings, IWallet wallet) : base(settings, wallet)
        {

        }
        public override string CmdName => "AliasList";

        public override string CmdDesc => "Retrieves distinct array of aliases and their respective addresses.";
        public override int TimeoutSec { get; set; } = 30;


        public async Task<AliasList> Run()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Arguments = settings.daemonArgBase + "aliasList";
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

            var res = JsonConvert.DeserializeObject<List<AliasResponse>>(Rpc.OutputMsg);
            Wallet.Aliases = res;
            await HandleResult(res, startInfo, true);

            return this;
        }
    }
}