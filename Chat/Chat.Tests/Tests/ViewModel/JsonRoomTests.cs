using System;
using System.Collections.ObjectModel;
using Chat.Models;
using Chat.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chat.Tests.Tests.ViewModel
{
    [TestClass]
    public class JsonRoomTests
    {
        [TestMethod]
        public void MyRoomToJsonRoomConvertTest()
        {
            var room = new Room
            {
                Id = 42,
                Title = "Amazing Room",
                CreatorId = 1,
                Members = new Collection<Member>
                        {
                            new Member {UserId = 2}
                        }
            };

            var jsonRoom = new JsonRoom(room, 1);

            Assert.AreEqual(42, jsonRoom.Id);
            Assert.AreEqual("Amazing Room", jsonRoom.Title);
            Assert.IsTrue(jsonRoom.IsCreator);
        }

        [TestMethod]
        public void NotMyRoomToJsonRoomConvertTest()
        {
            var room = new Room
            {
                Id = 42,
                Title = "Amazing Room",
                CreatorId = 1,
                Members = new Collection<Member>
                        {
                            new Member {UserId = 2}
                        }
            };

            var jsonRoom = new JsonRoom(room, 2);

            Assert.AreEqual(42, jsonRoom.Id);
            Assert.AreEqual("Amazing Room", jsonRoom.Title);
            Assert.IsFalse(jsonRoom.IsCreator);
        }
    }
}
