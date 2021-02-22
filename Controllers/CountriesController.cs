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
    public class CountriesController : ApiController
    {
        private readonly Model _db = new Model();

        // GET: api/Countries
        [HttpGet]
        public IHttpActionResult GetCountries()
        {
            return Ok(_db.Countries.Where(country => country.Delete == false).Select(country => new
            {
                country.Id,
                country.Name,
                Count = country.Rooms.Select(x => x.RoomClose == false).Count()
            }));
        }

        // GET: api/Countries/5
        [HttpGet]
        [ResponseType(typeof(Country))]
        public IHttpActionResult GetCountry(int id)
        {
            var countries = _db.Countries.Where(country => country.Id == id);
            return Ok(countries.Where(country => country.Delete == false).Select(country => new
            {
                country.Id,
                country.Name,
                Count = country.Rooms.Select(x => x.RoomClose == false).Count()
            }));
        }

        // PUT: api/Countries/5
        [HttpPut]
        [JwtAuth]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCountry(int id, [FromBody] Country newCountry)
        {
            var permission = JwtAuth.GetTokenPermission(Request.Headers.Authorization.Parameter);
            if ((permission & 128) <= 0) return BadRequest("權限不足");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var country = _db.Countries.Find(id);
            if (country == null) return NotFound();
            country.Name = newCountry.Name ?? country.Name;
            _db.Entry(country).State = EntityState.Modified;
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

        // POST: api/Countries
        [HttpPost]
        [JwtAuth]
        [ResponseType(typeof(Country))]
        public IHttpActionResult PostCountry([FromBody] Country country)
        {
            var permission = JwtAuth.GetTokenPermission(Request.Headers.Authorization.Parameter);
            if ((permission & 128) <= 0) return BadRequest("權限不足");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _db.Countries.Add(country);
            try
            {
                _db.SaveChanges();
                return Ok(country.Id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/Countries/5
        [HttpDelete]
        [JwtAuth]
        [ResponseType(typeof(Country))]
        public IHttpActionResult DeleteCountry(int id)
        {
            var permission = JwtAuth.GetTokenPermission(Request.Headers.Authorization.Parameter);
            if ((permission & 128) <= 0) return BadRequest("權限不足");
            var country = _db.Countries.Find(id);
            if (country == null) return NotFound();
            country.Delete = true;
            Sql.UpData(country.Delete);
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