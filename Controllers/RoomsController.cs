using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using Miubuy.Models;
using Miubuy.Utils;

namespace Miubuy.Controllers
{
    [EnableCors("*", "*", "*")]
    public class RoomsController : ApiController
    {
        private readonly Model _db = new Model();

        // GET: api/Rooms
        [HttpGet]
        public IHttpActionResult GetRooms()
        {
            var rooms = _db.Rooms.Where(x => x.RoomClose == false).ToList();
            return Ok(rooms.Select(room => new
            {
                room.Id,
                room.CountryId,
                CountryName = room.Country.Name,
                room.CountyId,
                CountyName = room.County.Name,
                room.CityId,
                CityName = room.City.Name,
                room.TagId,
                TagName = room.Tag.Name,
                sel = room.SellerId,
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

        // GET: api/Rooms/5
        [HttpGet]
        [ResponseType(typeof(Room))]
        public IHttpActionResult GetRoom(int id)
        {
            var room = _db.Rooms.Find(id);
            if (room == null) return NotFound();
            return Ok(new
            {
                room.Id,
                room.CountryId,
                CountryName = room.Country.Name,
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
            });
        }

        // PUT: api/Rooms/5
        [HttpPut]
        [JwtAuth]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRoom(int id, [FromBody] Room newRoom)
        {
            var permission = JwtAuth.GetTokenPermission(Request.Headers.Authorization.Parameter);
            if ((permission & 4) <= 0) return BadRequest("權限不足");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var room = _db.Rooms.Find(id);
            if (room == null) return NotFound();
            var tokenId = JwtAuth.GetTokenId(Request.Headers.Authorization.Parameter);
            if (tokenId != room.SellerId) return BadRequest("使用者錯誤");
            room.CountryId = newRoom.CountryId == 0 ? room.CountryId : newRoom.CountryId;
            room.CountyId = newRoom.CountyId == 0 ? room.CountyId : newRoom.CountyId;
            room.CityId = newRoom.CityId == 0 ? room.CityId : newRoom.CityId;
            room.TagId = newRoom.TagId == 0 ? room.TagId : newRoom.TagId;
            room.Name = newRoom.Name ?? room.Name;
            room.Picture = newRoom.Picture ?? room.Picture;
            room.Rule = newRoom.Rule ?? room.Rule;
            room.TagText = newRoom.TagText ?? room.TagText;
            room.MaxUsers = newRoom.MaxUsers == 0 ? room.MaxUsers : newRoom.MaxUsers;
            room.Star = newRoom.Star == 0 ? room.Star : newRoom.Star;
            room.R18 = newRoom.R18;
            room.RoomClose = newRoom.RoomClose;
            room.RoomEnd = DateTime.Now.AddHours(1);
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

        // POST: api/Rooms
        [HttpPost]
        [JwtAuth]
        [ResponseType(typeof(Room))]
        public IHttpActionResult PostRoom([FromBody] Room room)
        {
            var permission = JwtAuth.GetTokenPermission(Request.Headers.Authorization.Parameter);
            if ((permission & 4) <= 0) return BadRequest("權限不足");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            room.RoomStart = DateTime.Now;
            room.RoomEnd = DateTime.Now.AddHours(1);
            if (room.RoomStart > DateTime.Now && room.RoomEnd <= DateTime.Now) room.RoomClose = true;
            room.SellerId = JwtAuth.GetTokenId(Request.Headers.Authorization.Parameter);
            if (room.CountryId == 0) room.CountryId = 1;
            if (room.CountyId == 0) room.CountyId = 1;
            if (room.CityId == 0) room.CityId = 1;
            if (room.TagId == 0) room.TagId = 1;
            _db.Rooms.Add(room);
            try
            {
                _db.SaveChanges();
                return Ok(room.Id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/Rooms/5
        [HttpDelete]
        [JwtAuth]
        [ResponseType(typeof(Room))]
        public IHttpActionResult DeleteRoom(int id)
        {
            var permission = JwtAuth.GetTokenPermission(Request.Headers.Authorization.Parameter);
            if ((permission & 4) <= 0) return BadRequest("權限不足");
            var room = _db.Rooms.Find(id);
            if (room == null) return NotFound();
            var tokenId = JwtAuth.GetTokenId(Request.Headers.Authorization.Parameter);
            if (tokenId != room.SellerId) return BadRequest("使用者錯誤");
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
            //var roomUser = room.RoomUsers.FirstOrDefault(user => user.RoomId == id);
            //if (roomUser != null) return BadRequest("房間還有訪客");
            //_db.Rooms.Remove(room);
            //try
            //{
            //    _db.SaveChanges();
            //    return Ok(id);
            //}
            //catch (Exception e)
            //{
            //    return BadRequest(e.Message);
            //}
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