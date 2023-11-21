using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using Miubuy.Hubs;
using Miubuy.Models;
using Miubuy.Utils;

namespace Miubuy.Controllers
{
    [EnableCors("*", "*", "*")]
    public class OrdersController : ApiController
    {
        private readonly Model _db = new Model();

        // GET: api/Orders
        [HttpGet]
        [JwtAuth]
        public IHttpActionResult GetOrders()
        {
            var permission = JwtAuth.GetTokenPermission(Request.Headers.Authorization.Parameter);
            if ((permission & 128) <= 0) return BadRequest("權限不足");
            var orders = _db.Orders.ToList();
            return Ok(orders.Select(order => new
            {
                order.Id,
                order.Name,
                order.Address,
                order.Email,
                order.Phone,
                order.Payment,
                order.Pickup,
                Status = order.Status.ToString(),
                order.TotalPrice,
                order.Remark,
                RoomName = order.Room.Name.FirstOrDefault(),
                RoomPicture = order.Room.Picture.FirstOrDefault(),
                Detail = order.OrderDetails.Select(detail => new
                {
                    detail.Id,
                    detail.Name,
                    detail.Price
                }),
                order.BuyerStar,
                order.BuyerReviews,
                order.SellerStar,
                order.SellerReviews,
            }));
        }

        // GET: api/Orders/5
        [HttpGet]
        [JwtAuth]
        [ResponseType(typeof(Order))]
        public IHttpActionResult GetOrder(int id)
        {
            var permission = JwtAuth.GetTokenPermission(Request.Headers.Authorization.Parameter);
            if ((permission & 2) <= 0) return BadRequest("權限不足");
            var order = _db.Orders.Find(id);
            return Ok(new
            {
                order.Id,
                order.Name,
                order.Address,
                order.Email,
                order.Phone,
                order.Payment,
                order.Pickup,
                order.Status,
                order.TotalPrice,
                order.Remark,
                RoomName = order.Room.Name.FirstOrDefault(),
                RoomPicture = order.Room.Picture.FirstOrDefault(),
                Detail = order.OrderDetails.Select(detail => new
                {
                    detail.Id,
                    detail.Name,
                    detail.Price
                }).ToList(),
            });
        }

        // PUT: api/Orders/5
        [HttpPut]
        [JwtAuth]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrder(int id, [FromBody] Order order)
        {
            var permission = JwtAuth.GetTokenPermission(Request.Headers.Authorization.Parameter);
            if ((permission & 2) <= 0) return BadRequest("權限不足");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var orderData = _db.Orders.Find(id);
            if (orderData == null) return NotFound();
            orderData.Name = string.IsNullOrEmpty(order.Name) ? orderData.Name : order.Name;
            orderData.Address = string.IsNullOrEmpty(order.Address) ? orderData.Address : order.Address;
            orderData.Email = string.IsNullOrEmpty(order.Email) ? orderData.Email : order.Email;
            orderData.Phone = string.IsNullOrEmpty(order.Phone) ? orderData.Phone : order.Phone;
            orderData.Payment = order.Payment == Payment.未選擇 ? orderData.Payment : order.Payment;
            orderData.Pickup = order.Pickup == Pickup.未選擇 ? orderData.Pickup : order.Pickup;
            orderData.Status = order.Status == OrderStatus.未選擇 ? orderData.Status : order.Status;
            orderData.Remark = string.IsNullOrEmpty(order.Remark) ? orderData.Remark : order.Remark;
            _db.Entry(orderData).State = EntityState.Modified;
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

        // POST: api/Orders
        [HttpPost]
        [JwtAuth]
        [ResponseType(typeof(Order))]
        public IHttpActionResult PostOrder(InputId inputId)
        {
            var permission = JwtAuth.GetTokenPermission(Request.Headers.Authorization.Parameter);
            if ((permission & 2) <= 0) return BadRequest("權限不足");
            var order = new Order
            {
                TotalPrice = 0,
                SellerId = JwtAuth.GetTokenId(Request.Headers.Authorization.Parameter),
                BuyerId = inputId.BuyerId,
                RoomId = inputId.RoomId
            };
            var tempDetails = _db.TempDetails.Where(x => x.BuyerId == inputId.BuyerId && x.RoomId == inputId.RoomId);
            foreach (var detail in tempDetails)
            {
                var newDerail = new OrderDetail
                {
                    Name = detail.Name,
                    Price = detail.Price,
                    OrderId = order.Id
                };
                order.TotalPrice += detail.Price;
                _db.OrderDetails.Add(newDerail);
                //產生產品明細
            }
            //產生訂單表
            _db.Orders.Add(order);
            try
            {
                _db.SaveChanges();
                return Ok(order.Id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/Orders/5
        [HttpDelete]
        [JwtAuth]
        [ResponseType(typeof(Order))]
        public IHttpActionResult DeleteOrder(int id)
        {
            var permission = JwtAuth.GetTokenPermission(Request.Headers.Authorization.Parameter);
            if ((permission & 2) <= 0) return BadRequest("權限不足");
            var order = _db.Orders.Find(id);
            if (order == null) return NotFound();
            order.Status = OrderStatus.訂單取消;
            Sql.UpData(order.Status);
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

    public class InputId
    {
        public int BuyerId { get; set; }
        public int RoomId { get; set; }
    }
}