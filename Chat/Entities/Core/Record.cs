﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Core
{
    public class Record
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RecordId { get; set; }

        [Required]
        public string Text { get; set; }

        public Core.Chat Chat { get; set; }
    }
}