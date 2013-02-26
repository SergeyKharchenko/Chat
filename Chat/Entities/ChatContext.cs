using System.Data.Entity;
using Entities.Authorization;
using Entities.Core;

namespace Entities
{
    public class ChatContext : DbContext
    {
        public ChatContext() : base("ChatContext")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Record> Records { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chat>().ToTable("Chat");
            modelBuilder.Entity<Record>().ToTable("Record");

            modelBuilder.Entity<Chat>()
                        .HasMany(chat => chat.Records)
                        .WithRequired(record => record.Chat)
                        .Map(record => record.MapKey("ChatId"));

            modelBuilder.Entity<User>()
                        .HasMany(user => user.Chats)
                        .WithRequired(chat => chat.Creator)
                        .Map(chat => chat.MapKey("CreatorId"));
        }
    }
}