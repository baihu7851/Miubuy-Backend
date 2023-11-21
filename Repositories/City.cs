using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories
{
    public class City
    {
        public City()
        {
            Delete = false;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "郡Id")]
        public int CountyId { get; set; }

        [Required]
        [Display(Name = "地區名稱")]
        public string Name { get; set; }

        [Display(Name = "刪除")]
        [DefaultValue(false)]
        public bool Delete { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
    }
}