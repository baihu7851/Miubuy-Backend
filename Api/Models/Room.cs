using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public class Room
    {
        public Room()
        {
            R18 = false;
            MaxUsers = 1;
            Star = 0;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "國家Id")]
        public int CountryId { get; set; }

        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }

        [Display(Name = "郡Id")]
        public int CountyId { get; set; }

        [ForeignKey("CountyId")]
        public virtual County County { get; set; }

        [Display(Name = "城市Id")]
        public int CityId { get; set; }

        [ForeignKey("CityId")]
        public virtual City City { get; set; }

        [Display(Name = "主要標籤Id")]
        public int TagId { get; set; }

        [ForeignKey("TagId")]
        public virtual Tag Tag { get; set; }

        [Display(Name = "開房者Id")]
        public int SellerId { get; set; }

        [Required]
        [Display(Name = "房名")]
        public string Name { get; set; }

        [Display(Name = "圖片")]
        public string Picture { get; set; }

        [Display(Name = "規則")]
        public string Rule { get; set; }

        [Display(Name = "其他標籤")]
        public string TagText { get; set; }

        [Display(Name = "最大人數")]
        public int MaxUsers { get; set; }

        [Display(Name = "評分限制")]
        public int Star { get; set; }

        [Display(Name = "限制級開關")]
        public bool R18 { get; set; }

        [Display(Name = "房間開關")]
        public bool RoomClose { get; set; }

        [Display(Name = "開房時間")]
        public DateTime RoomStart { get; set; }

        [Display(Name = "截止時間")]
        public DateTime RoomEnd { get; set; }

        public virtual ICollection<RoomUser> RoomUsers { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}