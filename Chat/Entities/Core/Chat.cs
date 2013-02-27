using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entities.Authorization;

namespace Entities.Core
{
    public class Chat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChatId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must not be more than 50 characters")]
        public string Title { get; set; }

        //public virtual User Creator { get; set; }

        //public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<Record> Records { get; set; }

        public DateTime LastActivity { get; set; }
    }
}