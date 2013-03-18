using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using System.Linq;

namespace Entities.Models
{
    [Table("Room")]
    public class Room : Entity
    {
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must not be more than 50 characters")]
        public string Title { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int CreatorId { get; set; }
        public virtual User Creator { get; set; }

        [Display(Name = "Creation date")]
        [HiddenInput(DisplayValue = false)]
        public DateTime CreatorionDate { get; set; }

        public ICollection<Member> Members { get; set; }

        public ICollection<Record> Records { get; set; }

        [HiddenInput(DisplayValue = false)]
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