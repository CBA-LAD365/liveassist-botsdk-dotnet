using System;
using System.Collections.Generic;

namespace Cafex.LiveAssist.Bot
{
    /// <summary>
    /// Encapsulates the result of method <see cref="Cafex.LiveAssist.Bot.Sdk.Poll(ChatContext)"/> 
    /// </summary>
    public class ChatInfo
    {
        /// <summary>
        /// List of events that have occured since last call to  <see cref="Cafex.LiveAssist.Bot.Sdk.Poll(ChatContext)"/>
        /// </summary>
        public List<ChatEvent> ChatEvents { get; internal set; }

        /// <summary>
        /// Agent's name
        /// </summary>
        public string AgentName { get; internal set; }

        /// <summary>
        /// The time the chat started.
        /// </summary>
        public string StartTime { get; internal set; }

        /// <summary>
        /// Duration of chat
        /// </summary>
        public Int64 Duration { get; internal set; }

        /// <summary>
        /// The last time that any request was sent to the chat session.
        /// </summary>
        public string LastUpdate { get; internal set; }

        /// <summary>
        /// The time in seconds from the last update time, after which the chat times out and must be updated again before this timeout.
        /// </summary>
        public Int64 ChatTimeout { get; internal set; }

        /// <summary>
        /// Valid values: "typing", "not-typing"
        /// </summary>
        public string AgentTyping { get; internal set; }

        /// <summary>
        /// Current state, Valid values:  "waiting", "chatting", "ended"
        /// </summary>
        public string State { get; internal set; }

        internal ChatInfo() { }
    }
}
