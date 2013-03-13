using System.Linq;
using Entities.Models;

namespace Entities.Core.Abstract
{
    public interface IChatContext
    {
        IQueryable<User> Users { get; set; }
        IQueryable<Room> Rooms { get; set; }
        IQueryable<Record> Records { get; set; }
        IQueryable<Member> Members { get; set; }
    }
}