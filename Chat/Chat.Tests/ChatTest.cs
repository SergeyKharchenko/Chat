using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc;
using Chat.Controllers;
using Chat.ViewModels;
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

            var recordSergey1 = new Record { Text = "Hello", Creator = sergey, CreationDate = DateTime.MaxValue };
            var recordSergey2 = new Record { Text = "world", Creator = sergey, CreationDate = DateTime.MinValue };

            var recordIgor1 = new Record { Text = "Oh, no", Creator = igor, CreationDate = DateTime.Now };

            var recordAndrey1 = new Record { Text = "I so lonely", Creator = andrey, CreationDate = DateTime.Now };
            var recordAndrey2 = new Record { Text = "Java is the best", Creator = andrey, CreationDate = DateTime.Now };

            var recordMaxim1 = new Record { Text = "For C#!!!", Creator = maxim, CreationDate = DateTime.Now };

            mock.Setup(repo => repo.Chats).Returns(new List<Entities.Models.Chat>
                {
                    new Entities.Models.Chat
                        {
                            ChatId = 1,
                            Title = "Sergey's chat",
                            Creator = sergey,
                            CreatorionDate = DateTime.MinValue,
                            Records = new Collection<Record> {recordSergey1, recordSergey2},
                            Members = new Collection<User> {sergey, igor, andrey, maxim}
                        },
                    new Entities.Models.Chat
                        {
                            ChatId = 2,
                            Title = "Igor's chat",
                            Creator = igor,
                            CreatorionDate = DateTime.Now,
                            Records = new Collection<Record> {recordIgor1},
                            Members = new Collection<User> {sergey, igor}
                        },
                    new Entities.Models.Chat
                        {
                            ChatId = 3,
                            Title = "Andrey's chat",
                            Creator = andrey,
                            CreatorionDate = DateTime.Now,
                            Records = new Collection<Record> {recordAndrey2, recordMaxim1, recordAndrey1},
                            Members = new Collection<User> {sergey, andrey, maxim}
                        },
                    new Entities.Models.Chat
                        {
                            ChatId = 4,
                            Title = "Empty chat",
                            Creator = andrey,
                            CreatorionDate = DateTime.MinValue,
                            Records = new Collection<Record>(),
                            Members = new Collection<User>()
                        }
                }.AsQueryable);

            mock.Setup(repo => repo.GetChatById(It.IsAny<int>()))
                .Returns((int id) => mock.Object.Chats.Single(c => c.ChatId == id));
        }

        [TestMethod]
        public void CanShowAllChatsTest()
        {                        
            var chatController = new ChatController(mock.Object);
            
            var view = chatController.List();

            Assert.IsInstanceOfType(view, typeof(ViewResult));
            Assert.IsInstanceOfType(view.Model, typeof(IQueryable<Entities.Models.Chat>));
        }

        [TestMethod]
        public void CanGetChatInfoByRightIdTest()
        {
            var chatController = new ChatController(mock.Object);

            var chatInfo = chatController.Info(1).Model as ChatInfo;

            Assert.AreEqual(chatInfo.Title, "Sergey's chat");
            Assert.AreEqual(chatInfo.Members.Length, 4);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CannotGetChatInfoByWrongIdTest()
        {
            var chatController = new ChatController(mock.Object);

            chatController.Info(5);
        }

        [TestMethod]
        public void GetChatLastActivityTest()
        {
            var chatController = new ChatController(mock.Object);

            var chatInfo = chatController.Info(1).Model as ChatInfo;
            var emptyChatInfo = chatController.Info(4).Model as ChatInfo;

            Assert.AreEqual(chatInfo.LastActivity, DateTime.MaxValue);
            Assert.AreEqual(emptyChatInfo.LastActivity, DateTime.MinValue);
        }
    }
}