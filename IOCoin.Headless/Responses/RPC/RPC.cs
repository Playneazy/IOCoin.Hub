using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCoin.Headless.Responses.RPC
{
    public class RPC<ResultType>
    {
        [JsonProperty]
        public string ErrorMsg { get; set; }
        [JsonProperty]
        public string OutputMsg { get; set; }
        [JsonProperty]
        public int ExitCode { get; set; }

        [JsonProperty]
        public ResultType Result { get; set; }
    }
}
