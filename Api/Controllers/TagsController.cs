using Services;
using Services.Models;
using System;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using Api.Models;
using Api.Models.TagApiModel;
using Api.Utils;

namespace Api.Controllers
{
    [EnableCors("*", "*", "*")]
    public class TagsController : ApiController
    {
        private readonly Model _db = new Model();
        private readonly TagService _tagService;

        public TagsController()
        {
            _tagService = new TagService();
        }

        ~TagsController()
        {
            _tagService.Dispose();
            Dispose();
        }

        // GET: api/Tags
        [HttpGet]
        public IHttpActionResult GetTag()
        {
            var servicesResult = _tagService.GetTags();
            if (!servicesResult.IsSusses)
            {
                BadRequest(servicesResult.Message);
            }
            var viewModel = new TagsViewModel();
            foreach (var tagModel in servicesResult.Data)
            {
                var tag = new TagsViewModel.Tag
                {
                    Id = tagModel.Id,
                    Name = tagModel.Name,
                    Color = tagModel.Color,
                    RoomCount = tagModel.RoomCount,
                };
                viewModel.Tags.Add(tag);
            }
            return Ok(viewModel.Tags);
        }

        // GET: api/Tags/5
        [HttpGet]
        [ResponseType(typeof(Tag))]
        public IHttpActionResult GetTag(int id)
        {
            var servicesResult = _tagService.GetTag(id);
            if (!servicesResult.IsSusses)
            {
                BadRequest(servicesResult.Message);
            }
            var viewModel = new TagViewModel
            {
                Id = servicesResult.Data.Id,
                Name = servicesResult.Data.Name,
                Color = servicesResult.Data.Color,
                RoomCount = servicesResult.Data.RoomCount,
            };
            return Ok(viewModel);
        }

        // PUT: api/Tags/5
        [HttpPut]
        [JwtAuth]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTag(int id, [FromBody] TagUpdateModel newTag)
        {
            var token = JwtAuth.GetToken(Request.Headers.Authorization.Parameter);
            if ((Convert.ToInt32(token["Permission"]) & 128) <= 0) return BadRequest("權限不足");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var tagModel = new TagModel
            {
                Id = id,
                Name = newTag.Name,
                Color = newTag.Color
            };
            var servicesResult = _tagService.UpdateTag(tagModel);
            if (!servicesResult.IsSusses)
            {
                BadRequest(servicesResult.Message);
            }
            return Ok("新增成功");
        }

        // POST: api/Tags
        [HttpPost]
        [JwtAuth]
        [ResponseType(typeof(Tag))]
        public IHttpActionResult PostTag([FromBody] TagCreateModel tag)
        {
            var token = JwtAuth.GetToken(Request.Headers.Authorization.Parameter);
            if ((Convert.ToInt32(token["Permission"]) & 128) <= 0) return BadRequest("權限不足");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var tagModel = new TagModel
            {
                Name = tag.Name,
                Color = tag.Color
            };
            var servicesResult = _tagService.CreateTag(tagModel);
            if (!servicesResult.IsSusses)
            {
                BadRequest(servicesResult.Message);
            }
            return Ok("新增成功");
        }

        // DELETE: api/Tags/5
        [HttpDelete]
        [JwtAuth]
        [ResponseType(typeof(Tag))]
        public IHttpActionResult DeleteTag(int id)
        {
            var token = JwtAuth.GetToken(Request.Headers.Authorization.Parameter);
            if ((Convert.ToInt32(token["Permission"]) & 128) <= 0) return BadRequest("權限不足");
            var servicesResult = _tagService.SetDeleteFlag(id);
            if (!servicesResult.IsSusses)
            {
                BadRequest(servicesResult.Message);
            }
            return Ok(id);
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