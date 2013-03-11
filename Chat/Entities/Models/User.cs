using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Entities.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [HiddenInput(DisplayValue = false)]
        public int UserId { get; set; }

        public string Login { get; set; }

        public virtual ICollection<Chat> CreatedChats { get; set; }
        public virtual ICollection<Record> Records { get; set; }

        public virtual ICollection<Chat> ChatMembers { get; set; }
    }
}
