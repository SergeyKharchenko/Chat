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
        private Collection<User> users;

        private Mock<IEntityRepository<Room>> roomRepositoryMock;
        private Mock<IEntityRepository<Record>> recordRepositoryMock;
        private Mock<IEntityRepository<Member>> memberRepositoryMock;

        private RoomController roomController;

        [TestInitialize]
        public void InitializeChats()
        {           
            #region Users

            users = new Collection<User>
                {
                    new User {UserId = 1, Login = "sergey", Members = new Collection<Member>()},
                    new User {UserId = 2, Login = "igor", Members = new Collection<Member>()},
                    new User {UserId = 3, Login = "andrey", Members = new Collection<Member>()},
                    new User {UserId = 4, Login = "maxim", Members = new Collection<Member>()}
                };

            #endregion

            #region Records

            var records = new Collection<Record>
                {
                    new Record {Text = "Hello", Creator = users[0], CreationDate = DateTime.MaxValue},
                    new Record {Text = "world", Creator = users[0], CreationDate = DateTime.MinValue},
                    new Record {Text = "Oh, no", Creator = users[1], CreationDate = DateTime.Now},
                    new Record {Text = "I so lonely", Creator = users[2], CreationDate = DateTime.Now},
                    new Record {Text = "Java is the best", Creator = users[2], CreationDate = DateTime.Now},
                    new Record {Text = "For C#!!!", Creator = users[3], CreationDate = DateTime.Now}
                };

            #endregion

            #region Rooms

            var rooms = new List<Room>
                {
                    new Room
                        {
                            RoomId = 1,
                            Title = "sergey's Room",
                            Creator = users[0],
                            CreatorId = users[0].UserId,
                            CreatorionDate = DateTime.MinValue,
                            Records = new Collection<Record> {records[0], records[1]},
                        },
                    new Room
                        {
                            RoomId = 2,
                            Title = "igor's Room",
                            Creator = users[1],
                            CreatorId = users[1].UserId,
                            CreatorionDate = DateTime.Now,
                            Records = new Collection<Record> {records[2]},
                        },
                    new Room
                        {
                            RoomId = 3,
                            Title = "andrey's Room",
                            Creator = users[2],
                            CreatorId = users[2].UserId,
                            CreatorionDate = DateTime.Now,
                            Records = new Collection<Record> {records[3], records[4], records[5]},
                        },
                    new Room
                        {
                            RoomId = 4,
                            Title = "Empty Room",
                            Creator = users[3],
                            CreatorId = users[3].UserId,
                            CreatorionDate = DateTime.MinValue,
                            Records = new Collection<Record>(),
                            Members = new Collection<Member>()
                        }
                };

            #endregion

            #region Members

            rooms[0].Members = new List<Member>
                {
                    new Member {User = users[0], UserId = users[0].UserId, Room = rooms[0], RoomId = rooms[0].RoomId, EnterTime = DateTime.Now},
                    new Member {User = users[1], UserId = users[1].UserId, Room = rooms[0], RoomId = rooms[0].RoomId, EnterTime = DateTime.Now},
                    new Member {User = users[2], UserId = users[2].UserId, Room = rooms[0], RoomId = rooms[0].RoomId, EnterTime = DateTime.Now},
                    new Member {User = users[3], UserId = users[3].UserId, Room = rooms[0], RoomId = rooms[0].RoomId, EnterTime = DateTime.Now}
                };                                                                           
                                                                                             
            rooms[1].Members = new List<Member>                                              
                {                                                                            
                    new Member {User = users[0], UserId = users[0].UserId, Room = rooms[1], RoomId = rooms[1].RoomId, EnterTime = DateTime.Now},
                    new Member {User = users[1], UserId = users[1].UserId, Room = rooms[1], RoomId = rooms[1].RoomId, EnterTime = DateTime.Now},
                };                                                                           
                                                                                             
            rooms[2].Members = new List<Member>                                              
                {                                                                            
                    new Member {User = users[0], UserId = users[0].UserId, Room = rooms[2], RoomId = rooms[2].RoomId, EnterTime = DateTime.Now},
                    new Member {User = users[2], UserId = users[2].UserId, Room = rooms[2], RoomId = rooms[2].RoomId, EnterTime = DateTime.Now},
                    new Member {User = users[3], UserId = users[3].UserId, Room = rooms[2], RoomId = rooms[2].RoomId, EnterTime = DateTime.Now}
                };

            var members = new Collection<Member>();
            rooms.ForEach(room =>
                {
                    foreach (var member in room.Members)
                    {
                        member.User.Members.Add(member);
                        members.Add(member);
                    }
                });

            #endregion

            roomRepositoryMock = new Mock<IEntityRepository<Room>>();
            roomRepositoryMock.Setup(repo => repo.Entities).Returns(rooms.AsQueryable());

            recordRepositoryMock = new Mock<IEntityRepository<Record>>();
            recordRepositoryMock.Setup(repo => repo.Entities).Returns(records.AsQueryable());

            memberRepositoryMock = new Mock<IEntityRepository<Member>>();
            memberRepositoryMock.Setup(repo => repo.Entities).Returns(members.AsQueryable());

            authorizationServiceMock = new Mock<IAuthorizationService>();

            roomController = new RoomController(roomRepositoryMock.Object,
                                                recordRepositoryMock.Object,
                                                memberRepositoryMock.Object,
                                                authorizationServiceMock.Object);
        }

        [TestMethod]
        public void CanShowAllRoomsTest()
        {                   
            var view = roomController.List();

            Assert.IsInstanceOfType(view, typeof(ViewResult));
            Assert.IsInstanceOfType(view.Model, typeof(IQueryable<Room>));
        }

        [TestMethod]
        public void CanGetRoomInfoByRightIdTest()
        {
            roomRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>()))
                              .Returns((int id) => roomRepositoryMock.Object.Entities
                                                                     .First(room => room.RoomId == id));
            var roomInfo = roomController.Info(1).Model as RoomInfo;

            Assert.AreEqual(roomInfo.Title, "sergey's Room");
            Assert.AreEqual(roomInfo.Members.Length, 4);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CannotGetRoomInfoByWrongIdTest()
        {
            roomRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>()))
                  .Returns((int id) => roomRepositoryMock.Object.Entities
                                                         .First(room => room.RoomId == id));

            roomController.Info(5);
        }

        [TestMethod]
        public void GetRoomLastActivityTest()
        {
            roomRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>()))
                              .Returns((int id) => roomRepositoryMock.Object.Entities
                                                                     .First(room => room.RoomId == id));

            var roomInfo = roomController.Info(1).Model as RoomInfo;
            var emptyChatInfo = roomController.Info(4).Model as RoomInfo;

            Assert.AreEqual(roomInfo.LastActivity, roomInfo.Records.Max(record => record.CreationDate));
            Assert.AreEqual(emptyChatInfo.LastActivity, emptyChatInfo.CreationDate);
        }

        [TestMethod]
        public void CreateRoomTest()
        {
            roomRepositoryMock.Setup(service => service.Create(It.IsAny<Room>()));

            var chat = new Room { RoomId = 42, Title = "Test Room" };
            
            var view = roomController.Create(chat);

            roomRepositoryMock.Verify(service => service.Create(chat), Times.Once());
            Assert.IsInstanceOfType(view, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void CanEnterTheRoomWihtoutAddMemberTest()
        {
            authorizationServiceMock.Setup(service => service.GetCurrentUser())
                                    .Returns(users[0]);
            roomRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>()))
                  .Returns((int id) => roomRepositoryMock.Object.Entities
                                                         .First(room => room.RoomId == id));

            var view = roomController.JoinRoom(2);

            roomRepositoryMock.Verify(repo => repo.GetById(2), Times.Once());
            memberRepositoryMock.Verify(repo => repo
                .Create(It.Is((Member member) => member.UserId == users[3].UserId)), Times.Never());
            Assert.IsInstanceOfType(view, typeof(ViewResult));
            Assert.AreEqual((view.Model as Room).RoomId, 2);
        }

        [TestMethod]
        public void CanEnterTheRoomWihtAddMemberTest()
        {
            authorizationServiceMock.Setup(service => service.GetCurrentUser())
                                    .Returns(users[3]);
            roomRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>()))
                  .Returns((int id) => roomRepositoryMock.Object.Entities
                                                         .First(room => room.RoomId == id));

            var view = roomController.JoinRoom(2);

            roomRepositoryMock.Verify(repo => repo.GetById(2), Times.Once());
            memberRepositoryMock.Verify(repo => repo
                .Create(It.Is((Member member) => member.UserId == users[3].UserId)), Times.Once());
            Assert.IsInstanceOfType(view, typeof(ViewResult));
            Assert.AreEqual((view.Model as Room).RoomId, 2);
        }
    }
}