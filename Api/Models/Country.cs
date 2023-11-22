using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public class Country
    {
        public Country()
        {
            Delete = false;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "國家名稱")]
        public string Name { get; set; }

        [Display(Name = "刪除")]
        [DefaultValue(false)]
        public bool Delete { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
    }
}