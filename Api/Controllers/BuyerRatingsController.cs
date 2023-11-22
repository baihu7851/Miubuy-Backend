using Api.Models;
using Api.Utils;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace Api.Controllers
{
    [EnableCors("*", "*", "*")]
    public class BuyerRatingsController : ApiController
    {
        private readonly Model _db = new Model();

        // GET: api/BuyerRatings/
        [HttpGet]
        [JwtAuth]
        [ResponseType(typeof(Ratings))]
        public IHttpActionResult GetBuyerRatings()
        {
            var permission = JwtAuth.GetTokenPermission(Request.Headers.Authorization.Parameter);
            if ((permission & 1) <= 0) return BadRequest("權限不足");
            var tokenId = JwtAuth.GetTokenId(Request.Headers.Authorization.Parameter);
            var orders = _db.Orders.Where(order => order.BuyerId == tokenId).ToList();

            return Ok(orders.OrderByDescending(order => order.Id).Select(order => new
            {
                order.Id,
                order.Name,
                order.Address,
                order.Email,
                order.Phone,
                Payment = order.Payment.ToString(),
                Pickup = order.Pickup.ToString(),
                Status = order.Status.ToString(),
                order.TotalPrice,
                order.Remark,
                order.RoomId,
                RoomName = order.Room.Name,
                RoomPicture = order.Room.Picture,
                Detail = order.OrderDetails.Select(detail => new
                {
                    detail.Id,
                    detail.Name,
                    detail.Price
                }).ToList(),
                order.SellerId,
                SellerNickname = order.Seller.Nickname,
                SellerAccount = order.Seller.Account,
                SellerPicture = order.Seller.Picture,
                SellerStar = Star(order.SellerStar),
                order.SellerReviews,
                BuyerStar = Star(order.BuyerStar),
                order.BuyerReviews,
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

        private static string Star(int starNumber)
        {
            if (starNumber == -1) return "☆☆☆☆☆";
            var star = "";
            for (var i = 0; i < 5; i++)
            {
                if (starNumber > i)
                {
                    star += "★";
                }
                else
                {
                    star += "☆";
                }
            }
            return star;
        }
    }
}