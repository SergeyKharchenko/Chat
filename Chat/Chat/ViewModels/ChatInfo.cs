using System;
using System.Collections.Generic;
using Entities.Models;

namespace Chat.ViewModels
{
    public class ChatInfo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime LastActivity { get; set; }
        public string Creator { get; set; }
        public string[] Members { get; set; }
        public Record[] Records { get; set; }
    }
}