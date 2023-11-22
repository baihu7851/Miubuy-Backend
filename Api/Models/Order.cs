using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common;

namespace Api.Models
{
    public class Order
    {
        public Order()
        {
            Name = "";
            Address = "";
            Email = "";
            Phone = "";
            Remark = "";
            BuyerId = 0;
            Payment = Payment.未選擇;
            Pickup = Pickup.未選擇;
            Status = OrderStatus.未付款;
            BuyerStar = -1;
            BuyerReviews = "";
            SellerStar = -1;
            SellerReviews = "";
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "賣家Id")]
        public int SellerId { get; set; }

        [ForeignKey("SellerId")]
        public virtual User Seller { get; set; }

        [Display(Name = "買家Id")]
        public int BuyerId { get; set; }

        [ForeignKey("BuyerId")]
        public virtual User Buyer { get; set; }

        [Display(Name = "房間Id")]
        public int RoomId { get; set; }

        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; }

        [Display(Name = "收貨人")]
        public string Name { get; set; }

        [Display(Name = "收貨地址")]
        public string Address { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "收貨人信箱")]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "收貨人電話")]
        public string Phone { get; set; }

        [Display(Name = "付款方式")]
        public Payment Payment { get; set; }

        [Display(Name = "取貨方式")]
        public Pickup Pickup { get; set; }

        [Display(Name = "訂單狀態")]
        public OrderStatus Status { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "訂單總額")]
        public decimal TotalPrice { get; set; }

        [Display(Name = "備註")]
        public string Remark { get; set; }

        [Display(Name = "買方評價分數")]
        public int BuyerStar { get; set; }

        [Display(Name = "買方評價內容")]
        public string BuyerReviews { get; set; }

        [Display(Name = "賣方評價分數")]
        public int SellerStar { get; set; }

        [Display(Name = "賣方評價內容")]
        public string SellerReviews { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}