using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Entities.Models
{
    [Table("Record")]
    public class Record
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [HiddenInput(DisplayValue = false)]
        public int RecordId { get; set; }

        [Required]
        public string Text { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int CreatorId { get; set; }
        public User Creator { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int ChatId { get; set; }
        public Chat Chat { get; set; }
    }
}