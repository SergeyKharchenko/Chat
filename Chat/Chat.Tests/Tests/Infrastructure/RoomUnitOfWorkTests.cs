using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using Chat.Infrastructure.Abstract;
using Chat.Infrastructure.Concrete;
using Entities.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;

namespace Chat.Tests.Tests.Infrastructure
{
    [TestClass]
    public class RoomUnitOfWorkTests
    {
        [TestMethod]
        public void GetAllRoomsTest()
        {
            var mock = new Mock<IRepository<Room>>();
            mock.Setup(repo => repo.Entities);
            var unitOfWork = new RoomUnitOfWork{RoomRepository = mock.Object};

            var rooms = unitOfWork.Rooms;

            mock.Verify(repo => repo.Entities);
        }

        [TestMethod]
        public void FindRoomByIdTest()
        {
            var mock = new Mock<IRepository<Room>>();
            mock.Setup(repo => repo.FindById(It.IsAny<int>()));
            var unitOfWork = new RoomUnitOfWork { RoomRepository = mock.Object };

            unitOfWork.FindRoomById(3);

            mock.Verify(repo => repo.FindById(3));
        }

        [TestMethod]
        public void CreateRoomTest()
        {
            var mockRoomRepo = new Mock<IRepository<Room>>();
            mockRoomRepo.Setup(repo => repo.Add(It.IsAny<Room>()));

            var mockAuthService = new Mock<IAuthorizationService>();
            var user = new User {Login = "Tom"};
            mockAuthService.Setup(service => service.GetCurrentUser())
                .Returns(user);

            var unitOfWork = new RoomUnitOfWork
                {
                    RoomRepository = mockRoomRepo.Object,
                    AuthorizationService = mockAuthService.Object
                };

            var room = new Room {Title = "Amazing room"};
            unitOfWork.CreateRoom(room);

            mockAuthService.Verify(service => service.GetCurrentUser(), Times.Once());
            Assert.AreSame(user, room.Creator);
            Assert.AreEqual(1, room.Members.Count);
            Assert.AreSame(user, room.Members.First().User);
            Assert.AreSame(room, room.Members.First().Room);
            mockRoomRepo.Verify(repo => repo.Add(room), Times.Once());
        }

        [TestMethod]
        public void JoinRoomWithAddMemberTest()
        {
            Mock<IRepository<Room>> mockRoomRepo;
            Mock<IRepository<Member>> mockMemberRepo;
            Mock<IAuthorizationService> mockAuthService;
            var unitOfWork = CreateUnitOfWorkForJoinRoomTest(out mockRoomRepo, out mockMemberRepo, out mockAuthService);

            mockMemberRepo.Setup(repo => repo.Entities)
                          .Returns(new Collection<Member>());

            unitOfWork.JoinRoom(1);

            mockAuthService.Verify(service => service.GetCurrentUserId(), Times.Once());
            mockRoomRepo.Verify(repo => repo.FindBy(It.IsAny<Expression<Func<Room, bool>>>(),
                                                    It.IsAny<Expression<Func<Room, object>>[]>()), Times.Once());
            mockMemberRepo.Verify(repo => repo.Entities, Times.Once());
            mockMemberRepo.Verify(
                repo => repo.Add(It.Is((Member member) => member.RoomId == 100500 && member.UserId == 42)), Times.Once());
        }
        [TestMethod]
        public void JoinRoomWithoutAddMemberTest()
        {
            Mock<IRepository<Room>> mockRoomRepo;
            Mock<IRepository<Member>> mockMemberRepo;
            Mock<IAuthorizationService> mockAuthService;
            var unitOfWork = CreateUnitOfWorkForJoinRoomTest(out mockRoomRepo, out mockMemberRepo, out mockAuthService);

            mockMemberRepo.Setup(repo => repo.Entities)
                          .Returns(new Collection<Member>{new Member {RoomId = 100500, UserId = 42}});

            unitOfWork.JoinRoom(1);

            mockAuthService.Verify(service => service.GetCurrentUserId(), Times.Once());
            mockRoomRepo.Verify(repo => repo.FindBy(It.IsAny<Expression<Func<Room, bool>>>(),
                                                    It.IsAny<Expression<Func<Room, object>>[]>()), Times.Once());
            mockMemberRepo.Verify(repo => repo.Entities, Times.Once());
            mockMemberRepo.Verify(repo => repo.Add(It.IsAny<Member>()), Times.Never());
        }

        private RoomUnitOfWork CreateUnitOfWorkForJoinRoomTest(out Mock<IRepository<Room>> mockRoomRepo,
                                                               out Mock<IRepository<Member>> mockMemberRepo,
                                                               out Mock<IAuthorizationService> mockAuthService)
        {
            mockRoomRepo = new Mock<IRepository<Room>>();
            mockRoomRepo.Setup(repo => repo.FindBy(It.IsAny<Expression<Func<Room, bool>>>(),
                                                   It.IsAny<Expression<Func<Room, object>>[]>()))
                        .Returns(new Collection<Room> {new Room {Id = 100500}});

            mockMemberRepo = new Mock<IRepository<Member>>();
            mockMemberRepo.Setup(repo => repo.Add(It.IsAny<Member>()));

            mockAuthService = new Mock<IAuthorizationService>();
            mockAuthService.Setup(service => service.GetCurrentUserId())
                           .Returns(42);

            return new RoomUnitOfWork
                {
                    RoomRepository = mockRoomRepo.Object,
                    MemberRepository = mockMemberRepo.Object,
                    AuthorizationService = mockAuthService.Object
                };
        }
    
        [TestMethod]
        public void ExitRoomTest()
        {
            var mockMemberRepo = new Mock<IRepository<Member>>();
            mockMemberRepo.Setup(repo => repo.Entities)
                          .Returns(new Collection<Member> { new Member { RoomId = 100500, UserId = 42 } });
            mockMemberRepo.Setup(repo => repo.Remove(It.IsAny<Member>()));

            var mockAuthService = new Mock<IAuthorizationService>();
            mockAuthService.Setup(service => service.GetCurrentUserId())
                           .Returns(42);

            var unitOfWork = new RoomUnitOfWork
            {
                MemberRepository = mockMemberRepo.Object,
                AuthorizationService = mockAuthService.Object
            };

            unitOfWork.ExitRoom(100500);

            mockAuthService.Verify(service => service.GetCurrentUserId(), Times.Once());
            mockMemberRepo.Verify(repo => repo.Entities, Times.Once());
            mockMemberRepo.Verify(
                repo => repo.Remove(It.Is((Member member) => member.RoomId == 100500 && member.UserId == 42)),
                Times.Once());
        }

        [TestMethod]
        public void AddRecordTest()
        {
            var mockRecordRepo = new Mock<IRepository<Record>>();
            mockRecordRepo.Setup(repo => repo.Add(It.IsAny<Record>()));


            var mockAuthService = new Mock<IAuthorizationService>();
            mockAuthService.Setup(service => service.GetCurrentUserId())
                           .Returns(42);

            var unitOfWork = new RoomUnitOfWork
                {
                    RecordRepository = mockRecordRepo.Object,
                    AuthorizationService = mockAuthService.Object
                };

            unitOfWork.AddRecord(100500, "Hello");

            mockAuthService.Verify(service => service.GetCurrentUserId(), Times.Once());
            mockRecordRepo.Verify(
                repo =>
                repo.Add(
                    It.Is((Record record) => record.CreatorId == 42 && record.RoomId == 100500 && record.Text == "Hello")),
                Times.Once());
        }

        [TestMethod]
        public void GetRecordsAfterTest()
        {
            var mockRoomRepo = new Mock<IRepository<Room>>();
            mockRoomRepo.Setup(repo => repo.FindBy(It.IsAny<Expression<Func<Room, bool>>>(),
                                                   It.IsAny<Expression<Func<Room, object>>[]>()))
                        .Returns(new Collection<Room>
                            {
                                new Room
                                    {
                                        Id = 100500,
                                        Records = new Collection<Record>
                                            {
                                                new Record {RoomId = 100500, Text = "Hello", CreationDate = DateTime.MinValue},
                                                new Record {RoomId = 100500, Text = "World", CreationDate = DateTime.Now},
                                                new Record {RoomId = 100500, Text = "Sun", CreationDate = DateTime.MaxValue},
                                            }
                                    }
                            });

            var unitOfWork = new RoomUnitOfWork {RoomRepository = mockRoomRepo.Object};

            var records = unitOfWork.GetRecordsAfter(100500, (DateTime.Now + TimeSpan.FromMinutes(1)).ToBinary());

            mockRoomRepo.Verify(repo => repo.FindBy(It.IsAny<Expression<Func<Room, bool>>>(),
                                                    It.IsAny<Expression<Func<Room, object>>[]>()), Times.Once());
            Assert.AreEqual(1, records.Count());
            Assert.AreEqual(100500, records.First().RoomId);
            Assert.AreEqual("Sun", records.First().Text);
        }
    }
}
