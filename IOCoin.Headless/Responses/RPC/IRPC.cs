using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCoin.Headless.Responses.RPC
{
    public interface IRPC<ResultType>
    {
        public RPC<ResultType> Rpc { get; set; }
    }
}
