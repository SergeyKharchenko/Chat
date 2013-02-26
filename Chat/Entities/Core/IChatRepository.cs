using System.Linq;

namespace Entities.Core
{
    public interface IChatRepository
    {
        IQueryable<Core.Chat> Chats { get; } 
    }
}