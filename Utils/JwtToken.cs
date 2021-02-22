using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Jose;
using Miubuy.Models;

namespace Miubuy.Utils
{
    public class JwtToken
    {
        private const string Key = "miumiu";
        private readonly Model _db = new Model();

        public string GenerateToken(int id)
        {
            var user = _db.Users.Find(id);
            var payload = new Dictionary<string, object>
            {
                {"Id", user.Id},
                {"Permission", user.Permission},
                {"Exp", DateTime.Now.AddMonths(3)} //Token 時效設定1天
            };
            //payload 需透過token傳遞的資料
            var token = JWT.Encode(payload, Encoding.UTF8.GetBytes(Key), JwsAlgorithm.HS512);//產生token
            return token;
        }
    }
}