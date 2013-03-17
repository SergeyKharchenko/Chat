using System;
using System.Collections.ObjectModel;
using Entities.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chat.Tests.Tests.Model
{
    [TestClass]
    public class RoomTests
    {
        [TestMethod]
        public void LastActivityWithoutRecordsTest()
        {
            var room = new Room {CreatorionDate = DateTime.Now};

            var lastActivity = room.LastActivity;

            Assert.AreEqual(lastActivity, room.CreatorionDate);
        }

        [TestMethod]
        public void LastActivityWithRecordsTest()
        {
            var record1 = new Record {CreationDate = DateTime.Now.AddDays(1)};
            var record2 = new Record {CreationDate = DateTime.Now.AddDays(2)};
            var room = new Room
                {
                    CreatorionDate = DateTime.Now,
                    Records = new Collection<Record> {record1, record2}
                };

            var lastActivity = room.LastActivity;

            Assert.AreEqual(lastActivity, record2.CreationDate);
        }
    }
}
