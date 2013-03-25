using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.Mvc;
using Chat.Controllers;
using Chat.Infrastructure.Abstract;
using Chat.Models;
using Chat.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;

namespace Chat.Tests.Tests.Controller
{
    [TestClass]
    public class RoomControllerTests
    {
        private Mock<IRoomUnitOfWork> mock;
        private RoomController controller;

        [TestInitialize]
        public void InitializeChats()
        {
            mock = new Mock<IRoomUnitOfWork>();
            controller = new RoomController(mock.Object);
        }

        [TestMethod]
        public void ListTest()
        {
            return;
            mock.Setup(unit => unit.Rooms).Returns(new Collection<Room>
                {
                    new Room
                        {
                            Creator = new User(),
                            Title = "Amazing Room",
                            Members = new Collection<Member>(),
                            Records = new Collection<Record>()
                        },
                    new Room
                        {
                            Creator = new User(),
                            Title = "Good Room",
                            Members = new Collection<Member>(),
                            Records = new Collection<Record>()
                        }
                });
            mock.Setup(unit => unit.GetCurrentUserId())
                .Returns(2);

            var view = controller.List();
            var rooms = view.Model as IEnumerable<RoomInfo>;

            Assert.IsNotNull(rooms);
            mock.Verify(unit => unit.GetCurrentUserId(), Times.Once());
            mock.Verify(unit => unit.Rooms, Times.Once());
            Assert.AreEqual(2, rooms.Count());
            Assert.AreEqual("Good Room", rooms.Last().Title);
        }

        [TestMethod]
        public void InfoTest()
        {
            mock.Setup(unit => unit.FindRoomById(42))
                .Returns(new Room
                    {
                        Id = 42,
                        Creator = new User(),
                        CreatorId = 2,
                        Records = new Collection<Record>(),
                        Members = new Collection<Member>()
                    });
            mock.Setup(unit => unit.GetCurrentUserId())
                .Returns(2);

            var view = controller.Info(42);
            var roomInfo = view.Model as RoomInfo;

            Assert.IsNotNull(roomInfo);
            mock.Verify(unit => unit.FindRoomById(42), Times.Once());
            mock.Verify(unit => unit.GetCurrentUserId(), Times.Once());
            Assert.AreEqual(42, roomInfo.Id);
        }

        [TestMethod]
        public void CreateWithoutTitleTest()
        {
            var view = controller.Create(new Room());

            Assert.IsInstanceOfType(view, typeof(ViewResult));
        }

        [TestMethod]
        public void CreateTest()
        {
            mock.Setup(unit => unit.CreateRoom(It.IsAny<Room>()));
            mock.Setup(unit => unit.Commit());

            var view = controller.Create(new Room{Title = "Amazing Room"});

            mock.Verify(unit => unit.CreateRoom(It.Is((Room room) => room.Title == "Amazing Room")), Times.Once());
            mock.Verify(unit => unit.Commit(), Times.Once());
            Assert.IsInstanceOfType(view, typeof(RedirectToRouteResult));
            Assert.AreEqual("List", (view as RedirectToRouteResult).RouteValues["action"]);
        }

        [TestMethod]
        public void JoinTest()
        {
            mock.Setup(unit => unit.JoinRoom(It.IsAny<int>()))
                .Returns((int id) => new Room
                    {
                        Id = id,
                        Members = new Collection<Member>
                            {
                                new Member {UserId = 1, User = new User {Login = "John"}},
                                new Member {UserId = 2, User = new User {Login = "Tom"}},
                                new Member {UserId = 3, User = new User {Login = "Eric"}}
                            }
                    });
            mock.Setup(unit => unit.Commit());
            mock.Setup(unit => unit.GetCurrentUserId()).Returns(2);

            var view = controller.Join(42);
            var room = view.Model as Room;

            Assert.IsNotNull(room);
            mock.Verify(unit => unit.JoinRoom(42), Times.Once());
            mock.Verify(unit => unit.Commit(), Times.Once());
            mock.Verify(unit => unit.GetCurrentUserId(), Times.Once());
            Assert.AreEqual(42, room.Id);
            Assert.AreEqual(2, room.Members.Count);
            Assert.AreEqual("Eric", room.Members.Last().User.Login);
        }

        [TestMethod]
        public void LoadRecordsTest()
        {
            var record = new Record { Text = "Hello", CreationDate = DateTime.Now, Creator = new User() };
            mock.Setup(unit => unit.GetRecordsAfter(It.IsAny<int>(), It.IsAny<long>()))
                .Returns(new[]
                    {
                        record,
                        new Record {Text = "World", CreationDate = DateTime.Now, Creator = new User()},
                    });

            var jsonData = controller.LoadRecords(1, 2);
            var records = jsonData.Data as IEnumerable<JsonRecord>;

            Assert.IsNotNull(records);
            mock.Verify(unit => unit.GetRecordsAfter(1, 2));
            Assert.AreEqual(2, records.Count());
            Assert.AreEqual(record.ToString(), records.First().Text);
            Assert.AreEqual(record.CreationDate.ToBinary(), records.First().CreationDate);
        }

        [TestMethod]
        public void AddRecordTest()
        {
            mock.Setup(unit => unit.AddRecord(It.IsAny<int>(), It.IsAny<string>()));
            mock.Setup(unit => unit.Commit());

            controller.AddRecord(1, "Hello");

            mock.Verify(unit => unit.AddRecord(1, "Hello"), Times.Once());
            mock.Verify(unit => unit.Commit(), Times.Once());
        }

        [TestMethod]
        public void ExitTest()
        {
            mock.Setup(unit => unit.ExitRoom(It.IsAny<int>()));
            mock.Setup(unit => unit.Commit());

            var view = controller.Exit(1);

            mock.Verify(unit => unit.ExitRoom(1), Times.Once());
            mock.Verify(unit => unit.Commit(), Times.Once());
            Assert.AreEqual("List", view.RouteValues["action"]);
        }

        [TestMethod]
        public void RoomsPartialTest()
        {
            mock.Setup(unit => unit.GetCurrentUserId());
            mock.Setup(unit => unit.GetCurrentUserRooms())
                .Returns(new Collection<Room>
                    {
                        new Room {Id = 1, Title = "Amazing Room"},
                        new Room {Id = 2, Title = "Good Room"}
                    });

            var jsonData = controller.RoomsPartial();
            var rooms = jsonData.Data as IEnumerable<JsonRoom>;

            Assert.IsNotNull(rooms);
            mock.Verify(unit => unit.GetCurrentUserId(), Times.Once());
            mock.Verify(unit => unit.GetCurrentUserRooms(), Times.Once());
            Assert.AreEqual(2, rooms.Count());
            Assert.AreEqual("Amazing Room", rooms.First().Title);
        }
    } 
}