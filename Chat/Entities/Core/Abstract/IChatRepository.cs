using System.Linq;
using Entities.Models;

namespace Entities.Core.Abstract
{
    public interface IChatRepository
    {
        IQueryable<Chat> Chats { get; } 
    }
}