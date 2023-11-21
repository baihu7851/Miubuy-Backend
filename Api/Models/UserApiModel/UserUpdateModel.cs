﻿using System;

namespace Miubuy.Models.UserApiModel
{
    public class UserUpdateModel
    {
        public string Password { get; set; }
        public string Nickname { get; set; }
        public string Picture { get; set; }
        public string Name { get; set; }
        public DateTime? Birthday { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}