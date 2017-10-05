
using System.Collections.Generic;

namespace Cafex.LiveAssist.Bot
{
    /// <summary>
    /// Return any context data, with with provided context data ID.
    /// </summary>
    /// <param name="contextId">The ContextId, provided by the Live Assist SDK</param>
    /// <returns></returns>
    public delegate string ContextDataDelagate(string contextId);

    /// <summary>
    /// An instance of <c>ChatSpec</c> encapsulates properties to start a new chat.
    /// </summary>
    public class ChatSpec
    {
        /// <summary>
        /// Target an agent with a specific skill. (Optional)
        /// </summary>
        public string Skill { get; set; }

        /// <summary>
        ///  A list of transcript lines that are shown to the agent before the chat starts.
        ///  Typically these would be used to convey a transcript of a preceding BOT chat. (optional)
        /// </summary>
        public List<TranscriptLine> Transcript { get; set; }

        /// <summary>
        /// Name of the visitor. (optional)
        /// </summary>
        public string VisitorName { get; set; }

        /// <summary>
        /// If set, the Live Assist SDK will invoke this delagate to retrieve context data.
        /// If set, <see cref="Cafex.LiveAssist.Bot.SdkConfiguration.ContextDataHost"/> must also be set.
        /// </summary>
        public ContextDataDelagate ContextData { get; set; }

        /// <summary>
        /// Language hint. Default en-US (optional)
        /// The language specification must be of the form LANG-COUNTRY where:
        /// LANG is a lowercase 2 character code as specified in ISO 639-1 and COUNTRY is an upppercase 2 character code as specified in ISO_3166-1_alpha-2.
        /// Support for the specified language must have been configured in your Live Assist account.
        /// </summary>
        public string Language { get; set; } = "en-US";
    }
}
