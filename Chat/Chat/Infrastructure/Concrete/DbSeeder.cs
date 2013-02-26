using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using Entities;
using Entities.Authorization;
using Entities.Core;
using WebMatrix.WebData;

namespace Chat.Infrastructure.Concrete
{
    public class DbSeeder : DropCreateDatabaseIfModelChanges<ChatContext>
    {
        protected override void Seed(ChatContext context)
        {       
            if (WebSecurity.Initialized)
            {
                Database.SetInitializer<ChatContext>(null);
                WebSecurity.InitializeDatabaseConnection("ChatContext", "User", "UserId", "Login",
                                                         autoCreateTables: true);
            }

            Database.SetInitializer(new DbSeeder());

            var user = new User {Login = "Sergey"};
            context.Users.Add(user);
            context.SaveChanges();

            var chat = new Entities.Core.Chat
                {
                    Creator = user,
                    Title = "Nice chat",
                    LastActivity = DateTime.Now,
                    Users = new Collection<User>
                        {
                            new User {Login = "John"},
                            new User {Login = "Bob"}
                        },
                    Records = new Collection<Record>
                        {
                            new Record {Text = "Hello"}
                        }
                };
            context.Chats.Add(chat);
        }
    }
}