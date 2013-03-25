using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chat.Models
{
    [Table("User")]
    public class User : Entity
    {
        public string Login { get; set; }

        public virtual ICollection<Room> CreatedRooms { get; set; }
        public ICollection<Record> Records { get; set; }

        public virtual ICollection<Member> Members { get; set; }

        public virtual Image Image { get; set; }
    }
}
