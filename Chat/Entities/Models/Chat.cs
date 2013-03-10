using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using System.Linq;

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

        [Display(Name = "Creation date")]
        public DateTime CreatorionDate { get; set; }

        public virtual ICollection<User> Members { get; set; }

        public virtual ICollection<Record> Records { get; set; }

        public DateTime LastActivity
        {
            get
            {
                if (Records == null || Records.Count == 0)
                    return CreatorionDate;
                return Records.Max(record => record.CreationDate);
            }
        }
    }
}