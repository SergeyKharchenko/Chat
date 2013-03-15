using System.Collections.Generic;
using Entities.Models;

namespace Chat.Infrastructure.Abstract
{
    public interface IRoomUnitOfWork : IUnitOfWork
    {
        IRepository<User> UserRepository { get; set; }
        IRepository<Room> RoomRepository { get; set; }
        IRepository<Record> RecordRepository { get; set; }
        IRepository<Member> MemberRepository { get; set; }

        IEnumerable<Room> Rooms { get; }
        Room FindRoomById(int id);
        void CreateRoom(Room room);
        Room JoinRoom(int id);
        void ExitRoom(int id);

        void AddRecord(int roomId, string recordText);
        IEnumerable<Record> GetRecordsAfter(int roomId, long binaryDate);

        int GetCurrentUserId();
    }
}