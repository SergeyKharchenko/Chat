using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc;
using Chat.Filters;
using Chat.Infrastructure.Abstract;
using Chat.ViewModels;
using Entities.Models;

namespace Chat.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class RoomController : Controller
    {
        private readonly IAuthorizationService authorizationService;
        private readonly IEntityRepository<Room> roomRepository;
        private readonly IEntityRepository<Record> recordRepository;
        private readonly IEntityRepository<Member> memberRepository;

        public RoomController(IEntityRepository<Room> roomRepository,
                              IEntityRepository<Record> recordRepository,
                              IEntityRepository<Member> memberRepository,
                              IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
            this.roomRepository = roomRepository;
            this.recordRepository = recordRepository;
            this.memberRepository = memberRepository;
        }

        [AllowAnonymous]
        public ViewResult List()
        {
            return View(roomRepository.Entities);
        }

        public ViewResult Info(int roomId)
        {
            var chat = roomRepository.GetById(roomId);
            var chatInfo = new ChatInfo
                {
                    Id = chat.RoomId,
                    Title = chat.Title,
                    Creator = chat.Creator.Login,
                    LastActivity = chat.LastActivity,
                    Members = (from member in chat.Members select member.User.Login).ToArray(),
                    Records = chat.Records.Reverse().Take(3).Reverse().ToArray()
                };
            return View(chatInfo);
        }

        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Room room)
        {
            if (string.IsNullOrEmpty(room.Title))
                return View();

            room.CreatorionDate = DateTime.Now;
            var currentUser = authorizationService.GetCurrentUser();
            room.Creator = currentUser;
            room.Members = new Collection<Member>
                {
                    new Member {User = currentUser, Room = room, EnterTime = DateTime.Now}
                };
            roomRepository.Create(room);

            return RedirectToAction("List");
        }

        public ViewResult JoinRoom(int roomId)
        {
            var currentUser = authorizationService.GetCurrentUser();
            var room = roomRepository.GetById(roomId);
            if (!memberRepository.Entities.Any(member => member.RoomId == room.RoomId && member.UserId == currentUser.UserId))
                memberRepository.Create(new Member
                    {
                        UserId = currentUser.UserId,
                        RoomId = room.RoomId,
                        EnterTime = DateTime.Now
                    });
            room.Members.Remove(room.Members.FirstOrDefault(member => member.UserId == currentUser.UserId));
            return View("Room", room);
        }

        [HttpPost]
        public JsonResult LoadRecords(int roomId, long lastRecordsCreationDate)
        {
            var chat = roomRepository.GetById(roomId);
            var records = chat.Records.Where(record => record.CreationDate.ToBinary() > lastRecordsCreationDate)
                              .Select(
                                  record =>
                                  new {Text = record.ToString(), CreationDate = record.CreationDate.ToBinary()});
            return Json(records);
        }

        [HttpPost]
        public ActionResult AddRecord(int roomId, string text)
        {
            var record = new Record
                {
                    CreationDate = DateTime.Now,
                    Creator = authorizationService.GetCurrentUser(),
                    RoomId = roomId,
                    Text = text
                };
            recordRepository.Create(record);
            return new EmptyResult();
        }

        public RedirectToRouteResult ExitRoom(int roomId)
        {
            var currentUser = authorizationService.GetCurrentUser();
            var member =
                memberRepository.Entities.FirstOrDefault(m => m.RoomId == roomId && m.UserId == currentUser.UserId);
            memberRepository.Delete(member);

            return RedirectToAction("List");
        }
    }
}
