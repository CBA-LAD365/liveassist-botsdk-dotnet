using System;

namespace Cafex.LiveAssist.Bot
{
    /// <summary>
    /// Thrown in the event of failure to create or operate on a chat. 
    /// <see cref="Exception.Message"/> for details of the failure.
    /// </summary>
    public class ChatException : Exception
    {
        internal ChatException(String message) : base(message) { }
    }
}
