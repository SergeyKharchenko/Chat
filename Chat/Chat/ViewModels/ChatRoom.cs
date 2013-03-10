using Entities.Models;

namespace Chat.ViewModels
{
    public class ChatRoom
    {
        public Entities.Models.Chat Chat { get; set; }
        public User CurrentUser { get; set; }
    }
}