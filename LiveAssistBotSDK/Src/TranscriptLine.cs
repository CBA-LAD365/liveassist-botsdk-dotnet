using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafex.LiveAssist.Bot
{
    /// <summary>
    /// A line of chat text and associated metadata, used to build a transcript of a chat.
    /// </summary>
    [Serializable]
    public class TranscriptLine
    {
        /// <summary>
        /// DateTime at which the line occured.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// True if this line was written by a bot.
        /// </summary>
        public bool IsBot { get; set; } = false;

        /// <summary>
        /// Name of the author of the line.
        /// </summary>
        public string SrcName { get; set; }

        /// <summary>
        /// Line of text written.
        /// </summary>
        public string Line { get; set; }

        internal string Encode()
        {
            string t = (Timestamp == null) ? "" : Timestamp.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffK");
            string b = IsBot ? "+" : "";
            return $"{t} {b}{SrcName}: {Line}";
        }
    }
}
