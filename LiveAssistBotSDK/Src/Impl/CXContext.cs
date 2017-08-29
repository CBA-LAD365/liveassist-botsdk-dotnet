using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Cafex.LiveAssist.Bot.Impl
{
    internal class CXContext
    {
        [JsonProperty(PropertyName = "accountId")]
        internal string AccountId { get; set; }

        [JsonProperty(PropertyName = "contextData")]
        internal string ContextData { get; set; }
    }
}
