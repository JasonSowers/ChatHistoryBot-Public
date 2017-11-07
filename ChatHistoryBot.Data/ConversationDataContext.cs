using ChatHistoryBot.Data.SqlStateService;
using System.Data.Entity;

namespace ChatHistoryBot.Data
{
    public class ConversationDataContext : DbContext
    {
        public ConversationDataContext(string connectionStringName)
            : base(connectionStringName)
        {}

        public ConversationDataContext()
            : base("ConversationDataContextConnectionString")
        {}

        public DbSet<Activity> Activities { get; set; }
        public DbSet<UserConversation> UserConversations { get; set; }
        public DbSet<UserKey> UserKeys { get; set; }
        public DbSet<SqlBotDataEntity> BotData { get; set; }
    }
}