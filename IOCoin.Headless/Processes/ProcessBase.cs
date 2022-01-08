using IOCoin.Headless.Events;
using IOCoin.Headless.Helpers;
using IOCoin.Headless.Interfaces;
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
using System.Threading;
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

        public WalletConfig settings { get; set; }

        


        public ProcessBase(WalletConfig Settings, IWallet wallet)
        {
            settings = Settings;
            Wallet = wallet;
        }


        public async Task StartProcess(bool waitForExit, ProcessStartInfo startInfo)
        {
            using (Process process = new Process())
            {
                process.StartInfo = startInfo;
                process.StartInfo.FileName = settings.daemonPath;


                StringBuilder output = new StringBuilder();
                StringBuilder error = new StringBuilder();

                // Read the results asynchronously
                using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
                using (AutoResetEvent errorWaitHandle = new AutoResetEvent(false))
                {
                    process.OutputDataReceived += (sender, e) =>
                    {
                        if (e.Data == null)
                        {
                            outputWaitHandle.Set();
                        }
                        else
                        {
                            output.AppendLine(e.Data);
                        }
                    };
                    process.ErrorDataReceived += (sender, e) =>
                    {
                        if (e.Data == null)
                        {
                            errorWaitHandle.Set();
                        }
                        else
                        {
                            error.AppendLine(e.Data);
                        }
                    };



                    process.Start();

                    if (process.StartInfo.RedirectStandardError && process.StartInfo.RedirectStandardOutput)
                    {
                        process.BeginErrorReadLine();
                        process.BeginOutputReadLine();
                    }



                    if (waitForExit && process.WaitForExit(TimeoutSec * 1000) &&
                        outputWaitHandle.WaitOne(TimeoutSec * 1000) &&
                        errorWaitHandle.WaitOne(TimeoutSec * 1000))
                    {
                        // Process Complete

                        TimedOut = false;
                    }
                    else
                    {
                        // Timed out
                        TimedOut = true;
                        try
                        {
                            //process.Close();
                            //process.Kill();
                        }
                        catch (Exception ex)
                        {
                            Log.Error($"Timed out on Arguements: {startInfo.Arguments}");
                        }

                    }


                }






                if (!TimedOut && process.StartInfo.RedirectStandardError && process.StartInfo.RedirectStandardOutput)
                {
                    Rpc.ErrorMsg = error.ToString();
                    Rpc.OutputMsg = output.ToString();
                }

                if (!TimedOut && process.HasExited)
                    Rpc.ExitCode = process.ExitCode;
                else
                    Rpc.ExitCode = -1;

                if (!string.IsNullOrEmpty(Rpc.ErrorMsg)) Log.Error(Rpc.ErrorMsg);


                
            }
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
