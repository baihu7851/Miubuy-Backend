using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "連線Id")]
        public string ConnectionId { get; set; }

        [Required]
        [Display(Name = "帳號")]
        public string Account { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密碼")]
        public string Password { get; set; }

        [Display(Name = "密碼鹽")]
        public string PasswordSalt { get; set; }

        [Display(Name = "暱稱")]
        public string Nickname { get; set; }

        [Display(Name = "頭像")]
        public string Picture { get; set; }

        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Display(Name = "生日")]
        public DateTime? Birthday { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)] // 不是驗證
        [Display(Name = "信箱")]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]// 這是美國的
        [Display(Name = "電話")]
        public string Phone { get; set; }

        [Display(Name = "買方評價分數")]
        public float BuyerAverageStar { get; set; }

        [Display(Name = "買方評價分數")]
        public float SellerAverageStar { get; set; }

        [Display(Name = "權限")]
        public int Permission { get; set; }

        public virtual ICollection<RoomUser> RoomUsers { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }

    public class Login
    {
        [Required]
        [Display(Name = "帳號")]
        public string Account { get; set; }

        [Required]
        [Display(Name = "密碼")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class CheckUser
    {
        [Required]
        [Display(Name = "帳號")]
        public string Account { get; set; }
    }
}