using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Entities.Models
{
    [Table("Record")]
    public class Record : Entity
    {
        [Required]
        public string Text { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int CreatorId { get; set; }
        public virtual User Creator { get; set; }
        public DateTime CreationDate { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int RoomId { get; set; }
        public Room Room { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}: {2}", CreationDate, Creator.Login, Text);
        }
    }
}