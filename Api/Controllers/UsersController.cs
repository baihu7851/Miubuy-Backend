using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using Jose;
using Miubuy.Models;
using Miubuy.Utils;

namespace Miubuy.Controllers
{
    [EnableCors("*", "*", "*")]
    public class UsersController : ApiController
    {
        private const string Key = "miumiu";
        private readonly Model _db = new Model();

        // GET: api/Users
        [HttpGet]
        [JwtAuth]
        public IHttpActionResult GetUsers()
        {
            var permission = JwtAuth.GetTokenPermission(Request.Headers.Authorization.Parameter);
            if ((permission & 128) <= 0) return BadRequest("權限不足");
            return Ok(_db.Users.Select(user => new
            {
                user.Id,
                user.Account,
                user.Nickname,
                user.Name,
                user.Picture,
                user.Email,
                user.Phone,
                user.Birthday,
                user.Permission,
                user.BuyerAverageStar,
                user.SellerAverageStar
            }));
        }

        // GET: api/Users/5
        [HttpGet]
        [JwtAuth]
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(int id)
        {
            var permission = JwtAuth.GetTokenPermission(Request.Headers.Authorization.Parameter);
            var tokenId = JwtAuth.GetTokenId(Request.Headers.Authorization.Parameter);
            if ((permission & 1) <= 0) return BadRequest("權限不足");
            if (tokenId != id) return BadRequest("使用者錯誤");
            var user = _db.Users.Find(id);
            return Ok(new
            {
                user.Id,
                user.Account,
                user.Nickname,
                user.Name,
                user.Picture,
                user.Email,
                user.Phone,
                user.Birthday,
                user.BuyerAverageStar,
                user.SellerAverageStar
            });
        }

        // PUT: api/Users/5
        [HttpPut]
        [JwtAuth]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(int id, [FromBody] User newUser)
        {
            var permission = JwtAuth.GetTokenPermission(Request.Headers.Authorization.Parameter);
            var tokenId = JwtAuth.GetTokenId(Request.Headers.Authorization.Parameter);
            if ((permission & 1) <= 0) return BadRequest("權限不足");
            if (tokenId != id) return BadRequest("使用者錯誤");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var user = _db.Users.Find(id);
            user.PasswordSalt = Salt.CreateSalt();
            user.Password = Salt.GenerateHashWithSalt(newUser.Password, user.PasswordSalt);
            user.Nickname = newUser.Nickname ?? user.Nickname;
            user.Name = newUser.Name ?? user.Name;
            user.Picture = newUser.Picture ?? user.Picture;
            user.Email = newUser.Email ?? user.Email;
            user.Phone = newUser.Phone ?? user.Phone;
            user.Birthday = user.Birthday;
            _db.Entry(user).State = EntityState.Modified;
            try
            {
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok(id);
        }

        // POST: api/Users
        [HttpPost]
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser([FromBody] User user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (_db.Users.FirstOrDefault(data => data.Account == user.Account) != null) return BadRequest("帳號已存在");
            user.PasswordSalt = Salt.CreateSalt();
            user.Password = Salt.GenerateHashWithSalt(user.Password, user.PasswordSalt);
            user.Permission = 127;
            _db.Users.Add(user);
            try
            {
                _db.SaveChanges();
                return Ok(user.Id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/Users/5
        [HttpDelete]
        [JwtAuth]
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(int id)
        {
            var permission = JwtAuth.GetTokenPermission(Request.Headers.Authorization.Parameter);
            if ((permission & 128) <= 0) return BadRequest("權限不足");
            var userData = _db.Users.Find(id);
            if (userData == null) return NotFound();
            userData.Permission = 0;
            Sql.UpData(userData.Permission);
            try
            {
                _db.SaveChanges();
                return Ok(id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}