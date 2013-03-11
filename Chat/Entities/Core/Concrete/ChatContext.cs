using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Entities.Models;

namespace Entities.Core.Concrete
{
    public class ChatContext : DbContext
    {

        public ChatContext()
            : base("ChatContext")
        {
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Record> Records { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Record>()
                        .HasRequired(record => record.Chat)
                        .WithMany(chat => chat.Records)
                        .HasForeignKey(record => record.ChatId);

            modelBuilder.Entity<Chat>()
                        .HasRequired(chat => chat.Creator)
                        .WithMany(user => user.CreatedChats)
                        .HasForeignKey(chat => chat.CreatorId);

            modelBuilder.Entity<Record>()
                        .HasRequired(chat => chat.Creator)
                        .WithMany(user => user.Records)
                        .HasForeignKey(chat => chat.CreatorId);

            modelBuilder.Entity<Member>()
                        .HasRequired(member => member.User)
                        .WithMany(user => user.Members)
                        .HasForeignKey(member => member.UserId);

            modelBuilder.Entity<Member>()
                        .HasRequired(member => member.Chat)
                        .WithMany(user => user.Members)
                        .HasForeignKey(member => member.ChatId);
        }
    }
}