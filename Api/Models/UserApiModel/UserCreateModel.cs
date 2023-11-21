using System;

namespace Miubuy.Models.UserApiModel
{
    public class UserCreateModel
    {
        public string Account { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string Nickname { get; set; }
        public string Picture { get; set; }
        public string Name { get; set; }
        public DateTime? Birthday { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}