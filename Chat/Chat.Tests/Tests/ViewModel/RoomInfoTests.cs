﻿using System;
using System.Collections.ObjectModel;
using Chat.ViewModels;
using Entities.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Chat.Tests.Tests.ViewModel
{
    [TestClass]
    public class RoomInfoTests
    {
        [TestMethod]
        public void RoomToRoomInfoConvertTest()
        {
            var creator = new User { Login = "John" };
            var room = new Room
            {
                Id = 42,
                Title = "Amazing Room",
                Creator = creator,
                CreatorId = 1,
                CreatorionDate = DateTime.MaxValue,
                Records = new Collection<Record>
                        {
                            new Record {Id = 1, Text = "Hello"},
                            new Record {Id = 2, Text = "World"},
                            new Record {Id = 3, Text = "!"},
                        },
                Members = new Collection<Member>
                        {
                            new Member {User = creator}
                        }
            };

            var roomInfo = new RoomInfo(room, 1, 2);

            Assert.AreEqual(42, roomInfo.Id);
            Assert.AreEqual("Amazing Room", roomInfo.Title);
            Assert.AreEqual("John", roomInfo.CreatorName);
            Assert.AreEqual(DateTime.MaxValue, roomInfo.CreationDate);
            Assert.AreEqual(1, roomInfo.MemberNames.Count());
            Assert.AreEqual("John", roomInfo.MemberNames.First());
            Assert.AreEqual(2, roomInfo.Records.Count());
            Assert.AreEqual("World", roomInfo.Records.First().Text);
            Assert.IsTrue(roomInfo.IsCreator);

            roomInfo = new RoomInfo(room, 2);
            Assert.IsFalse(roomInfo.IsCreator);
        }
    }
}
