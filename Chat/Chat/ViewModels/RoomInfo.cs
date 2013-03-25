using System;
using System.Collections.Generic;
using System.Linq;
using Chat.Models;

namespace Chat.ViewModels
{
    public class RoomInfo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime LastActivity { get; set; }
        public string CreatorName { get; set; }
        public DateTime CreationDate { get; set; }
        public string[] MemberNames { get; set; }
        public Record[] Records { get; set; }
        public bool IsCreator { get; set; }

        public RoomInfo(Room room, int currentUserId, int recordsToShowCount = 3)
        {
            Id = room.Id;
            Title = room.Title;
            CreatorName = room.Creator.Login;
            CreationDate = room.CreatorionDate;
            LastActivity = room.LastActivity;
            if (room.Members != null)
                MemberNames = (from member in room.Members select member.User.Login).ToArray();
            if (room.Records != null)
                Records = room.Records.Reverse().Take(recordsToShowCount).Reverse().ToArray();
            IsCreator = room.CreatorId == currentUserId;
        }
    }
}