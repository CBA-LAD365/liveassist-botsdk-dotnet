using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace Cafex.LiveAssist.Bot.Impl
{
    internal class LPAvailability
    {
        /*
           {
                "availability": true
           }   
       */
        [JsonProperty(PropertyName = "availability")]
        internal bool Availability { get; set; }
    }
}
