using Newtonsoft.Json;

namespace Cafex.LiveAssist.Bot.Impl
{
    // { "event":{ "@type":"state","state":"ended"} }
    internal class LPEnd
    { 
        [JsonProperty(PropertyName = "event")]
        internal EndEvent endEvent = new EndEvent();
    }

    internal class EndEvent
    {
        [JsonProperty(PropertyName = "@type")]
        internal string type = "state";

        [JsonProperty(PropertyName = "state")]
        internal string State = "ended"; 
    }
}
