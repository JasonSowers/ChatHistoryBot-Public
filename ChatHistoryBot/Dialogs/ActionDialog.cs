using ChatHistoryBot.Data;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Activity = Microsoft.Bot.Connector.Activity;
using AdaptiveCards;

namespace ChatHistoryBot.Dialogs
{
    [Serializable]
    public class ActionDialog : IDialog<string>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            var activity = await item as Activity;
            var userId = context.Activity.From.Id;

            using (ConversationDataContext dataContext = new ConversationDataContext())
            {
                if (!dataContext.UserConversations.Any(
                    c => c.UserId == userId && c.ConversationId == context.Activity.Conversation.Id))
                {
                    dataContext.UserConversations.Add(new UserConversation
                    {
                        ID = Guid.NewGuid().ToString(),
                        ConversationId = context.Activity.Conversation.Id,
                        UserId = userId
                    });
                    try
                    {
                        dataContext.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        //TODO: Add error logging
                        Console.WriteLine(e);
                        throw;
                    }
                }
            }

            var reply = activity.CreateReply();

            if (activity.Text == null)
            {
                var value = activity.Value as Newtonsoft.Json.Linq.JObject;
                if (value != null)
                {
                    var cardValue = value.ToObject<CardValue>();

                    var activityValue = activity.Value as Newtonsoft.Json.Linq.JObject;
                    if (activityValue != null)
                    {
                        var categorySelection = activityValue.ToObject<CategorySelection>();
                        var category = categorySelection.Category;
                        //query the database for products of this category type, create another adaptive card displaying the products and send to user


                        reply.Text = "Category=" + category;
                        await context.PostAsync(reply);
                    }
                    return;
                }
            }

            else if (activity.Text.ToLowerInvariant().Contains("hero"))
            {
                ReplyHeroCard(activity, reply);
            }
            else if (activity.Text.ToLowerInvariant().Contains("adaptive"))
            {
                ReplyAdaptiveCard(context, activity, reply);
            }
            else
            {
                int length = (activity.Text ?? string.Empty).Length;
                reply.Text = $"You sent {activity.Text} which was {length} characters";
            }




            await context.PostAsync(reply);

            context.Wait(MessageReceivedAsync);
        }

        private static void ReplyAdaptiveCard(IDialogContext context, Activity activity, Activity reply)
        {
            var card = new AdaptiveCard();

            var choices = new List<Choice>();
            choices.Add(new Choice()
            {
                Title = "Category 1",
                Value = "c1"
            });
            choices.Add(new Choice()
            {
                Title = "Category 2",
                Value = "c2"
            });
            var choiceSet = new ChoiceSet()
            {
                IsMultiSelect = false,
                Choices = choices,
                Style = ChoiceInputStyle.Compact,
                Id = "Category"
            };
            card.Body.Add(choiceSet);
            //object o = "wewqeriwq[poefljk"
            card.Actions.Add(new SubmitAction()
            {
                Title = "Select Category",
                Data = Newtonsoft.Json.Linq.JObject.FromObject(new {button = "select"})

            });

            reply.Attachments.Add(new Attachment()
            {
                Content = card,
                ContentType = AdaptiveCard.ContentType,
                Name = $"Card"
            });
        }
        //            await context.PostAsync(reply);
            //            context.Wait(MessageReceivedAsync);
        

        private static void ReplyHeroCard(Activity activity, Activity reply)
        {
            int length = (activity.Text ?? string.Empty).Length;
            HeroCard plCard = new HeroCard()
            {
                Title = $"Hi, I'm Botty McBotFace",
                //Subtitle = $" test \n\n Page \n\n test",
                Text = $"You sent {activity.Text} which was {length} characters"
            };
            List<Attachment> list = new List<Attachment>();

            list.Add(plCard.ToAttachment());
            reply.Attachments = list;
        }
    }

    class CardValue
    {
        public string card { get; set; }
    }

    class ButtonValue
    {
        public string button { get; set; }
    }

    class CategorySelection
    {
        public string Category { get; set; }
    }
}