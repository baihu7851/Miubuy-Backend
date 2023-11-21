using System.Web.Http;
using System.Web.Http.Cors;
using Miubuy.Models.UserApiModel;
using Miubuy.Utils;
using Services;
using Services.Models;

namespace Miubuy.Controllers
{
    [EnableCors("*", "*", "*")]
    public class UsersController : ApiController
    {
        private readonly UserService _userService;

        public UsersController()
        {
            _userService = new UserService();
        }

        ~UsersController()
        {
            _userService.Dispose();
            Dispose();
        }

        // GET: api/Users
        [HttpGet]
        [JwtAuth]
        public IHttpActionResult GetUsers()
        {
            var permission = JwtAuth.GetTokenPermission(Request.Headers.Authorization.Parameter);
            if ((permission & 128) <= 0) return BadRequest("權限不足");
            var servicesResult = _userService.GetUsers();
            if (!servicesResult.IsSusses)
            {
                BadRequest(servicesResult.Message);
            }
            var viewModel = new UsersViewModel();
            foreach (var userModel in servicesResult.Data)
            {
                var user = new UsersViewModel.User
                {
                    Id = userModel.Id,
                    Account = userModel.Account,
                    Nickname = userModel.Nickname,
                    Name = userModel.Name,
                    Picture = userModel.Picture,
                    Email = userModel.Email,
                    Phone = userModel.Phone,
                    Birthday = userModel.Birthday,
                    Permission = userModel.Permission,
                    BuyerAverageStar = userModel.BuyerAverageStar,
                    SellerAverageStar = userModel.SellerAverageStar
                };
                viewModel.Users.Add(user);
            }

            return Ok(viewModel.Users);
        }

        // GET: api/Users/5
        [HttpGet]
        [JwtAuth]
        public IHttpActionResult GetUser(int id)
        {
            var permission = JwtAuth.GetTokenPermission(Request.Headers.Authorization.Parameter);
            var tokenId = JwtAuth.GetTokenId(Request.Headers.Authorization.Parameter);
            if ((permission & 1) <= 0) return BadRequest("權限不足");
            if (tokenId != id) return BadRequest("使用者錯誤");
            var servicesResult = _userService.GetUser(id);
            if (!servicesResult.IsSusses)
            {
                BadRequest(servicesResult.Message);
            }

            var viewModel = new UserViewModel()
            {
                Id = servicesResult.Data.Id,
                Account = servicesResult.Data.Account,
                Nickname = servicesResult.Data.Nickname,
                Name = servicesResult.Data.Name,
                Picture = servicesResult.Data.Picture,
                Email = servicesResult.Data.Email,
                Phone = servicesResult.Data.Phone,
                Birthday = servicesResult.Data.Birthday,
                BuyerAverageStar = servicesResult.Data.BuyerAverageStar,
                SellerAverageStar = servicesResult.Data.SellerAverageStar
            };

            return Ok(viewModel);
        }

        // PUT: api/Users/5
        [HttpPut]
        [JwtAuth]
        public IHttpActionResult PutUser(int id, [FromBody] UserUpdateModel newUser)
        {
            var permission = JwtAuth.GetTokenPermission(Request.Headers.Authorization.Parameter);
            var tokenId = JwtAuth.GetTokenId(Request.Headers.Authorization.Parameter);
            if ((permission & 1) <= 0) return BadRequest("權限不足");
            if (tokenId != id) return BadRequest("使用者錯誤");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var userModel = new UserModel
            {
                Id = newUser.Id,
                Password = newUser.Password,
                Nickname = newUser.Nickname,
                Picture = newUser.Picture,
                Name = newUser.Name,
                Birthday = newUser.Birthday,
                Email = newUser.Email,
                Phone = newUser.Phone
            };
            var servicesResult = _userService.UpdateUser(userModel);
            if (!servicesResult.IsSusses)
            {
                BadRequest(servicesResult.Message);
            }
            return Ok(id);
        }

        // POST: api/Users
        [HttpPost]
        public IHttpActionResult PostUser([FromBody] UserCreateModel user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var userModel = new UserModel
            {
                Account = user.Account,
                Password = user.Password,
                PasswordSalt = user.PasswordSalt,
                Nickname = user.Nickname,
                Picture = user.Picture,
                Name = user.Name,
                Birthday = user.Birthday,
                Email = user.Email,
                Phone = user.Phone
            };
            var servicesResult = _userService.CreateUser(userModel);
            if (!servicesResult.IsSusses)
            {
                BadRequest(servicesResult.Message);
            }
            var userResult = _userService.GetUser(user.Account);
            if (!servicesResult.IsSusses)
            {
                BadRequest(servicesResult.Message);
            }
            return Ok(userResult.Data.Id);
        }

        // DELETE: api/Users/5
        [HttpDelete]
        [JwtAuth]
        public IHttpActionResult DeleteUser(int id)
        {
            var permission = JwtAuth.GetTokenPermission(Request.Headers.Authorization.Parameter);
            if ((permission & 128) <= 0) return BadRequest("權限不足");
            var servicesResult = _userService.DeletePermission(id);
            if (!servicesResult.IsSusses)
            {
                BadRequest(servicesResult.Message);
            }
            return Ok(id);
        }
    }
}