using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using Api.Models;
using Api.Utils;

namespace Api.Controllers
{
    [EnableCors("*", "*", "*")]
    public class RatingsController : ApiController
    {
        private readonly Model _db = new Model();

        // GET: api/Ratings
        [HttpGet]
        [JwtAuth]
        public IHttpActionResult GetRatings()
        {
            var permission = JwtAuth.GetTokenPermission(Request.Headers.Authorization.Parameter);
            if ((permission & 1) <= 0) return BadRequest("權限不足");
            var orders = _db.Orders;
            return Ok(orders.Select(order => new
            {
                order.Id,
                order.BuyerId,
                BuyerNickname = order.Buyer.Nickname,
                BuyerPicture = order.Buyer.Picture,
                order.BuyerStar,
                order.BuyerReviews,
                order.SellerId,
                SellerNickname = order.Seller.Nickname,
                SellerPicture = order.Seller.Picture,
                order.SellerStar,
                order.SellerReviews
            }));
        }

        // GET: api/Ratings/5
        [HttpGet]
        [JwtAuth]
        [ResponseType(typeof(Ratings))]
        public IHttpActionResult GetRatings(int id)
        {
            var permission = JwtAuth.GetTokenPermission(Request.Headers.Authorization.Parameter);
            if ((permission & 1) <= 0) return BadRequest("權限不足");
            var order = _db.Orders.Find(id);
            return Ok(new
            {
                order.Id,
                order.BuyerId,
                BuyerNickname = order.Buyer.Nickname,
                BuyerPicture = order.Buyer.Picture,
                order.BuyerStar,
                order.BuyerReviews,
                order.SellerId,
                SellerNickname = order.Seller.Nickname,
                SellerPicture = order.Seller.Picture,
                order.SellerStar,
                order.SellerReviews
            });
        }

        // PUT: api/Ratings/5
        [HttpPut]
        [JwtAuth]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRatings(int id, Ratings newRating)
        {
            var permission = JwtAuth.GetTokenPermission(Request.Headers.Authorization.Parameter);
            var tokenId = JwtAuth.GetTokenId(Request.Headers.Authorization.Parameter);
            var user = _db.Users.Find(tokenId);
            if ((permission & 16) <= 0) return BadRequest("權限不足");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var order = _db.Orders.Find(id);
            if (order == null) return NotFound();
            if (order.SellerId == tokenId)
            {
                order.BuyerStar = newRating.BuyerStar;
                order.BuyerReviews = newRating.BuyerReviews;
                if (order.Buyer.SellerAverageStar > 0)
                {
                    order.Buyer.SellerAverageStar += newRating.SellerStar;
                    order.Buyer.SellerAverageStar /= 2;
                }
                else
                {
                    order.Buyer.SellerAverageStar = newRating.SellerStar;
                }
            }
            else
            {
                order.SellerStar = newRating.SellerStar;
                order.SellerReviews = newRating.SellerReviews;
                if (order.Seller.BuyerAverageStar > 0)
                {
                    order.Seller.BuyerAverageStar += newRating.BuyerStar;
                    order.Seller.BuyerAverageStar /= 2;
                }
                else
                {
                    order.Seller.BuyerAverageStar = newRating.BuyerStar;
                }
            }
            _db.Entry(user).State = EntityState.Modified;
            _db.Entry(order).State = EntityState.Modified;
            try
            {
                _db.SaveChanges();
                return Ok(new
                {
                    id,
                    order.SellerId,
                    order.BuyerId,
                    tokenId
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/Ratings/5
        [HttpDelete]
        [JwtAuth]
        [ResponseType(typeof(Ratings))]
        public IHttpActionResult DeleteRatings(int id)
        {
            var permission = JwtAuth.GetTokenPermission(Request.Headers.Authorization.Parameter);
            if ((permission & 128) <= 0) return BadRequest("權限不足");
            var order = _db.Orders.Find(id);
            if (order == null) return NotFound();
            order.BuyerStar = 0;
            order.BuyerReviews = "";
            order.SellerStar = 0;
            order.SellerReviews = "";
            _db.Entry(order).State = EntityState.Modified;
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