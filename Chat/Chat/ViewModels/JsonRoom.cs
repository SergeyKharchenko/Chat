using Chat.Models;

namespace Chat.ViewModels
{
    public class JsonRoom
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsCreator { get; set; }

        public JsonRoom(Room room, int currentUserId)
        {
            Id = room.Id;
            Title = room.Title;
            IsCreator = room.CreatorId == currentUserId;
        }
    }
}