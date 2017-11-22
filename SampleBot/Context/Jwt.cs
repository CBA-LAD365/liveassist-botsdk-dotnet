using System;
using System.Collections.Generic;
using Jose;
using System.Security.Cryptography.X509Certificates;

namespace EscalationBot.Context
{
    /// <summary>
    /// Utility class for constructing a Signed JWT
    /// </summary>
    public class Jwt
    {
        static string pass = "__CHANGE_ME__"; 

        /// <summary>
        /// X509 Certificate, with public and private keys.
        /// </summary>
        static string certBase64 = "__CHANGE_ME__";


        /// <summary>
        /// Create a JWT using provided context data and sign using private key from certBase64.
        /// Note: The matching public must be provisioned to Live Assist via the Live Assist Management Portal 
        /// </summary>
        /// <param name="contextId"> ID for the context data, as provided by Live Assist Chat SDK</param>
        /// <param name="contextData">Context data, provided by Chat visitor </param>
        /// <returns></returns>
        public static String Create(string contextId, ContextData contextData)
        {
            var payload = new Dictionary<string, object>()
            {
                { "contextId", contextId},
                { "contextData", contextData }
            };

            JWT.DefaultSettings.JsonMapper = new NewtonsoftMapper();

            var cert = new X509Certificate2(Convert.FromBase64String(certBase64), pass);
            var privateKey = cert.GetRSAPrivateKey();

            string token = JWT.Encode(payload, privateKey, JwsAlgorithm.RS256);
            return token;
        }
    }   
}