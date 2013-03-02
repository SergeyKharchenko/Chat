using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using Chat.Filters;
using Entities.Core.Concrete;
using Entities.Models;
using WebMatrix.WebData;
using System.Linq;

namespace Chat.Infrastructure.Concrete
{
    public class DbSeeder : DropCreateDatabaseIfModelChanges<ChatContext>
    {
        protected override void Seed(ChatContext context)
        {
            var initializeSimpleMembershipAttribute = new InitializeSimpleMembershipAttribute();
            initializeSimpleMembershipAttribute.OnActionExecuting(null);

            WebSecurity.CreateUserAndAccount("Sergey", "1234");
            WebSecurity.CreateUserAndAccount("Igor", "1234");
            WebSecurity.CreateUserAndAccount("Andrey", "1234");
            WebSecurity.CreateUserAndAccount("Maxim", "1234");

            var sergey = context.Users.First(user => user.Login == "Sergey");
            var igor = context.Users.First(user => user.Login == "Igor");
            var andrey = context.Users.First(user => user.Login == "Andrey");
            var maxim = context.Users.First(user => user.Login == "Maxim");

            var recordSergey1 = new Record { Text = "Hello", Creator = sergey };
            var recordSergey2 = new Record { Text = "world", Creator = sergey };

            var recordIgor1 = new Record { Text = "Oh, no", Creator = igor };

            var recordAndrey1 = new Record { Text = "I so lonely", Creator = andrey };
            var recordAndrey2 = new Record { Text = "Java is the best", Creator = andrey };

            var recordMaxim1 = new Record { Text = "For C#!!!", Creator = maxim };

            var chats = new List<Entities.Models.Chat>
                {
                    new Entities.Models.Chat
                        {
                            Title = "Sergey's chat",
                            Creator = sergey,
                            LastActivity = DateTime.Now,
                            Records = new Collection<Record> {recordSergey1, recordSergey2},
                            Members = new Collection<User> {sergey, igor, andrey, maxim}
                        },
                    new Entities.Models.Chat
                        {
                            Title = "Igor's chat",
                            Creator = igor,
                            LastActivity = DateTime.Now,
                            Records = new Collection<Record> {recordIgor1},
                            Members = new Collection<User> {sergey, igor}
                        },
                    new Entities.Models.Chat
                        {
                            Title = "Andrey's chat",
                            Creator = andrey,
                            LastActivity = DateTime.Now,
                            Records = new Collection<Record> {recordAndrey2, recordMaxim1, recordAndrey1},
                            Members = new Collection<User> {sergey, andrey, maxim}
                        }
                };
            
            chats.ForEach(chat => context.Chats.Add(chat));
            context.SaveChanges();
            /*
            foreach (var record in context.Records.Where(record => record.CreatorId == sergey.UserId))
            {
                context.Records.Remove(record);
            }
            foreach (var c in context.Chats.Where(c => chat.CreatorId == sergey.UserId))
            {
                context.Chats.Remove(c);
            }
            context.Users.Remove(sergey);

            context.SaveChanges();
             */
        }
    }
}