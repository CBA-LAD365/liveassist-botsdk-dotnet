using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cafex.LiveAssist.Bot
{
    /// <summary>
    /// A references to a chat.
    /// </summary>
    /// <remarks>
    /// A <c>ChatContext</c> is created and returned by <see cref="Cafex.LiveAssist.Bot.Sdk.RequestChat(ChatSpec)"/>
    /// 
    /// Subsequent operations on a chat, via <see cref="Cafex.LiveAssist.Bot.Sdk"/> require an instance of <c>ChatContext</c>
    /// 
    /// <c>ChatContext</c> is serializable and as such may be persisted to storage; however it should be noted that 
    /// SDK operations that take a <c>ChatContext</c> as a parameter may update its internal state; in this case, a serialized and stored <c>ChatContext</c>
    /// must be updated.
    /// </remarks>
    [Serializable()]
    [DataContract]
    public class ChatContext
    {
        [DataMember]
        internal string SessionUrl { get; set; }

        [DataMember]
        internal string NextEvents { get; set; }

        public ChatContext(string sessionUrl, string nextEvents)
        {
            this.SessionUrl = sessionUrl;
            this.NextEvents = nextEvents;
        }

        public override bool Equals(object obj)
        {
            var context = obj as ChatContext;
            return context != null &&
                   SessionUrl == context.SessionUrl &&
                   NextEvents == context.NextEvents;
        }

        public override int GetHashCode()
        {
            var hashCode = 1722029513;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SessionUrl);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(NextEvents);
            return hashCode;
        }
    }
}
