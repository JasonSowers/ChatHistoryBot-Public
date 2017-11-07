using System.Web.Mvc;

namespace ChatHistoryBot.Areas.WebChat
{
    public class WebChatAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "WebChat";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "WebChat_default",
                "WebChat/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}