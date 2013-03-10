using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Chat.ViewModels;
using Entities.Core;
using Entities.Core.Abstract;

namespace Chat.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly IChatRepository chatRepository;

        public ChatController(IChatRepository chatRepository)
        {
            this.chatRepository = chatRepository;
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
                    Members = (from member in chat.Members select member.Login).ToArray(),
                    Records = chat.Records.Reverse().Take(3).Reverse().ToArray()
                };
            return View(chatInfo);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Entities.Models.Chat chat)
        {
            if (!ModelState.IsValid)
                return View();
            chatRepository.Create(chat);
            return RedirectToAction("List");
        }

        public ActionResult Join(int id)
        {
            throw new NotImplementedException();
        }
    }
}
