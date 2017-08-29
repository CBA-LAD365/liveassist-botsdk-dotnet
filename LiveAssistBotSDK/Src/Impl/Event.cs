using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Cafex.LiveAssist.Bot.Impl
{
    internal class Event
    {
        [JsonProperty(PropertyName = "@id")]
        internal string id { get; set; }

        [JsonProperty(PropertyName = "@type")]
        internal string type { get; set; }

        [JsonProperty(PropertyName = "time", NullValueHandling = NullValueHandling.Ignore)]
        internal string time { get; set; }

        [JsonProperty(PropertyName = "state", NullValueHandling = NullValueHandling.Ignore)]
        internal string state { get; set; }

        [JsonProperty(PropertyName = "text", NullValueHandling = NullValueHandling.Ignore)]
        internal string text { get; set; }

        [JsonProperty(PropertyName = "textType", NullValueHandling = NullValueHandling.Ignore)]
        internal string textType { get; set; }

        [JsonProperty(PropertyName = "by", NullValueHandling = NullValueHandling.Ignore)]
        internal string by { get; set; }

        [JsonProperty(PropertyName = "source", NullValueHandling = NullValueHandling.Ignore)]
        internal string source { get; set; }

        [JsonProperty(PropertyName = "systemMessageId", NullValueHandling = NullValueHandling.Ignore)]
        internal Int64 systemMessageId { get; set; }

        [JsonProperty(PropertyName = "subtype", NullValueHandling = NullValueHandling.Ignore)]
        internal string subtype { get; set; }
    }
}
