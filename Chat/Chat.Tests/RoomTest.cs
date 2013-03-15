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