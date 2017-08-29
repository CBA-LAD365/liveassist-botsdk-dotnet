using System;
using System.Threading.Tasks;
using System.Timers;
using System.Collections.Generic;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.ConnectorEx;
using Newtonsoft.Json;

using Cafex.LiveAssist.Bot;
using EscalationBot.Context;


namespace EscalationBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private static Sdk sdk;

        // This simple sample handles one chatContext, i.e one concurrent chat.
        // A production solution would persist multiple chatContexts and transcripts.
        private static ChatContext chatContext;

        private static List<TranscriptLine> transcript;
        private static Timer timer = null;
        private static string conversationRef;


        public Task StartAsync(IDialogContext context)
        {
            sdk = sdk ?? new Sdk(new SdkConfiguration() {
                AccountNumber = "__CHANGE_ME__",  // Live assist account number.
                ContextDataHost = "__CHANGE_ME__" // Host name of the context data service.
            });

            transcript = new List<TranscriptLine>();
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
          
            if (chatContext != null)
            {
                // We already have an escalated chat, post the incoming message line to this.
                await sdk.PostLine(activity.Text, chatContext);
            }
            else if (activity.Text.Contains("help"))
            {
                transcript.Add(new TranscriptLine()
                {
                    IsBot = false,
                    Timestamp = DateTime.Now,
                    SrcName = activity.From.Name,
                    Line = activity.Text
                });

                // Escalate to Live Assist agent
                await context.PostAsync("Unfortunatly BOT is unable to help, transferring to an agent");
                await Escalate(activity);
            }
            else
            {
                // Just Echo
                var message = $"You sent {activity.Text} which was {(activity.Text ?? string.Empty).Length} characters";

                transcript.Add(new TranscriptLine()
                {
                    IsBot = false,
                    Timestamp = DateTime.Now,
                    SrcName = activity.From.Name,
                    Line = activity.Text
                });

                transcript.Add(new TranscriptLine()
                {
                    IsBot = true,
                    Timestamp = DateTime.Now,
                    SrcName = "EscalationBot",
                    Line = message
                });

                await context.PostAsync(message);
            }

            context.Wait(MessageReceivedAsync);
        }

        private async Task Escalate(Activity activity)
        {
            conversationRef = JsonConvert.SerializeObject(activity.ToConversationReference());
            var chatSpec = new ChatSpec()
            {
                Skill = "__CHANGE_ME__", // Agent skill to target
                Transcript = transcript, 
                VisitorName = activity.From.Name,
                ContextData = CreateJwt
            };
            chatContext = await sdk.RequestChat(chatSpec);
            StartTimer();
        }


        // Called by the SDK to obtain a signed JWT, for a given contextId,
        // encapsulating context data to pass to Live Assist on chat creation
        public static String CreateJwt(string contextId)
        {
            var contextData = new ContextData()
            {
                customer = new Customer()
                {
                    firstName = new AssertedString()
                    {
                        value = "Sid"
                    },
                    lastName = new AssertedString()
                    {
                        value = "James",
                        isAsserted = true
                    },
                    companySize = new AssertedInteger()
                    {
                        value = 10
                    }
                }
            };

            var jwt = Jwt.Create(contextId, contextData);
            return jwt;
        }

        private void StartTimer()
        {
            if (timer == null)
            {
                timer = timer ?? new Timer(5000);
                timer.Elapsed += (sender, e) => OnTimedEvent(sender, e);
                timer.Start();
            }
        }

        async void OnTimedEvent(Object source, ElapsedEventArgs eea)
        {
            if (chatContext != null)
            {
                var reply = JsonConvert.DeserializeObject<ConversationReference>(conversationRef).GetPostToBotMessage().CreateReply();
                var client = new ConnectorClient(new Uri(reply.ServiceUrl));
                var chatInfo = await sdk.Poll(chatContext);

                if (chatInfo != null)
                {
                    if (chatInfo.ChatEvents != null && chatInfo.ChatEvents.Count > 0)
                    {
                        foreach (ChatEvent e in chatInfo.ChatEvents)
                        {
                            switch (e.Type)
                            {
                                // type is either "state" or "line".
                                case "line":
                                    // Source is either: "system", "agent" or "visitor"
                                    if (e.Source.Equals("system"))
                                    {
                                        reply.From.Name = "system";
                                    }
                                    else if (e.Source.Equals("agent"))
                                    {
                                        reply.From.Name = chatInfo.AgentName;

                                    }
                                    else
                                    {
                                        break;
                                    }

                                    reply.Type = "message";
                                    reply.Text = e.Text;
                                    client.Conversations.ReplyToActivity(reply);
                                    break;

                                case "state":
                                    // State changes
                                    // Valid values: "waiting", "chatting", "ended"
                                    break;
                            }
                        }
                    }

                    if (chatInfo.AgentTyping.Equals("typing"))
                    {
                        reply.Text = null;
                        reply.Type = "typing";
                        client.Conversations.ReplyToActivity(reply);
                    }

                    // Valid values: "waiting", "chatting", "ended"
                    if (chatInfo.State.Equals("ended"))
                    {
                        chatContext = null;
                    }
                }
            }
        }
    }
}