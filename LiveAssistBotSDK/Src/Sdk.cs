using System;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Cafex.LiveAssist.Bot.Impl;


namespace Cafex.LiveAssist.Bot
{
    /// <summary>
    /// Operations on the Live Assist chat SDK are performed through an instance of this class.
    /// </summary>
    public class Sdk
    {
        private BaseUri baseUri = null;
        private SdkConfiguration sdkConfiguration = null;
        private HttpClient client = new HttpClient();

        /// <summary>
        /// Construct an instance of the SDK with provided configuration.
        /// </summary>
        /// <param name="sdkConfiguration"> Sdk Configuration</param>
        public Sdk(SdkConfiguration sdkConfiguration)
        {
            this.sdkConfiguration = sdkConfiguration;
            this.client.DefaultRequestHeaders.Accept.Clear();
            this.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Request a new chat, routing to an agent based on the criteria specified in the provided <c>ChatSpec</c>.
        /// A <c>ChatContext</c> will be returned, this is used to reference the chat when making subsequent calls to the SDK.
        /// Live Assist will queue the chat request until a suitable agent is available, if Live Assist does not recognise
        /// the criteria specified by the <c>ChatSpec</c>, or a server error occurs, a <c>ChatException</c> will be thrown
        /// </summary>
        /// <param name="chatSpec"> Routing criteria and chat configuration</param>
        /// <returns>A <c>ChatContext</c> referencing the requested chat</returns>
        public async Task<ChatContext> RequestChat(ChatSpec chatSpec)
        {
            // TODO wrap in a try catch ?
            // TODO refactor out the posing of the chatRequest
            BaseUri baseUri = await GetBaseUriAsync();

            String uri = "https://" + baseUri.baseURI + "/api/account/" +
                         sdkConfiguration.AccountNumber + "/chat/request.json?v=1&appKey=" + sdkConfiguration.AppKey;

            ChatRequest chatRequest = new ChatRequest(chatSpec);
            var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(chatRequest));
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(uri, httpContent);

            if (response.StatusCode == HttpStatusCode.Created)
            {
                Uri sessionUri = response.Headers.Location;
                string nextEventUri = $"{sessionUri}.json?v=1&appKey={sdkConfiguration.AppKey}";
                ChatContext chatContext = new ChatContext(sessionUri.AbsoluteUri, nextEventUri);

                if (chatSpec.VisitorName != null && chatSpec.VisitorName.Length > 0)
                {
                    await SetVisitorName(chatSpec.VisitorName, chatContext);
                }

                if (chatSpec.ContextData != null)
                {
                    var info = await GetInfo(chatContext);
                    string context = chatSpec.ContextData(info.Info.rtSessionId.ToString());
                    await PostContext(context, chatContext);
                }

                return chatContext;
            }
            else
            {
                throw new ChatException($"Failed to start chat, received HTTP response code {response.StatusCode} from chat server");
            }
        }

        /// <summary>
        /// Poll ongoing chat for info and any events occuring since last call to poll, <see cref="Cafex.LiveAssist.Bot.ChatInfo"/>
        /// </summary>
        /// <param name="chatContext">Reference to specific chat to poll</param>
        /// <returns>An instance of <c>ChatInfo</c></returns>
        public async Task<ChatInfo> Poll(ChatContext chatContext)
        {
            if (chatContext != null)
            {
                LPEvent chatEvent = await PollEvents(chatContext);

                if (chatEvent != null && chatEvent.chat.link != null)
                {
                    foreach (Link link in chatEvent.chat.link)
                    {
                        if (link.rel.Equals("next"))
                        {
                            chatContext.NextEvents = RepairUri(link.href).AbsoluteUri;
                        }
                    }
                }
                else
                {
                    return null;
                }
                return chatEvent.getChatInfo();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Post a line of text to the chat
        /// </summary>
        /// <param name="text">The line of text to post</param>
        /// <param name="chatContext">Reference to the specific chat</param>
        /// <returns>True if line successfully posted</returns>
        public async Task<bool> PostLine(string text, ChatContext chatContext)
        {
            string path = $"{chatContext.SessionUrl}/events?v=1&appKey={sdkConfiguration.AppKey}";
            var line = new LPLine(text);
            var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(line));
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(path, httpContent);
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// End a chat.
        /// </summary>
        /// <param name="chatContext">Reference to the chat to end</param>
        /// <returns>True if chat successfully ended</returns>
        public async Task<bool> EndChat(ChatContext chatContext)
        {
            string path = $"{chatContext.SessionUrl}/events.json?v=1&appKey={sdkConfiguration.AppKey}";
            var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(new LPEnd()));
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(path, httpContent);
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// For a given <c>ChatSpec</c>, return <c>true</c> if agents are available for chat. <c>false</c> otherwise.
        /// </summary>
        /// <param name="chatSpec">Reference to the chat to end</param>
        /// <returns><c>true</c> if agents are available for chat. <c>false</c> otherwise</returns>
        public async Task<bool> GetAvailability(ChatSpec chatSpec)
        {
            BaseUri baseUri = await GetBaseUriAsync();

            var uri  = $"https://{baseUri.baseURI}/api/account/{sdkConfiguration.AccountNumber}/chat/availability?v=1&appKey={sdkConfiguration.AppKey}&skill={chatSpec.Skill}";

            HttpResponseMessage response = await client.GetAsync(uri);
            LPAvailability availability = null;

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                availability = JsonConvert.DeserializeObject<LPAvailability>(content);
                response.Dispose();
                return availability.Availability;
            }
            else
            {
                response.Dispose();
                throw new ChatException($"Failed to get agent availability, received HTTP response code {response.StatusCode} from chat server");
            }
        }

        private async Task<BaseUri> GetBaseUriAsync()
        {
            string account = sdkConfiguration.AccountNumber;

            if (baseUri == null)
            {
                string path = $"http://api.liveperson.net/api/account/{account}/service/conversationVep/baseURI.json?version=1.0";
                HttpResponseMessage response = await client.GetAsync(path);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    baseUri = JsonConvert.DeserializeObject<BaseUri>(content);
                }
                else
                {
                    throw new ChatException("Failed to contact Chat server: check Live Persion account ID is valid");
                }
                response.Dispose();
            }
            return baseUri;
        }

        internal async Task<LPEvent> PollEvents(ChatContext chatContext)
        {
            LPEvent chatEvent = null;

            if (client != null && chatContext.NextEvents != null)
            {
                HttpResponseMessage response = await client.GetAsync(chatContext.NextEvents);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    chatEvent = JsonConvert.DeserializeObject<LPEvent>(content);
                    response.Dispose();
                }
                else
                {
                    response.Dispose();
                    throw new ChatException($"Failed to poll events, received HTTP response code {response.StatusCode} from chat server");
                }
            }
            else
            {
                return null;
            }
            return chatEvent;
        }


        private async Task<LPInfo> GetInfo(ChatContext chatContext)
        {
            string path = $"{chatContext.SessionUrl}/info.json?v=1&appKey={sdkConfiguration.AppKey}";
            HttpResponseMessage response = await client.GetAsync(path);
            LPInfo info = null;

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                info = JsonConvert.DeserializeObject<LPInfo>(content);
                response.Dispose();
                return info;
            } else
            {
                response.Dispose();
                throw new ChatException($"Failed to get chat info, received HTTP response code {response.StatusCode} from chat server");
            }
        }

        private async Task PostContext(string context, ChatContext chatContext)
        {
            string path = $"https://{sdkConfiguration.ContextDataHost}:{sdkConfiguration.ContextDataPort}{sdkConfiguration.ContextDataPath}";

            var contextRequest = new CXContext()
            {
                AccountId = sdkConfiguration.AccountNumber,
                ContextData = context
            };

            var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(contextRequest));
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(path, httpContent);

            if (!response.IsSuccessStatusCode)
            {
                response.Dispose();
                throw new ChatException($"Failed to post contextData, received HTTP response code {response.StatusCode} from context server");
            }
            response.Dispose();
        }

        private async Task SetVisitorName(string visitorName, ChatContext chatContext)
        {
            string path = $"{chatContext.SessionUrl}/info/visitorName.json?v=1&appKey={sdkConfiguration.AppKey}";

            var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(new LPVisitorName() { VisitorName = visitorName }));
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            httpContent.Headers.Add("X-HTTP-Method-Override", "PUT");
            HttpResponseMessage response = await client.PostAsync(path, httpContent);

            if (!response.IsSuccessStatusCode)
            {
                response.Dispose();
                throw new ChatException($"Failed to set visitor name, received HTTP response code {response.StatusCode} from chat server");
            }
            response.Dispose();
        }

        // Add parameters "v" and "appKey". Append ".json" to the path.
        internal Uri RepairUri(String baseUri)
        {
            UriBuilder b = new UriBuilder(baseUri);
            b.Path = $"{b.Path}.json";
            b.Query = $"{b.Query.TrimStart('?')}&v=1&appKey={sdkConfiguration.AppKey}";
            return b.Uri;
        }
    }
}
