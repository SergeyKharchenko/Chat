using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using Chat.Filters;
using Entities;
using Entities.Core;
using Entities.Core.Concrete;
using Entities.Models;
using WebMatrix.WebData;
using System.Linq;

namespace Chat.Infrastructure.Concrete
{
    public class DbSeeder : DropCreateDatabaseAlways<ChatContext>
    {
        protected override void Seed(ChatContext context)
        {
            var initializeSimpleMembershipAttribute = new InitializeSimpleMembershipAttribute();
            initializeSimpleMembershipAttribute.OnActionExecuting(null);

            WebSecurity.CreateUserAndAccount("Sergey", "1234");
            var user = context.Users.First();

            var record1 = new Record {Text = "a", Creator = user};
            var record2 = new Record { Text = "b", Creator = user };

            var chat = new Entities.Models.Chat
                {
                    Title = "Nice chat",
                    Creator = user,
                    LastActivity = DateTime.Now,
                    Records = new Collection<Record>
                        {
                            record1,
                            record2
                        },
                    //Participants = new Collection<User> {user}
                };
            context.Chats.Add(chat);
            

            context.SaveChanges();

            context.Entry(chat).State = EntityState.Deleted;
            context.SaveChanges();
        }
    }
}