using System.ComponentModel.DataAnnotations;

namespace Cafex.LiveAssist.Bot
{
    /// <summary>
    /// <c>SdkConfiguration</c> encapsulates SDK configuration parameters.
    /// </summary>
    public class SdkConfiguration
    {
        /// <summary>
        /// Host name of the Context Data service. May be unset if Context Data service is not required.
        /// </summary>
        public string ContextDataHost { get; set; }

        /// <summary>
        /// Port of the Context Data Service.
        /// </summary>
        public int ContextDataPort { get; set; } = 443;

        /// <summary>
        /// The Live Assist account number
        /// </summary>
        /// <remarks>
        /// This is a mandatory parameter.
        /// </remarks>
        [Required]
        public string AccountNumber { get; set; } 

        internal string AppKey { get; } = "721c180b09eb463d9f3191c41762bb68"; // This never changes
        internal string ContextDataPath { get; } = "/context-service/context";

    }
}
