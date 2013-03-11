using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Entities.Models
{
    [Table("Member")]
    public class Member
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MemberId { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int ChatId { get; set; }
        public virtual Chat Chat { get; set; }

        public DateTime EnterTime { get; set; }
    }
}