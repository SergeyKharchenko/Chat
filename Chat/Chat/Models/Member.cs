using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chat.Models
{
    [Table("Member")]
    public class Member : Entity
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int RoomId { get; set; }
        public virtual Room Room { get; set; }

        public DateTime EnterTime { get; set; }
    }
}