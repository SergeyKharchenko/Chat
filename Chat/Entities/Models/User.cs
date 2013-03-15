﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Entities.Models
{
    [Table("User")]
    public class User : Entity
    {
        public string Login { get; set; }

        public virtual ICollection<Room> CreatedRooms { get; set; }
        public virtual ICollection<Record> Records { get; set; }

        public virtual ICollection<Member> Members { get; set; }
    }
}
