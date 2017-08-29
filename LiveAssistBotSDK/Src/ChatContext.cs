using System;

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
    [Serializable]
    public class ChatContext
    {
        internal string SessionUrl { get; set; }
        internal string NextEvents { get; set; }

        internal ChatContext(string sessionUrl, string nextEvents)
        {
            this.SessionUrl = sessionUrl;
            this.NextEvents = nextEvents;
        }
    }
}
