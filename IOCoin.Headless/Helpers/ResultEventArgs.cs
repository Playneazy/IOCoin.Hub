using IOCoin.Headless.Responses.RPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCoin.Headless.Helpers
{
    public class ResultEventArgs<ResultType>
    {
        public string cmdName { get; set; }
        public RPC<ResultType> Rpc { get; set; }
        public int TimeoutSec { get; set; }
        public bool isTimedOut { get; set; }
        public string Args { get; set; }

    }
}
