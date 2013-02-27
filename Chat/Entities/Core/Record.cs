using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entities.Authorization;

namespace Entities.Core
{
    public class Record
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RecordId { get; set; }

        [Required]
        public string Text { get; set; }

        public User User { get; set; }

        //public Chat Chat { get; set; }
    }
}