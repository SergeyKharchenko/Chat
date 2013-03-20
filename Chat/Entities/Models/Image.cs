using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Entities.Models
{
    [Table("Image")]
    public class Image : Entity
    {
        [HiddenInput(DisplayValue = false)]
        public string ImageMimeType { get; set; }

        public string FileName { get; set; }

        public User User { get; set; }
    }
}