using IOCoin.Headless.Events;
using IOCoin.Headless.Helpers;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WatsonWebsocket;

namespace IOCoin.Headless
{
    public class WebServer
    {
        public delegate void NotificationEventHandler(object sender, NotificationEventArgs e);

        public event NotificationEventHandler BlockNotification;
        public event NotificationEventHandler WalletNotification;

        public static HttpListener listener;
        public WebServer(WalletConfig settings)
        {

            BackgroundWorker bw = new BackgroundWorker();
            bw.WorkerReportsProgress = false;

            // Run the server in the background
            bw.DoWork += new DoWorkEventHandler(
            delegate (object o, DoWorkEventArgs args)
            {
                listener = new HttpListener();
                listener.Prefixes.Add(settings.notificationAddress);
                listener.Start();
                //Log.Debug("Listening for daemon Notificaitons on {0}", settings.notificationAddress);

                BackgroundWorker b = o as BackgroundWorker;

                Task listenTask = HandleIncomingConnections();
                listenTask.GetAwaiter().GetResult();

            });

            bw.RunWorkerAsync();


        }



        public async Task HandleIncomingConnections()
        {
            bool runServer = true;

            // While a user hasn't visited the `shutdown` url, keep on handling requests
            while (runServer)
            {
                // Will wait here until we hear from a connection
                HttpListenerContext ctx = await listener.GetContextAsync();

                // Peel out the requests and response objects
                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse resp = ctx.Response;

                // If `shutdown` url requested w/ POST, then shutdown the server after serving the page
                if (req.Url.AbsolutePath == "/notify/block")
                {
                    //Log.Debug($"#### Block Notification! - {req.QueryString["trx"]}");

                    NotificationEventArgs args = new NotificationEventArgs();
                    args.Tx = req.QueryString["trx"];

                    OnBlockNotification(args);
                } else
                if (req.Url.AbsolutePath == "/notify/wallet")
                {
                    //Log.Debug($"#### Wallet Notification! - {req.QueryString["trx"]}");

                    NotificationEventArgs args = new NotificationEventArgs();
                    args.Tx = req.QueryString["trx"];

                    OnWalletNotification(args);
                }

                // Write the response info
                //string disableSubmit = !runServer ? "disabled" : "";
                //byte[] data = Encoding.UTF8.GetBytes(String.Format(pageData, pageViews, disableSubmit));
                //resp.ContentType = "text/html";
                //resp.ContentEncoding = Encoding.UTF8;
                //resp.ContentLength64 = data.LongLength;

                // Write out to the response stream (asynchronously), then close it
                //await resp.OutputStream.WriteAsync(data, 0, data.Length);
                resp.Close();
            }
        }
        protected virtual void OnBlockNotification(NotificationEventArgs e)
        {
            NotificationEventHandler handler = BlockNotification;
            handler?.Invoke(this, e);
        }
        protected virtual void OnWalletNotification(NotificationEventArgs e)
        {
            NotificationEventHandler handler = WalletNotification;
            handler?.Invoke(this, e);
        }
    }
}
