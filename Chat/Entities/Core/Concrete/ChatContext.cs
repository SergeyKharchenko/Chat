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
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Record>()
                        .HasRequired(record => record.Room)
                        .WithMany(chat => chat.Records)
                        .HasForeignKey(record => record.RoomId);

            modelBuilder.Entity<Room>()
                        .HasRequired(chat => chat.Creator)
                        .WithMany(user => user.CreatedRooms)
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
                        .HasRequired(member => member.Room)
                        .WithMany(user => user.Members)
                        .HasForeignKey(member => member.RoomId);

            modelBuilder.Entity<Image>()
                        .HasRequired(image => image.User)
                        .WithOptional(user => user.Image)
                        .Map(configuration => configuration.MapKey("UserId"));

            //modelBuilder.Entity<User>()
            //            .HasOptional(user => user.Image)
            //            .WithRequired(image => image.User)
            //            .Map(configuration => configuration.MapKey("ImageId"));
        }
    }
}