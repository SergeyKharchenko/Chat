using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        private readonly IChatRepository chatRepository;
        private readonly IAuthorizationService authorizationService;

        public ChatController(IChatRepository chatRepository, IAuthorizationService authorizationService)
        {
            this.chatRepository = chatRepository;
            this.authorizationService = authorizationService;
        }

        [AllowAnonymous]
        public ViewResult List()
        {
            return View(chatRepository.Chats);
        }

        public ViewResult Info(int id)
        {
            var chat = chatRepository.GetChatById(id);
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
            var currentUser = chatRepository.GetUserById(authorizationService.GetCurrentuserId());
            chat.Creator = currentUser;
            chat.Members = new Collection<Member> {new Member {User = currentUser, Chat = chat, EnterTime = DateTime.Now}};
            chatRepository.Create(chat);

            return RedirectToAction("List");
        }

        public ViewResult JoinRoom(int id)
        {
            var currentUser = chatRepository.GetUserById(authorizationService.GetCurrentuserId());
            var chat = chatRepository.GetChatById(id);
            chat.Members.Remove(chat.Members.FirstOrDefault(member => member.UserId == currentUser.UserId));
            return View("Room", chat);
        }

        [HttpPost]
        public JsonResult LoadRoom(int chatId, long lastRecordsCreationDate)
        {
            var chat = chatRepository.GetChatById(chatId);
            var records = chat.Records.Where(record => record.CreationDate.ToBinary() > lastRecordsCreationDate)
                              .Select(
                                  record =>
                                  new {Text = record.ToString(), CreationDate = record.CreationDate.ToBinary()});
            return Json(records);
        }

        [HttpPost]
        public ActionResult AddRecord(int chatId, string record)
        {
            var chat = chatRepository.GetChatById(chatId);
            var currentUser = chatRepository.GetUserById(authorizationService.GetCurrentuserId());
            chat.Records.Add(new Record {CreationDate = DateTime.Now, Creator = currentUser, Text = record});
            chatRepository.Update(chat);
            return new EmptyResult();
        }
    }
}
