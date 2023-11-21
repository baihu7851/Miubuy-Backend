using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using System.Web.Optimization;
using Miubuy.Models;
using Miubuy.Utils;

namespace Miubuy.Controllers
{
    [EnableCors("*", "*", "*")]
    public class SellerRatingsController : ApiController
    {
        private readonly Model _db = new Model();

        // GET: api/SellerRatings
        [HttpGet]
        [JwtAuth]
        [ResponseType(typeof(Ratings))]
        public IHttpActionResult GetSellerRatings()
        {
            var permission = JwtAuth.GetTokenPermission(Request.Headers.Authorization.Parameter);
            if ((permission & 1) <= 0) return BadRequest("權限不足");
            var tokenId = JwtAuth.GetTokenId(Request.Headers.Authorization.Parameter);
            var orders = _db.Orders.Where(order => order.SellerId == tokenId).ToList();
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
                order.BuyerId,
                BuyerNickname = order.Buyer.Nickname,
                BuyerAccount = order.Buyer.Account,
                BuyerPicture = order.Buyer.Picture,
                SellerStar = Star(order.SellerStar),
                order.BuyerReviews,
                BuyerStar = Star(order.BuyerStar),
                order.SellerReviews,
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