using Common;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using Api.Models;
using Api.Utils;

namespace Api.Controllers
{
    [EnableCors("*", "*", "*")]
    public class RoomUsersController : ApiController
    {
        private readonly Model _db = new Model();

        // GET: api/RoomUsers
        [HttpGet]
        [JwtAuth]
        public IHttpActionResult GetRoomUser()
        {
            var permission = JwtAuth.GetTokenPermission(Request.Headers.Authorization.Parameter);
            if ((permission & 128) <= 0) return BadRequest("權限不足");
            var roomUsers = _db.RoomUsers;
            return Ok(roomUsers.Select(user => new
            {
                user.User.Id,
                user.User.Nickname,
                user.User.Picture,
            }));
        }

        // GET: api/RoomUsers/5
        [HttpGet]
        [ResponseType(typeof(RoomUser))]
        public IHttpActionResult GetRoomUser(int id)
        {
            var permission = JwtAuth.GetTokenPermission(Request.Headers.Authorization.Parameter);
            if ((permission & 128) <= 0) return BadRequest("權限不足");
            var roomUsers = _db.RoomUsers.Where(room => room.RoomId == id);
            return Ok(roomUsers.Select(user => new
            {
                user.User.Id,
                user.User.Nickname,
                user.User.Picture,
            }));
        }

        // POST: api/RoomUsers
        [HttpPost]
        [JwtAuth]
        [ResponseType(typeof(RoomUser))]
        public IHttpActionResult PostRoomUser([FromBody] Room room)
        {
            var permission = JwtAuth.GetTokenPermission(Request.Headers.Authorization.Parameter);
            if ((permission & 2) <= 0) return BadRequest("權限不足");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var roomData = _db.Rooms.Find(room.Id);
            if (roomData != null && roomData.RoomClose) return BadRequest("找不到房間");
            var roomUsers = _db.RoomUsers.Where(x => x.RoomId == room.Id);
            var tokenId = JwtAuth.GetTokenId(Request.Headers.Authorization.Parameter);
            if (roomData.SellerId == tokenId)
            {
                // 房主直接進入
                return Ok(roomUsers.Select(user => new
                {
                    user.RoomId,
                    user.Room.Name,
                    UserId = user.User.Id,
                    UserName = user.User.Name,
                    UserPicture = user.User.Picture,
                    user.Status
                }));
            }
            var joinedUser = roomUsers.Where(user => user.User.Id == tokenId);
            if (joinedUser.Any())
            {
                // 房客直接進入
                return Ok(joinedUser.Select(user => new
                {
                    user.RoomId,
                    user.Room.Name,
                    UserId = user.User.Id,
                    UserName = user.User.Name,
                    UserPicture = user.User.Picture,
                    user.Status
                }));
            }
            if (roomData.MaxUsers < roomUsers.Count()) return BadRequest("人數已經滿");
            // 不在房間內則進入
            var newUser = new RoomUser
            {
                RoomId = room.Id,
                UserId = tokenId,
                Status = UserStatus.無訂單
            };
            _db.RoomUsers.Add(newUser);
            try
            {
                _db.SaveChanges();
                return Ok(roomUsers.Select(user => new
                {
                    user.RoomId,
                    user.Room.Name,
                    UserId = user.User.Id,
                    UserName = user.User.Name,
                    UserPicture = user.User.Picture,
                    user.Status
                }));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/RoomUsers/5
        [ResponseType(typeof(RoomUser))]
        public IHttpActionResult DeleteRoomUser(int id)
        {
            var permission = JwtAuth.GetTokenPermission(Request.Headers.Authorization.Parameter);
            if ((permission & 2) <= 0) return BadRequest("權限不足");
            var tokenId = JwtAuth.GetTokenId(Request.Headers.Authorization.Parameter);
            var roomUser = _db.RoomUsers.FirstOrDefault(x => x.RoomId == id && x.UserId == tokenId);
            var delUser = _db.RoomUsers.Find(roomUser.Id);
            _db.RoomUsers.Remove(delUser);
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