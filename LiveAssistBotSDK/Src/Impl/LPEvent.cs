using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Cafex.LiveAssist.Bot.Impl
{
    internal class LPEvent
    {
        [JsonProperty(PropertyName = "chat")]
        internal Chat chat { get; set; }

        internal ChatInfo getChatInfo()
        {
            ChatInfo chatInfo = new ChatInfo();

            chatInfo.AgentName = chat.info.agentName;
            chatInfo.AgentTyping = chat.info.agentTyping;
            chatInfo.ChatTimeout = chat.info.chatTimeout;
            chatInfo.Duration = chat.info.duration;
            chatInfo.StartTime = chat.info.startTime;
            chatInfo.LastUpdate = chat.info.lastUpdate;
            chatInfo.State = chat.info.state;

            if (chat.events != null && chat.events.eventsList != null && chat.events.eventsList.Count > 0)
            {
                List<ChatEvent> events = new List<ChatEvent>();
                foreach (Event e in chat.events.eventsList)
                {
                     if (e.source != null && e.source.Equals("system") && e.systemMessageId == 0)
                     {
                    // This is to ignore pre-chat lines.
                       continue;
                    }

                    ChatEvent chatEvent = new ChatEvent();

                    chatEvent.Type = e.type;
                    chatEvent.Source = e.source;
                    chatEvent.State = e.state;
                    chatEvent.Text = e.text;
                    chatEvent.Time = e.time;
                    chatEvent.Id = e.id;

                    events.Add(chatEvent);
  
                }
                chatInfo.ChatEvents = events;
            }
            return chatInfo;
        }
    }

    internal class Chat
    {
        [JsonProperty(PropertyName = "link")]
        internal List<Link> link { get; set; }

        [JsonProperty(PropertyName = "events")]
        internal Events events { get; set; }

        [JsonProperty(PropertyName = "info")]
        internal Info info { get; set; }
    }

    internal class Link
    {
        [JsonProperty(PropertyName = "@href")]
         internal string href { get; set; }

        [JsonProperty(PropertyName = "@rel")]
        internal string rel { get; set; }
    }

    internal class Events
    {
        [JsonProperty(PropertyName = "link", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(SingleOrArrayConverter<Link>))]
        internal List<Link> linksList { get; set; }

        [JsonProperty(PropertyName = "event", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(SingleOrArrayConverter<Event>))]
        internal List<Event> eventsList { get; set; }
    }
}
