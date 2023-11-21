using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Miubuy.Models
{
    public class Tag
    {
        public Tag()
        {
            Delete = false;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "標籤名稱")]
        public string Name { get; set; }

        [Display(Name = "標籤顏色")]
        public string Color { get; set; }

        [Display(Name = "刪除")]
        public bool Delete { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
    }
}