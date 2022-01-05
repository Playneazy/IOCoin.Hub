using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCoin.Headless.Interfaces
{
    public interface IProcess
    {
        public string CmdName { get; }
        public string CmdDesc { get; }
        public int TimeoutSec { get; set; }
        public bool TimedOut { get; set; }
        public double TxFee { get; set; }
    }
}
