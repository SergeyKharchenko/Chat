using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Chat.Infrastructure.Abstract;
using Chat.ViewModels;
using Entities.Core;
using Entities.Core.Abstract;
using Entities.Models;
using WebMatrix.WebData;

namespace Chat.Controllers
{
    [Authorize]
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
        [ValidateAntiForgeryToken]
        public ActionResult Create(Entities.Models.Chat chat)
        {
            if (string.IsNullOrEmpty(chat.Title))
                return View();

            chat.CreatorionDate = DateTime.Now;
            var currentUser = chatRepository.GetUserById(authorizationService.GetCurrentuserId());
            chat.Creator = currentUser;
            chat.Members = new Collection<User> {currentUser};
            chatRepository.Create(chat);

            return RedirectToAction("List");
        }

        public ActionResult Join(int id)
        {
            throw new NotImplementedException();
        }
    }
}
