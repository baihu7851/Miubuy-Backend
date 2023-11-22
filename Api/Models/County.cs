using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public class County
    {
        public County()
        {
            Delete = false;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "國家Id")]
        public int CountryId { get; set; }

        [Required]
        [Display(Name = "郡名稱")]
        public string Name { get; set; }

        [Display(Name = "刪除")]
        [DefaultValue(false)]
        public bool Delete { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
    }
}