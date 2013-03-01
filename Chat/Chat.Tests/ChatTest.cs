using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc;
using Chat.Controllers;
using Entities.Core.Abstract;
using Entities.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Chat.Tests
{
    [TestClass]
    public class ChatTest
    {
        private Mock<IChatRepository> mock;

        [TestInitialize]
        public void InitializeChats()
        {
            mock = new Mock<IChatRepository>();

            var sergey = new User {Login = "Sergey"};
            var igor = new User {Login = "Igor"};
            var andrey = new User {Login = "Andrey"};
            var maxim = new User {Login = "Maxim"};

            var recordSergey1 = new Record {Text = "Hello", Creator = sergey};
            var recordSergey2 = new Record {Text = "world", Creator = sergey};

            var recordIgor1 = new Record {Text = "Oh, no", Creator = igor};

            var recordAndrey1 = new Record {Text = "I so lonely", Creator = andrey};
            var recordAndrey2 = new Record {Text = "Java is the best", Creator = andrey};

            var recordMaxim1 = new Record {Text = "For C#!!!", Creator = maxim};

            mock.Setup(repo => repo.Chats).Returns(new List<Entities.Models.Chat>
                {
                    new Entities.Models.Chat
                        {
                            Title = "Sergey's chat",
                            Creator = sergey,
                            LastActivity = DateTime.Now,
                            Records = new Collection<Record> {recordSergey1, recordSergey2},
                            Participants = new Collection<User> {sergey, igor, andrey, maxim}
                        },
                    new Entities.Models.Chat
                        {
                            Title = "Igor's chat",
                            Creator = igor,
                            LastActivity = DateTime.Now,
                            Records = new Collection<Record> {recordIgor1},
                            Participants = new Collection<User> {sergey, igor}
                        },
                    new Entities.Models.Chat
                        {
                            Title = "Andrey's chat",
                            Creator = andrey,
                            LastActivity = DateTime.Now,
                            Records = new Collection<Record> {recordAndrey2, recordMaxim1, recordAndrey1},
                            Participants = new Collection<User> {sergey, andrey, maxim}
                        }
                }.AsQueryable);
        }

        [TestMethod]
        public void ShowAllChatsTest()
        {                        
            var chatController = new ChatController(mock.Object);
            
            var view = chatController.Index();

            Assert.IsInstanceOfType(view, typeof(ViewResult));
            Assert.IsInstanceOfType(view.Model, typeof(IQueryable<Entities.Models.Chat>));
            Assert.AreEqual((view.Model as IEnumerable<Entities.Models.Chat>).Count(), 3);
            Assert.AreEqual((view.Model as IEnumerable<Entities.Models.Chat>).First().Title, "Sergey's chat");
        }
    }
}