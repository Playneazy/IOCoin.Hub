using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCoin.Headless.Responses
{
    public class ShadeResponse
    {
        [JsonProperty("abskey")]
        public string absKey { get; set; }
        [JsonProperty("ordKey")]
        public string ordKey { get; set; }
        [JsonProperty("shade")]
        public string Shade { get; set; }
    }
}
