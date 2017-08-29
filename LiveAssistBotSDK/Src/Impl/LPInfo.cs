using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Cafex.LiveAssist.Bot.Impl
{
    internal class LPInfo
    {
        [JsonProperty(PropertyName = "info")]
        internal Info Info { get; set; }
    }
}
