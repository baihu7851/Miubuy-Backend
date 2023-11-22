using Api.Models;
using Api.Utils;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace Api.Controllers
{
    [EnableCors("*", "*", "*")]
    public class CountiesController : ApiController
    {
        private readonly Model _db = new Model();

        // GET: api/Counties
        [HttpGet]
        public IHttpActionResult GetCounties()
        {
            return Ok(_db.Counties.Where(county => county.Delete == false).Select(county => new
            {
                county.Id,
                county.Name,
                Count = county.Rooms.Select(x => x.RoomClose == false).Count()
            }));
        }

        // GET: api/Counties/5
        [HttpGet]
        [ResponseType(typeof(County))]
        public IHttpActionResult GetCounty(int id)
        {
            var counties = _db.Counties.Where(county => county.Delete == false).Where(county => county.CountryId == id);
            return Ok(counties.Select(county => new
            {
                county.Id,
                county.Name,
                Count = county.Rooms.Select(x => x.RoomClose == false).Count()
            }));
        }

        // PUT: api/Counties/5
        [HttpPut]
        [JwtAuth]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCounty(int id, [FromBody] County newCounty)
        {
            var permission = JwtAuth.GetTokenPermission(Request.Headers.Authorization.Parameter);
            if ((permission & 128) <= 0) return BadRequest("權限不足");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var county = _db.Counties.Find(id);
            if (county == null) return NotFound();
            county.Name = newCounty.Name ?? county.Name;
            county.CountryId = county.CountryId;

            _db.Entry(county).State = EntityState.Modified;
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

        // POST: api/Counties
        [HttpPost]
        [JwtAuth]
        [ResponseType(typeof(County))]
        public IHttpActionResult PostCounty([FromBody] County county)
        {
            var permission = JwtAuth.GetTokenPermission(Request.Headers.Authorization.Parameter);
            if ((permission & 128) <= 0) return BadRequest("權限不足");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _db.Counties.Add(county);
            try
            {
                _db.SaveChanges();
                return Ok(county.Id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/Counties/5
        [HttpDelete]
        [JwtAuth]
        [ResponseType(typeof(County))]
        public IHttpActionResult DeleteCounty(int id)
        {
            var permission = JwtAuth.GetTokenPermission(Request.Headers.Authorization.Parameter);
            if ((permission & 128) <= 0) return BadRequest("權限不足");
            var county = _db.Counties.Find(id);
            if (county == null) return NotFound();
            county.Delete = true;
            Sql.UpData(county.Delete);
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