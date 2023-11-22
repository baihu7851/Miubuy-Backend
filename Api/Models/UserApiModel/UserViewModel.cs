using System;

namespace Api.Models.UserApiModel
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string Nickname { get; set; }
        public string Picture { get; set; }
        public string Name { get; set; }
        public DateTime? Birthday { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public float BuyerAverageStar { get; set; }
        public float SellerAverageStar { get; set; }
    }
}