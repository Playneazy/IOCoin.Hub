using IOCoin.Headless.Helpers;
using IOCoin.Headless.Processes.Interfaces;
using IOCoin.Headless.Responses;
using IOCoin.Headless.Responses.RPC;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCoin.Headless.Processes
{
    public abstract class ProcessBase<ResponseType> : IProcess, IRPC<ResponseType>
    {
        [JsonIgnore]
        public abstract string CmdName { get; }
        [JsonIgnore]
        public abstract string CmdDesc { get; }
        public abstract int TimeoutSec { get; set; }
        public bool TimedOut { get; set; } = false;
        public virtual double TxFee { get; set; } = 0;

        public RPC<ResponseType> Rpc { get; set; } = new RPC<ResponseType>();

        public IWallet Wallet { get; set; }     // Expose access to wallet in order to set variables at the lowest level of 'Headless' calls

        public delegate void ResultEventHandler(object sender, ResultEventArgs<ResponseType> e);
        public event ResultEventHandler Result;

        public Settings settings { get; set; }

        


        public ProcessBase(Settings Settings, IWallet wallet)
        {
            settings = Settings;
            Wallet = wallet;
        }


        public async Task StartProcess(bool waitForExit, ProcessStartInfo startInfo)
        {
            Process process = new Process();

            process.StartInfo = startInfo;
            process.StartInfo.FileName = settings.daemonPath;
            process.Start();

            // Check Timeout while we wait for process to complete
            int timeoutCount = 0;
            bool isTimedOut = false;
            if (waitForExit)
            {
                do
                {
                    await Task.Delay(1000);
                    timeoutCount += 1;
                    
                    //Log.Debug($"Timing out in [{TimeoutSec - timeoutCount}] seconds...");

                    if (timeoutCount >= TimeoutSec)
                    {
                        isTimedOut = true;
                        try
                        {
                            process.Close();
                            process.Kill();
                        }
                        catch (Exception ex)
                        {
                            Log.Error($"Timed out on Arguements: {startInfo.Arguments}");
                        }
                        break;
                    }
                } while (!process.HasExited);
            }

            if (!isTimedOut && process.StartInfo.RedirectStandardError && process.StartInfo.RedirectStandardOutput)
            {
                Rpc.ErrorMsg = process.StandardError?.ReadToEnd();
                Rpc.OutputMsg = process.StandardOutput?.ReadToEnd();
            }

            if (!isTimedOut && process.HasExited)
                Rpc.ExitCode = process.ExitCode;
            else
                Rpc.ExitCode = -1;

            if (!string.IsNullOrEmpty(Rpc.ErrorMsg)) Log.Error(Rpc.ErrorMsg);

            if (timeoutCount >= TimeoutSec)
                TimedOut = true;
            else
                TimedOut = false;

            process.Close();

            return;
        }

        protected virtual void OnResult(ResultEventArgs<ResponseType> e)
        {
            ResultEventHandler handler = Result;
            handler?.Invoke(this, e);
        }

        public async Task HandleResult(ResponseType result, ProcessStartInfo startInfo, bool FireWalletUpdate = false)
        {
            Rpc.Result = result;

            var resArgs = new ResultEventArgs<ResponseType>();
            resArgs.isTimedOut = TimedOut;
            resArgs.TimeoutSec = TimeoutSec;
            resArgs.cmdName = CmdName;
            resArgs.Rpc = Rpc;
            resArgs.Args = startInfo.Arguments;

            if (FireWalletUpdate)
                Wallet.Update(new WalletUpdateEventArgs(Wallet));
            
            OnResult(resArgs);
            return;
        }

    }
}
