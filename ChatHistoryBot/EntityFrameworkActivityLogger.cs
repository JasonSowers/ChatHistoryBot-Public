using AutoMapper;
using Microsoft.Bot.Builder.History;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace ChatHistoryBot
{
    public class EntityFrameworkActivityLogger : IActivityLogger
    {

#pragma warning disable 1998
        public async Task LogAsync(IActivity activity)
        {
            IMessageActivity msg = activity.AsMessageActivity();
            if (msg != null)
            {
                using (Data.ConversationDataContext dataContext = new Data.ConversationDataContext())
                {
                    var newActivity = Mapper.Map<IMessageActivity, Data.Activity>(msg);
                    if (string.IsNullOrEmpty(newActivity.Id))
                    {
                        newActivity.Id = Guid.NewGuid().ToString();
                    }
                    newActivity.Json = JsonConvert.SerializeObject(activity);
                    dataContext.Activities.Add(newActivity);
                    dataContext.SaveChanges();
                }
            }
        }
    }
}