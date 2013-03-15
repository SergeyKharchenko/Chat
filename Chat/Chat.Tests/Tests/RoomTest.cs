using System;
using System.Collections.ObjectModel;
using Chat.Controllers;
using Chat.Infrastructure.Abstract;
using Entities.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Chat.Tests.Tests
{
    [TestClass]
    public class RoomTest
    {
        private Mock<IAuthorizationService> authorizationServiceMock;
        private Collection<User> users;

        private Mock<IRepository<Room>> roomRepositoryMock;
        private Mock<IRepository<Record>> recordRepositoryMock;
        private Mock<IRepository<Member>> memberRepositoryMock;

        private RoomController roomController;

        [TestInitialize]
        public void InitializeChats()
        {           
           
        }

        [TestMethod]
        public void CanShowAllRoomsTest()
        {                   
           
        }

        [TestMethod]
        public void CanGetRoomInfoByRightIdTest()
        {
         
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CannotGetRoomInfoByWrongIdTest()
        {
            
        }

        [TestMethod]
        public void GetRoomLastActivityTest()
        {
       
        }

        [TestMethod]
        public void CreateRoomTest()
        {
          
        }

        [TestMethod]
        public void CanEnterTheRoomWihtoutAddMemberTest()
        {
           
        }

        [TestMethod]
        public void CanEnterTheRoomWihtAddMemberTest()
        {
        }
    }
}