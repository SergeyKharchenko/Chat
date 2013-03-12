using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc;
using Chat.Controllers;
using Chat.Infrastructure.Abstract;
using Chat.ViewModels;
using Entities.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Chat.Tests
{
    [TestClass]
    public class ChatTest
    {
        private Mock<IEntityRepository<Entities.Models.Chat>> chatRepositoryMock;
        private Mock<IAuthorizationService> authorizationServiceMock;
        private ChatController chatController;

        [TestInitialize]
        public void InitializeChats()
        {
            chatRepositoryMock = new Mock<IEntityRepository<Entities.Models.Chat>>();

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

            var chats = new List<Entities.Models.Chat>
                {
                    new Entities.Models.Chat
                        {
                            ChatId = 1,
                            Title = "Sergey's chat",
                            Creator = sergey,
                            CreatorionDate = DateTime.MinValue,
                            Records = new Collection<Record> {recordSergey1, recordSergey2},
                        },
                    new Entities.Models.Chat
                        {
                            ChatId = 2,
                            Title = "Igor's chat",
                            Creator = igor,
                            CreatorionDate = DateTime.Now,
                            Records = new Collection<Record> {recordIgor1},
                        },
                    new Entities.Models.Chat
                        {
                            ChatId = 3,
                            Title = "Andrey's chat",
                            Creator = andrey,
                            CreatorionDate = DateTime.Now,
                            Records = new Collection<Record> {recordAndrey2, recordMaxim1, recordAndrey1},
                        },
                    new Entities.Models.Chat
                        {
                            ChatId = 4,
                            Title = "Empty chat",
                            Creator = andrey,
                            CreatorionDate = DateTime.MinValue,
                            Records = new Collection<Record>(),
                            Members = new Collection<Member>()
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

            chatRepositoryMock.Setup(repo => repo.Entities).Returns(chats.AsQueryable);

            chatRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>()))
                .Returns((int id) => chatRepositoryMock.Object.Entities.Single(c => c.ChatId == id));

            authorizationServiceMock = new Mock<IAuthorizationService>();
            authorizationServiceMock.Setup(service => service.GetCurrentUser()).Returns(new User());
            chatController = new ChatController(chatRepositoryMock.Object, null, null, authorizationServiceMock.Object);
        }

        [TestMethod]
        public void CanShowAllChatsTest()
        {                   
            var view = chatController.List();

            Assert.IsInstanceOfType(view, typeof(ViewResult));
            Assert.IsInstanceOfType(view.Model, typeof(IQueryable<Entities.Models.Chat>));
        }

        [TestMethod]
        public void CanGetChatInfoByRightIdTest()
        {
            var chatInfo = chatController.Info(1).Model as ChatInfo;

            Assert.AreEqual(chatInfo.Title, "Sergey's chat");
            Assert.AreEqual(chatInfo.Members.Length, 4);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CannotGetChatInfoByWrongIdTest()
        {
            chatController.Info(5);
        }

        [TestMethod]
        public void GetChatLastActivityTest()
        {
            var chatInfo = chatController.Info(1).Model as ChatInfo;
            var emptyChatInfo = chatController.Info(4).Model as ChatInfo;

            Assert.AreEqual(chatInfo.LastActivity, DateTime.MaxValue);
            Assert.AreEqual(emptyChatInfo.LastActivity, DateTime.MinValue);
        }

        [TestMethod]
        public void CreateChatSuccessTest()
        {
            var chat = new Entities.Models.Chat {Title = "Test chat"};
            
            var view = chatController.Create(chat);

            chatRepositoryMock.Verify(service => service.Create(chat), Times.Once());
            Assert.IsInstanceOfType(view, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateChatUnsuccessTest()
        {           
            var chat = new Entities.Models.Chat {Title = "Test chat"};
            chatRepositoryMock.Setup(service => service.Create(chat)).Throws(new ArgumentException());
            
            var view = chatController.Create(chat);

            chatRepositoryMock.Verify(service => service.Create(chat), Times.Once());
            Assert.IsInstanceOfType(view, typeof(ViewResult));
        }

        [TestMethod]
        public void CanEnterTheRoomTest()
        {           
            var view = chatController.JoinRoom(1);

            chatRepositoryMock.Verify(service => service.GetById(1), Times.Once());
            chatRepositoryMock.Verify(service => service.GetById(It.IsAny<int>()), Times.Once());
            Assert.IsInstanceOfType(view, typeof(ViewResult));
            Assert.AreEqual((view.Model as Entities.Models.Chat).ChatId, 1);
        }
    }
}