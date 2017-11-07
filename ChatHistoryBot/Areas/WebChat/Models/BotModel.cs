using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChatHistoryBot.Areas.WebChat.Models
{
    public class BotModel
    {
        public BotModel(string userID) //TODO:take a user id
        {

            UniqueUserId = userID;

            Task.Run(async () => await SetToken()).Wait();
        }

        public string Token { get; set; }

        public string UniqueUserId { get; set; }

        //public List<Activity> History { get; set; }

        async Task SetToken()
        {
            string botChatSecret = ConfigurationManager.AppSettings["BotChatSecret"];

            var request = new HttpRequestMessage(HttpMethod.Get, "https://webchat.botframework.com/api/tokens");
            request.Headers.Add("Authorization", "BOTCONNECTOR " + botChatSecret);

            using (HttpResponseMessage response = await new HttpClient().SendAsync(request))
            {
                string token = await response.Content.ReadAsStringAsync();
                Token = token.Replace("\"", "");
            }
        }

        List<JObject> _history;

        public List<JObject> History
        {
            get
            {
                List<string> history=null;
                if (_history == null)
                {
                    
                    using (var dataContext = new Data.ConversationDataContext())
                    {
                        history = (from conversation in dataContext.UserConversations
                            where conversation.UserId == UniqueUserId
                            join activity in dataContext.Activities
                            on conversation.ConversationId equals activity.ConversationId
                            where activity.Type == ActivityTypes.Message
                            orderby activity.Timestamp ascending 
                            select activity.Json).ToList();
                    }
                }
                _history = new List<JObject>();
                foreach (var item  in history)
                {
                    Activity x = JsonConvert.DeserializeObject<Activity>(item);
                    if (x.Attachments != null || string.IsNullOrWhiteSpace(x.Text))
                    {
                        _history.Add(JObject.Parse(item));
                    }
                }
                return _history;
            }

            set => _history = value;
        }
    }
}