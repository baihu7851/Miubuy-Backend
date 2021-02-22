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
    public class TagsController : ApiController
    {
        private readonly Model _db = new Model();

        // GET: api/Tags
        [HttpGet]
        public IHttpActionResult GetTag()
        {
            return Ok(_db.Tags.Where(tag => tag.Delete == false).Select(tag => new
            {
                tag.Id,
                tag.Name,
                tag.Color,
                tag.Rooms.Count
            }));
        }

        // GET: api/Tags/5
        [HttpGet]
        [ResponseType(typeof(Tag))]
        public IHttpActionResult GetTag(int id)
        {
            var tags = _db.Tags.Where(x => x.Id == id);
            return Ok(tags.Where(tag => tag.Delete == false).Select(tag => new
            {
                tag.Id,
                tag.Name,
                tag.Color,
                tag.Rooms.Count
            }));
        }

        // PUT: api/Tags/5
        [HttpPut]
        [JwtAuth]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTag(int id, [FromBody] Tag newTag)
        {
            var token = JwtAuth.GetToken(Request.Headers.Authorization.Parameter);
            if ((Convert.ToInt32(token["Permission"]) & 128) <= 0) return BadRequest("權限不足");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var tag = _db.Tags.Find(id);
            if (tag == null) return NotFound();
            tag.Name = newTag.Name ?? tag.Name;
            tag.Color = newTag.Color ?? tag.Color;
            _db.Entry(tag).State = EntityState.Modified;
            try
            {
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok(id);
        }

        // POST: api/Tags
        [HttpPost]
        [JwtAuth]
        [ResponseType(typeof(Tag))]
        public IHttpActionResult PostTag([FromBody] Tag tag)
        {
            var token = JwtAuth.GetToken(Request.Headers.Authorization.Parameter);
            if ((Convert.ToInt32(token["Permission"]) & 128) <= 0) return BadRequest("權限不足");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _db.Tags.Add(tag);
            try
            {
                _db.SaveChanges();
                return Ok("新增成功");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/Tags/5
        [HttpDelete]
        [JwtAuth]
        [ResponseType(typeof(Tag))]
        public IHttpActionResult DeleteTag(int id)
        {
            var token = JwtAuth.GetToken(Request.Headers.Authorization.Parameter);
            if ((Convert.ToInt32(token["Permission"]) & 128) <= 0) return BadRequest("權限不足");
            var tag = _db.Tags.Find(id);
            if (tag == null) return NotFound();
            tag.Delete = true;
            Sql.UpData(tag.Delete);
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