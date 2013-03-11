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

            var recordSergey1 = new Record { Text = "Hello", Creator = sergey, CreationDate = DateTime.Now};
            var recordSergey2 = new Record { Text = "world", Creator = sergey, CreationDate = DateTime.Now };

            var recordIgor1 = new Record { Text = "Oh, no", Creator = igor, CreationDate = DateTime.Now };

            var recordAndrey1 = new Record { Text = "I so lonely", Creator = andrey, CreationDate = DateTime.Now };
            var recordAndrey2 = new Record { Text = "Java is the best", Creator = andrey, CreationDate = DateTime.Now };

            var recordMaxim1 = new Record { Text = "For C#!!!", Creator = maxim, CreationDate = DateTime.Now };

            var chats = new List<Entities.Models.Chat>
                {
                    new Entities.Models.Chat
                        {
                            Title = "Sergey's chat",
                            Creator = sergey,
                            CreatorionDate = DateTime.Now,
                            Records = new Collection<Record> {recordSergey1, recordSergey2},
                        },
                    new Entities.Models.Chat
                        {
                            Title = "Igor's chat",
                            Creator = igor,
                            CreatorionDate = DateTime.Now,
                            Records = new Collection<Record> {recordIgor1},
                        },
                    new Entities.Models.Chat
                        {
                            Title = "Andrey's chat",
                            Creator = andrey,
                            CreatorionDate = DateTime.Now,
                            Records = new Collection<Record> {recordAndrey2, recordMaxim1, recordAndrey1},
                        }
                };

            chats[0].Members = new Collection<Member>
                {
                    new Member {User = sergey, Chat = chats[0], EnterTime = DateTime.Now},
                    new Member {User = igor, Chat = chats[0], EnterTime = DateTime.Now},
                    new Member {User = andrey, Chat = chats[0], EnterTime = DateTime.Now},
                    new Member {User = maxim, Chat = chats[0], EnterTime = DateTime.Now}
                };
            chats[1].Members = new Collection<Member>
                {
                    new Member {User = sergey, Chat = chats[1], EnterTime = DateTime.Now},
                    new Member {User = igor, Chat = chats[1], EnterTime = DateTime.Now},
                };
            chats[2].Members = new Collection<Member>
                {
                    new Member {User = sergey, Chat = chats[2], EnterTime = DateTime.Now},
                    new Member {User = andrey, Chat = chats[2], EnterTime = DateTime.Now},
                    new Member {User = maxim, Chat = chats[2], EnterTime = DateTime.Now}
                };

            chats.ForEach(chat => context.Chats.Add(chat));
            context.SaveChanges();
        }
    }
}