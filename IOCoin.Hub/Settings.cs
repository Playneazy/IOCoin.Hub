using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCoin.Console
{
    public class Settings
    {
        [JsonProperty]
        public int updateintervalMin { get; set; }
    }
}
