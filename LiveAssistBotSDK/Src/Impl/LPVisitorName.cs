using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Cafex.LiveAssist.Bot.Impl
{
    internal class LPVisitorName
    {
        [JsonProperty(PropertyName = "visitorName", NullValueHandling = NullValueHandling.Ignore)]
        internal string VisitorName { get; set; }
    }
}
