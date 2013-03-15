using System.Collections.Generic;
using Entities.Models;

namespace Chat.Infrastructure.Abstract
{
    public interface IRoomUnitOfWork : IUnitOfWork
    {
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