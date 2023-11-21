using System;
using System.Collections.Generic;

namespace Miubuy.Models.UserApiModel
{
    public class UsersViewModel
    {
        public List<User> Users { get; set; } = new List<User>();

        public class User
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
            public int Permission { get; set; }
        }
    }
}