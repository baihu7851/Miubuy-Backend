using System;
using System.Collections.Generic;
using System.Linq;
using Repositories;
using Services.Models;

namespace Services
{
    public class TagService : IDisposable
    {
        private readonly Model _db;

        public TagService()
        {
            _db = new Model();
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public ServicesResultModel<List<TagModel>> GetTags()
        {
            var result = new ServicesResultModel<List<TagModel>>();
            try
            {
                var models = _db.Tags
                    .AsNoTracking()
                    .Where(tag => !tag.Delete)
                    .Select(tag => new TagModel
                    {
                        Id = tag.Id,
                        Name = tag.Name,
                        Color = tag.Color,
                        RoomCount = tag.Rooms.Count
                    }).ToList();
                result.SetSuccess(models);
            }
            catch (Exception e)
            {
                result.SetException(e);
            }
            return result;
        }

        public ServicesResultModel<TagModel> GetTag(int id)
        {
            var result = new ServicesResultModel<TagModel>();
            try
            {
                var tag = _db.Tags
                    .AsNoTracking()
                    .SingleOrDefault(x => x.Id == id);
                if (tag == default)
                {
                    result.SetError("找不到標籤", 1);
                }
                else
                {
                    var model = new TagModel()
                    {
                        Id = tag.Id,
                        Name = tag.Name,
                        Color = tag.Color,
                        RoomCount = tag.Rooms.Count
                    };
                    result.SetSuccess(model);
                }
            }
            catch (Exception e)
            {
                result.SetException(e);
            }
            return result;
        }

        public ServicesResultModel<TagModel> CreateTag(TagModel tagModel)
        {
            var result = new ServicesResultModel<TagModel>();
            try
            {
                var tag = _db.Tags
                    .SingleOrDefault(x => x.Name == tagModel.Name);
                if (tag != default)
                {
                    result.SetError("標籤已存在", 1);
                }
                else
                {
                    tag = new Tag
                    {
                        Name = tagModel.Name,
                        Color = tagModel.Color
                    };
                    _db.Tags.Add(tag);
                    _db.SaveChanges();
                    result.SetSuccess(null);
                }
            }
            catch (Exception e)
            {
                result.SetException(e);
            }
            return result;
        }

        public ServicesResultModel<object> UpdateTag(TagModel tagModel)
        {
            var result = new ServicesResultModel<object>();
            try
            {
                var tag = _db.Tags
                    .SingleOrDefault(x => x.Id == tagModel.Id);
                if (tag == default)
                {
                    result.SetError("找不到標籤", 1);
                }
                else
                {
                    tag.Name = string.IsNullOrEmpty(tagModel.Name) ? tag.Name : tagModel.Name;
                    tag.Color = string.IsNullOrEmpty(tagModel.Color) ? tag.Color : tagModel.Color;
                    _db.SaveChanges();
                    result.SetSuccess(null);
                }
            }
            catch (Exception e)
            {
                result.SetException(e);
            }
            return result;
        }

        public ServicesResultModel<object> SetDeleteFlag(int id)
        {
            var result = new ServicesResultModel<object>();
            try
            {
                var tag = _db.Tags
                    .SingleOrDefault(x => x.Id == id);
                if (tag == default)
                {
                    result.SetError("找不到標籤", 1);
                }
                else
                {
                    tag.Delete = true;
                    _db.SaveChanges();
                    result.SetSuccess(null);
                }
            }
            catch (Exception e)
            {
                result.SetException(e);
            }
            return result;
        }
    }
}