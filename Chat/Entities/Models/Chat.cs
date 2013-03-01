using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Entities.Models
{
    [Table("Chat")]
    public class Chat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [HiddenInput(DisplayValue = false)]
        public int ChatId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must not be more than 50 characters")]
        public string Title { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int CreatorId { get; set; }
        public virtual User Creator { get; set; }

        public virtual ICollection<User> Participants { get; set; }

        public virtual ICollection<Record> Records { get; set; }

        [Display(Name = "Last activity")]
        public DateTime LastActivity { get; set; }
    }
}