using Autofac;
using ChatHistoryBot.Data;
using ChatHistoryBot.Data.SqlStateService;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ChatHistoryBot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
           
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Microsoft.Bot.Connector.IMessageActivity, Data.Activity>()
                    .ForMember(dest => dest.FromId, opt => opt.MapFrom(src => src.From.Id))
                    .ForMember(dest => dest.RecipientId, opt => opt.MapFrom(src => src.Recipient.Id))
                    .ForMember(dest => dest.FromName, opt => opt.MapFrom(src => src.From.Name))
                    .ForMember(dest => dest.RecipientName, opt => opt.MapFrom(src => src.Recipient.Name));
            });
            var builder = new ContainerBuilder();
            builder.RegisterType<EntityFrameworkActivityLogger>().AsImplementedInterfaces().InstancePerDependency();

            var store = new SqlBotDataStore("ConversationDataContextConnectionString");

            builder.Register(c => new CachingBotDataStore(store, CachingBotDataStoreConsistencyPolicy.LastWriteWins))
                .As<IBotDataStore<BotData>>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.Update(Conversation.Container);
            
        }
    }
}
