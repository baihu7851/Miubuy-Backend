using Common;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using Api.Models;
using Api.Utils;

namespace Api.Controllers
{
    [EnableCors("*", "*", "*")]
    public class UtilsController : ApiController
    {
        private readonly Model _db = new Model();

        // POST: api/CheckUser
        [HttpPost]
        [Route("api/CheckUser")]
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser([FromBody] CheckUser account)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (_db.Users.FirstOrDefault(user => user.Account == account.Account) == null) return Ok("此帳號可以使用");
            return BadRequest("帳號已存在");
        }

        // POST: api/Login
        [HttpPost]
        [Route("api/Login")]
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser([FromBody] Login login)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var user = _db.Users.FirstOrDefault(x => x.Account == login.Account);
            if (user == null) return Ok(false);
            var password = Salt.GenerateHashWithSalt(login.Password, user.PasswordSalt);
            var user2 = _db.Users.FirstOrDefault(x => x.Account == login.Account && x.Password == password);
            if (user2 == null) return Ok(false);
            var jwtAuth = new JwtToken();
            var token = jwtAuth.GenerateToken(user2.Id);
            return Ok(new
            {
                user2.Id,
                user2.Nickname,
                user2.Picture,
                token
            });
        }

        // POST: api/UpLoadFile
        [HttpPost]
        [Route("api/UpLoadFile")]
        public IHttpActionResult UpLoadFile()
        {
            //獲取引數資訊
            var request = HttpContext.Current.Request;
            if (request.Files.Count <= 0) return NotFound();
            var fileType = request.Files[0]?.FileName.Substring(request.Files[0].FileName.LastIndexOf('.') + 1);
            //if (fileType.IndexOf("jpg", StringComparison.Ordinal) == -1) return BadRequest("檔案型態錯誤");
            var newImgName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "." + fileType;
            var path = HttpContext.Current.Server.MapPath("/") + "Img/";
            var imgPath = path + newImgName;
            var directoryInfo = new DirectoryInfo(path);
            if (!directoryInfo.Exists) directoryInfo.Create();
            try
            {
                request.Files[0]?.SaveAs(imgPath);
                return Ok(newImgName);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST: api/SelectRooms
        [HttpPost]
        [Route("api/SelectRooms")]
        [ResponseType(typeof(Room))]
        public IHttpActionResult SelectRooms([FromBody] Room select)
        {
            var rooms = _db.Rooms.Where(x => x.RoomClose == false);
            if (select.CountryId > 0)
            {
                rooms = rooms.Where(room => room.CountryId == select.CountryId);
            }
            if (select.CountyId > 0)
            {
                rooms = rooms.Where(room => room.CountyId == select.CountyId);
            }
            if (select.CityId > 0)
            {
                rooms = rooms.Where(room => room.CityId == select.CityId);
            }
            if (select.TagId > 0)
            {
                rooms = rooms.Where(room => room.TagId == select.TagId);
            }

            rooms = rooms.OrderByDescending(room => room.Id);
            var roomsList = rooms.ToList();
            return Ok(roomsList.Select(room => new
            {
                room.Id,
                room.CountyId,
                CountyName = room.County.Name,
                room.CityId,
                CityName = room.City.Name,
                room.TagId,
                TagName = room.Tag.Name,
                Seller = _db.Users.Where(user => user.Id == room.SellerId).Select(user => new
                {
                    user.Id,
                    user.Nickname,
                    user.Picture
                }),
                room.Name,
                room.Picture,
                room.Rule,
                room.TagText,
                room.MaxUsers,
                JoinRoom = room.MaxUsers - room.RoomUsers.Count,
                room.Star,
                room.R18,
                roomStart = room.RoomStart.ToString("yyyy/MM/dd HH:mm"),
                roomEnd = room.RoomEnd.ToString("yyyy/MM/dd HH:mm"),
                users = room.RoomUsers.Select(user => new
                {
                    user.User.Id,
                    user.User.Nickname,
                    user.User.Picture
                }),
            }));
        }

        // DELETE: api/RoomTimeEnd
        [HttpDelete]
        [Route("api/RoomTimeEnd")]
        [ResponseType(typeof(Order))]
        public IHttpActionResult RoomTimeEnd(int id)
        {
            var room = _db.Rooms.Find(id);
            if (room == null) return NotFound();
            room.RoomClose = true;
            _db.Entry(room).State = EntityState.Modified;
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

        // GET: api/JoinedRoom
        [HttpGet]
        [Route("api/JoinedRoom")]
        [ResponseType(typeof(RoomUser))]
        public IHttpActionResult GetRoomUser()
        {
            var tokenId = JwtAuth.GetTokenId(Request.Headers.Authorization.Parameter);
            var roomUsers = _db.Rooms.Where(room => room.SellerId == tokenId);
            return Ok(roomUsers.Select(room => new
            {
                room.Id,
            }));
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