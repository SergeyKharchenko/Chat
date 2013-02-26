using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entities.Core;

namespace Chat.Controllers
{
    public class ChatController : Controller
    {
        private readonly IChatRepository chatRepository;

        public ChatController(IChatRepository chatRepository)
        {
            this.chatRepository = chatRepository;
        }

        public ViewResult Index()
        {
            var chats = chatRepository.Chats.ToArray();
            return View();
        }
    }
}
