using System.Linq;
using Entities.Models;

namespace Chat.Infrastructure.Abstract
{
    public interface IChatRepository
    {
        IQueryable<Entities.Models.Chat> Chats { get; }

        Entities.Models.Chat GetChatById(int id);
        void CreateChat(Entities.Models.Chat chat);
        void CreateRecord(Record record);

        void UpdateChat(Entities.Models.Chat chat);
        void UpdateRecord(Record record);
        void Save();

        User GetUserById(int id);
    }
}