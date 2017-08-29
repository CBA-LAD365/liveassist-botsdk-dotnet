using Newtonsoft.Json;

namespace Cafex.LiveAssist.Bot.Impl
{
    internal class BaseUri
    {
        /*
        Example response:
          {
                "service":"conversationVep",
                "account":"__CHANGE_ME__",
                "baseURI":"va-a.convep.liveperson.net"
          }
        */

        [JsonProperty(PropertyName = "service")]
        internal string service { get; set; }

        [JsonProperty(PropertyName = "account")]
        internal string account { get; set; }

        [JsonProperty(PropertyName = "baseURI")]
        internal string baseURI { get; set; }
    }
}
