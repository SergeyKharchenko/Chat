using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc;
using Chat.Controllers;
using Chat.Infrastructure.Abstract;
using Chat.Infrastructure.Concrete;
using Chat.ViewModels;
using Entities.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Chat.Tests
{
    [TestClass]
    public class RoomTest
    {
        private Mock<IAuthorizationService> authorizationServiceMock;

        private Mock<IEntityRepository<Room>> chatRepositoryMock;
        private Mock<IEntityRepository<Record>>  recordRepositoryMock;
        private Mock<IEntityRepository<Member>>  memberRepositoryMock;

        private RoomController roomController;

        [TestInitialize]
        public void InitializeChats()
        {
            chatRepositoryMock = new Mock<IEntityRepository<Room>>();

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

            var chats = new List<Room>
                {
                    new Room
                        {
                            RoomId = 1,
                            Title = "Sergey's Room",
                            Creator = sergey,
                            CreatorionDate = DateTime.MinValue,
                            Records = new Collection<Record> {recordSergey1, recordSergey2},
                        },
                    new Room
                        {
                            RoomId = 2,
                            Title = "Igor's Room",
                            Creator = igor,
                            CreatorionDate = DateTime.Now,
                            Records = new Collection<Record> {recordIgor1},
                        },
                    new Room
                        {
                            RoomId = 3,
                            Title = "Andrey's Room",
                            Creator = andrey,
                            CreatorionDate = DateTime.Now,
                            Records = new Collection<Record> {recordAndrey2, recordMaxim1, recordAndrey1},
                        },
                    new Room
                        {
                            RoomId = 4,
                            Title = "Empty Room",
                            Creator = andrey,
                            CreatorionDate = DateTime.MinValue,
                            Records = new Collection<Record>(),
                            Members = new Collection<Member>()
                        }
                };

            chats[0].Members = new Collection<Member>
                {
                    new Member {User = sergey, Room = chats[0], EnterTime = DateTime.Now},
                    new Member {User = igor, Room = chats[0], EnterTime = DateTime.Now},
                    new Member {User = andrey, Room = chats[0], EnterTime = DateTime.Now},
                    new Member {User = maxim, Room = chats[0], EnterTime = DateTime.Now}
                };
            chats[1].Members = new Collection<Member>
                {
                    new Member {User = sergey, Room = chats[1], EnterTime = DateTime.Now},
                    new Member {User = igor, Room = chats[1], EnterTime = DateTime.Now},
                };
            chats[2].Members = new Collection<Member>
                {
                    new Member {User = sergey, Room = chats[2], EnterTime = DateTime.Now},
                    new Member {User = andrey, Room = chats[2], EnterTime = DateTime.Now},
                    new Member {User = maxim, Room = chats[2], EnterTime = DateTime.Now}
                };

            memberRepositoryMock = new Mock<IEntityRepository<Member>>();
            var memberCollection = from chat in chats
                    from member in chat.Members
                    select member;
            memberRepositoryMock.Setup(repo => repo.Entities).Returns(memberCollection.AsQueryable());
            
            chatRepositoryMock.Setup(repo => repo.Entities).Returns(chats.AsQueryable);

            chatRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>()))
                .Returns((int id) => chatRepositoryMock.Object.Entities.Single(c => c.RoomId == id));

            authorizationServiceMock = new Mock<IAuthorizationService>();
            authorizationServiceMock.Setup(service => service.GetCurrentUser()).Returns(new User());

            roomController = new RoomController(chatRepositoryMock.Object,
                                                null,
                                                memberRepositoryMock.Object,
                                                authorizationServiceMock.Object);
        }

        [TestMethod]
        public void CanShowAllChatsTest()
        {                   
            var view = roomController.List();

            Assert.IsInstanceOfType(view, typeof(ViewResult));
            Assert.IsInstanceOfType(view.Model, typeof(IQueryable<Room>));
        }

        [TestMethod]
        public void CanGetChatInfoByRightIdTest()
        {
            var chatInfo = roomController.Info(1).Model as ChatInfo;

            Assert.AreEqual(chatInfo.Title, "Sergey's Room");
            Assert.AreEqual(chatInfo.Members.Length, 4);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CannotGetChatInfoByWrongIdTest()
        {
            roomController.Info(5);
        }

        [TestMethod]
        public void GetChatLastActivityTest()
        {
            var chatInfo = roomController.Info(1).Model as ChatInfo;
            var emptyChatInfo = roomController.Info(4).Model as ChatInfo;

            Assert.AreEqual(chatInfo.LastActivity, DateTime.MaxValue);
            Assert.AreEqual(emptyChatInfo.LastActivity, DateTime.MinValue);
        }

        [TestMethod]
        public void CreateChatSuccessTest()
        {
            var chat = new Room {Title = "Test Room"};
            
            var view = roomController.Create(chat);

            chatRepositoryMock.Verify(service => service.Create(chat), Times.Once());
            Assert.IsInstanceOfType(view, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateChatUnsuccessTest()
        {           
            var chat = new Room {Title = "Test Room"};
            chatRepositoryMock.Setup(service => service.Create(chat)).Throws(new ArgumentException());
            
            var view = roomController.Create(chat);

            chatRepositoryMock.Verify(service => service.Create(chat), Times.Once());
            Assert.IsInstanceOfType(view, typeof(ViewResult));
        }

        [TestMethod]
        public void CanEnterTheRoomTest()
        {           
            var view = roomController.JoinRoom(1);

            chatRepositoryMock.Verify(service => service.GetById(1), Times.Once());
            chatRepositoryMock.Verify(service => service.GetById(It.IsAny<int>()), Times.Once());
            Assert.IsInstanceOfType(view, typeof(ViewResult));
            Assert.AreEqual((view.Model as Room).RoomId, 1);
        }
    }
}