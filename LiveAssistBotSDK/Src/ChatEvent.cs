using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafex.LiveAssist.Bot
{
    /// <summary>
    /// An event on a chat, representing either a line of text or a state change.
    /// </summary>
    public class ChatEvent
    {
        /// <summary>
        /// Event ID, unique within the context of a chat.
        /// </summary>
        public string Id { get; internal set; }

        /// <summary>
        /// Type of event, value is either "state" or "line"
        /// </summary>
        public string Type { get; internal set; }

        /// <summary>
        /// Time at which the event occured
        /// </summary>
        public string Time { get; internal set; }

        /// <summary>
        /// If <see cref="Type"/> is "state", this value is set to one of the following:   "waiting", "chatting", "ended"
        /// </summary>
        public string State { get; internal set; }

        /// <summary>
        /// If <see cref="Type"/> is "line", this value is set to the line of text typed
        /// </summary>
        public string Text { get; internal set; }

        /// <summary>
        /// The source of the event, either  "system", "agent" or "visitor".
        /// </summary>
        public string Source { get; internal set; }

        internal ChatEvent() { }
    }
}
