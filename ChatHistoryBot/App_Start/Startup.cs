using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(ChatHistoryBot.Startup))]
namespace ChatHistoryBot
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}