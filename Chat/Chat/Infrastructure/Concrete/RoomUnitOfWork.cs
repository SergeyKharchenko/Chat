using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Chat.Infrastructure.Abstract;
using Entities.Models;

namespace Chat.Infrastructure.Concrete
{
    public class RoomUnitOfWork : IRoomUnitOfWork
    {
        private readonly ChatContext context = new ChatContext();

        public IAuthorizationService AuthorizationService { get; set; }

        public IRepository<User> UserRepository { get; set; }
        public IRepository<Room> RoomRepository { get; set; }
        public IRepository<Record> RecordRepository { get; set; }
        public IRepository<Member> MemberRepository { get; set; }

        public RoomUnitOfWork()
        {
            AuthorizationService = new WebSecurityAuthorizationService(context);

            UserRepository = new Repository<User>(context.Set<User>());
            RoomRepository = new Repository<Room>(context.Set<Room>());
            RecordRepository = new Repository<Record>(context.Set<Record>());
            MemberRepository = new Repository<Member>(context.Set<Member>());
        }

        public IEnumerable<Room> Rooms { get { return RoomRepository.Entities; } }

        public Room FindRoomById(int id)
        {
            return RoomRepository.FindBy(room => room.Id == id,
                                         room => room.Members, room => room.Records)
                                 .Single();
        }

        public void CreateRoom(Room room)
        {
            room.CreatorionDate = DateTime.Now;
            var currentUserId = AuthorizationService.GetCurrentUserId();
            room.CreatorId = currentUserId;
            room.Members = new Collection<Member>
                {
                    new Member {UserId = currentUserId, Room = room, EnterTime = DateTime.Now}
                };
            RoomRepository.Add(room);
        }

        public Room JoinRoom(int id)
        {
            var userId = AuthorizationService.GetCurrentUserId();
            var room = RoomRepository.FindBy(r => r.Id == id,
                                             r => r.Records, r => r.Members)
                                     .Single();
            if (!MemberRepository.Entities.Any(member => member.RoomId == room.Id && member.UserId == userId))
                MemberRepository.Add(new Member
                {
                    UserId = userId,
                    RoomId = room.Id,
                    EnterTime = DateTime.Now
                });
            return room;
        }

        public void ExitRoom(int id)
        {
            var userId = AuthorizationService.GetCurrentUserId();
            var member = MemberRepository.Entities.FirstOrDefault(m => m.RoomId == id && m.UserId == userId);
            MemberRepository.Remove(member);
            Commit();
            var room = FindRoomById(id);
            if (room.Members.Count == 0)
            {
                for (var i = room.Records.Count - 1; i >= 0; i--)
                    RecordRepository.Remove(room.Records.ElementAt(i));
                RoomRepository.Remove(room);
            }
        }

        public void AddRecord(int roomId, string recordText)
        {
            var record = new Record
            {
                CreationDate = DateTime.Now,
                CreatorId = AuthorizationService.GetCurrentUserId(),
                RoomId = roomId,
                Text = recordText
            };
            RecordRepository.Add(record);
        }

        public IEnumerable<Record> GetRecordsAfter(int roomId, long binaryDate)
        {
            var room = RoomRepository.FindBy(filterCriterion: r => r.Id == roomId,
                                             includeCriterion: r => r.Records)
                                     .Single();
            var records = room.Records.Where(record => record.CreationDate > DateTime.FromBinary(binaryDate));
            return records;
        }

        public IEnumerable<Room> GetCurrentUserRooms()
        {
            var currentUserId = AuthorizationService.GetCurrentUserId();
            var rooms = MemberRepository.FindBy(member => member.UserId == currentUserId)
                .Select(member => member.Room);
            return rooms;
        }

        public int GetCurrentUserId()
        {
            return AuthorizationService.GetCurrentUserId();
        }

        public void Commit()
        {
            context.SaveChanges();
        }
    }
}