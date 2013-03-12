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
    public class ChatController : Controller
    {
        private readonly IAuthorizationService authorizationService;
        private readonly IEntityRepository<Entities.Models.Chat> chatRepository;
        private readonly IEntityRepository<Record> recordRepository;
        private readonly IEntityRepository<Member> memberRepository;

        public ChatController(IEntityRepository<Entities.Models.Chat> chatRepository,
                              IEntityRepository<Record> recordRepository,
                              IEntityRepository<Member> memberRepository,
                              IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
            this.chatRepository = chatRepository;
            this.recordRepository = recordRepository;
            this.memberRepository = memberRepository;
        }

        [AllowAnonymous]
        public ViewResult List()
        {
            return View(chatRepository.Entities);
        }

        public ViewResult Info(int chatId)
        {
            var chat = chatRepository.GetById(chatId);
            var chatInfo = new ChatInfo
                {
                    Id = chat.ChatId,
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
        public ActionResult Create(Entities.Models.Chat chat)
        {
            if (string.IsNullOrEmpty(chat.Title))
                return View();

            chat.CreatorionDate = DateTime.Now;
            var currentUser = authorizationService.GetCurrentUser();
            chat.Creator = currentUser;
            chat.Members = new Collection<Member>
                {
                    new Member {User = currentUser, Chat = chat, EnterTime = DateTime.Now}
                };
            chatRepository.Create(chat);

            return RedirectToAction("List");
        }

        public ViewResult JoinRoom(int chatId)
        {
            var currentUser = authorizationService.GetCurrentUser();
            var chat = chatRepository.GetById(chatId);
            if (!memberRepository.Entities.Any(member => member.ChatId == chat.ChatId && member.UserId == currentUser.UserId))
                memberRepository.Create(new Member
                    {
                        UserId = currentUser.UserId,
                        ChatId = chat.ChatId,
                        EnterTime = DateTime.Now
                    });
            chat.Members.Remove(chat.Members.FirstOrDefault(member => member.UserId == currentUser.UserId));
            return View("Room", chat);
        }

        [HttpPost]
        public JsonResult LoadRecords(int chatId, long lastRecordsCreationDate)
        {
            var chat = chatRepository.GetById(chatId);
            var records = chat.Records.Where(record => record.CreationDate.ToBinary() > lastRecordsCreationDate)
                              .Select(
                                  record =>
                                  new {Text = record.ToString(), CreationDate = record.CreationDate.ToBinary()});
            return Json(records);
        }

        [HttpPost]
        public ActionResult AddRecord(int chatId, string text)
        {
            var record = new Record
                {
                    CreationDate = DateTime.Now,
                    Creator = authorizationService.GetCurrentUser(),
                    ChatId = chatId,
                    Text = text
                };
            recordRepository.Create(record);
            return new EmptyResult();
        }

        public RedirectToRouteResult ExitRoom(int chatId)
        {
            var currentUser = authorizationService.GetCurrentUser();
            var member =
                memberRepository.Entities.FirstOrDefault(m => m.ChatId == chatId && m.UserId == currentUser.UserId);
            memberRepository.Delete(member);

            return RedirectToAction("List");
        }
    }
}
