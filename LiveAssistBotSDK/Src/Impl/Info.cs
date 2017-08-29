using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Cafex.LiveAssist.Bot.Impl
{
    internal class Info
    {
        [JsonProperty(PropertyName = "link", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(SingleOrArrayConverter<Link>))]
        internal List<Link> linksList { get; set; }

        [JsonProperty(PropertyName = "state", NullValueHandling = NullValueHandling.Ignore)]
        internal string state { get; set; }

        [JsonProperty(PropertyName = "skillName", NullValueHandling = NullValueHandling.Ignore)]
        internal string skillName { get; set; }

        [JsonProperty(PropertyName = "skillId", NullValueHandling = NullValueHandling.Ignore)]
        internal Int64 skillId { get; set; }

        [JsonProperty(PropertyName = "agentName", NullValueHandling = NullValueHandling.Ignore)]
        internal string agentName { get; set; }

        [JsonProperty(PropertyName = "agentId", NullValueHandling = NullValueHandling.Ignore)]
        internal Int64 agentId { get; set; }

        [JsonProperty(PropertyName = "startTime", NullValueHandling = NullValueHandling.Ignore)]
        internal string startTime { get; set; }

        [JsonProperty(PropertyName = "duration", NullValueHandling = NullValueHandling.Ignore)]
        internal Int64 duration { get; set; }

        [JsonProperty(PropertyName = "lastUpdate", NullValueHandling = NullValueHandling.Ignore)]
        internal string lastUpdate { get; set; }

        [JsonProperty(PropertyName = "chatTimeout", NullValueHandling = NullValueHandling.Ignore)]
        internal Int64 chatTimeout { get; set; }

        [JsonProperty(PropertyName = "visitorId", NullValueHandling = NullValueHandling.Ignore)]
        internal Int64 visitorId { get; set; }

        [JsonProperty(PropertyName = "agentTyping", NullValueHandling = NullValueHandling.Ignore)]
        internal string agentTyping { get; set; }

        [JsonProperty(PropertyName = "visitorTyping", NullValueHandling = NullValueHandling.Ignore)]
        internal string visitorTyping { get; set; }

        [JsonProperty(PropertyName = "visitorName", NullValueHandling = NullValueHandling.Ignore)]
        internal string visitorName { get; set; }

        [JsonProperty(PropertyName = "rtSessionId", NullValueHandling = NullValueHandling.Ignore)]
        internal Int64 rtSessionId { get; set; }
    }
}
