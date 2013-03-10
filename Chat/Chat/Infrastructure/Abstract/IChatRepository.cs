using System.Linq;
using Entities.Models;

namespace Chat.Infrastructure.Abstract
{
    public interface IChatRepository
    {
        IQueryable<Entities.Models.Chat> Chats { get; }

        Entities.Models.Chat GetChatById(int id);
        void Create(Entities.Models.Chat chat);
        void Update(Entities.Models.Chat chat);
        void Save();

        User GetUserById(int id);
    }
}