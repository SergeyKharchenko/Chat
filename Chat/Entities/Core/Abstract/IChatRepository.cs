using System.Linq;
using Entities.Models;

namespace Entities.Core.Abstract
{
    public interface IChatRepository
    {
        IQueryable<Chat> Chats { get; }

        Chat GetChatById(int id);
        void Create(Chat chat);
        void Save();
    }
}