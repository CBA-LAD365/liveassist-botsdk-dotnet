using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cafex.LiveAssist.Bot.Impl
{

    /*
    {
        "request": {
            "skill": "my_skill",
            "language" : "en-US",
            "interactionTimeout" : 120
            "preChatLines" : {"line":["line one", "line 2"]}
        }
    }
    */

    internal class ChatRequest
    {
        [JsonProperty(PropertyName = "request")]
        Request request = new Request();

        internal ChatRequest(ChatSpec chatSpec)
        {
            request.Skill = chatSpec.Skill;
            request.Language = chatSpec.Language;
            request.InteractionTimeout = 120;

            if (chatSpec.Transcript != null && chatSpec.Transcript.Count > 0)
            {
                List<string> lines = new List<string>();
                foreach (TranscriptLine line in chatSpec.Transcript)
                {
                    lines.Add(line.Encode());
                }

                request.PreChatLines = new PreChatLines()
                {
                    Lines = lines.ToArray()
                };
                
            }
        }
    }

    internal class Request
    {
       [JsonProperty(PropertyName = "skill", NullValueHandling = NullValueHandling.Ignore)]
       internal String Skill { get; set; }

       [JsonProperty(PropertyName = "preChatLines", NullValueHandling = NullValueHandling.Ignore)]
       internal PreChatLines PreChatLines { get; set; }

       [JsonProperty(PropertyName = "language", NullValueHandling = NullValueHandling.Ignore)]
       internal String Language { get; set; }

        [JsonProperty(PropertyName = "interactionTimeout", NullValueHandling = NullValueHandling.Ignore)]
        internal int InteractionTimeout { get; set; }
    }

    internal class PreChatLines
    {
        [JsonProperty(PropertyName = "line", NullValueHandling = NullValueHandling.Ignore)]
        internal String[] Lines { get; set; }
    }
}
