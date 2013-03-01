using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entities.Core;
using Entities.Core.Abstract;

namespace Chat.Controllers
{
    public class ChatController : Controller
    {
        private readonly IChatRepository chatRepository;

        public ChatController(IChatRepository chatRepository)
        {
            this.chatRepository = chatRepository;
        }

        public ViewResult List()
        {
            return View(chatRepository.Chats);
        }
    }
}
