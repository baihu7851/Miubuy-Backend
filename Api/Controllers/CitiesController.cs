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
    public class CitiesController : ApiController
    {
        private readonly Model _db = new Model();

        // GET: api/Cities
        [HttpGet]
        public IHttpActionResult GetCities()
        {
            return Ok(_db.Cities.Where(city => city.Delete == false).Select(city => new
            {
                city.Id,
                city.Name,
                Count = city.Rooms.Select(x => x.RoomClose == false).Count()
            }));
        }

        // GET: api/Cities/5
        [HttpGet]
        [ResponseType(typeof(City))]
        public IHttpActionResult GetCity(int id)
        {
            var cities = _db.Cities.Where(city => city.CountyId == id);
            return Ok(cities.Where(city => city.Delete == false).Select(city => new
            {
                city.Id,
                city.Name,
                Count = city.Rooms.Select(x => x.RoomClose == false).Count()
            }));
        }

        // PUT: api/Cities/5
        [HttpPut]
        [JwtAuth]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCity(int id, [FromBody] City newCity)
        {
            var permission = JwtAuth.GetTokenPermission(Request.Headers.Authorization.Parameter);
            if ((permission & 128) <= 0) return BadRequest("權限不足");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var city = _db.Cities.Find(id);
            if (city == null) return NotFound();
            city.Name = newCity.Name;
            city.CountyId = newCity.CountyId;
            _db.Entry(city).State = EntityState.Modified;
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

        // POST: api/Cities
        [HttpPost]
        [JwtAuth]
        [ResponseType(typeof(City))]
        public IHttpActionResult PostCity([FromBody] City city)
        {
            var permission = JwtAuth.GetTokenPermission(Request.Headers.Authorization.Parameter);
            if ((permission & 128) <= 0) return BadRequest("權限不足");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _db.Cities.Add(city);
            try
            {
                _db.SaveChanges();
                return Ok(city.Id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/Cities/5
        [HttpDelete]
        [JwtAuth]
        [ResponseType(typeof(City))]
        public IHttpActionResult DeleteCity(int id)
        {
            var permission = JwtAuth.GetTokenPermission(Request.Headers.Authorization.Parameter);
            if ((permission & 128) <= 0) return BadRequest("權限不足");
            var city = _db.Cities.Find(id);
            if (city == null) return NotFound();
            city.Delete = true;
            Sql.UpData(city.Delete);
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