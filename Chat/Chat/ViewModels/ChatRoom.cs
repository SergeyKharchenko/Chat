using Entities.Models;

namespace Chat.ViewModels
{
    public class ChatRoom
    {
        public Room Room { get; set; }
        public User CurrentUser { get; set; }
    }
}