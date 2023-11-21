using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Miubuy.Models
{
    public class TempDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "產品名稱")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "單價")]
        public decimal Price { get; set; }

        [Display(Name = "買家Id")]
        public int BuyerId { get; set; }

        [Display(Name = "房間Id")]
        public int RoomId { get; set; }
    }
}