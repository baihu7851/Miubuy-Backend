using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Repositories;
using Services;
using Services.Models;

namespace ServiceTest
{
    [TestClass]
    public class TagServiceTest
    {
        private Model _mockDbContext;
        private DbSet<Tag> _mockDbSet;

        [TestInitialize]
        public void TestInitialize()
        {
            // 初始化測試資料
            var testData = new List<Tag>
            {
                new Tag { Id = 1, Name = "Tag1" ,Delete = false, Rooms = new List<Room>() },
                new Tag { Id = 2, Name = "Tag2" ,Delete = false, Rooms = new List<Room>() }
            };

            _mockDbSet = Substitute.For<DbSet<Tag>, IQueryable<Tag>>();
            ((IQueryable<Tag>)_mockDbSet).Provider.Returns(testData.AsQueryable().Provider);
            ((IQueryable<Tag>)_mockDbSet).Expression.Returns(testData.AsQueryable().Expression);
            ((IQueryable<Tag>)_mockDbSet).ElementType.Returns(testData.AsQueryable().ElementType);
            ((IQueryable<Tag>)_mockDbSet).GetEnumerator().Returns(testData.GetEnumerator());

            _mockDbSet.AsNoTracking().Returns(_mockDbSet);

            _mockDbContext = Substitute.For<Model>();
            _mockDbContext.Tags.Returns(_mockDbSet);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // 清理資料，以確保每個測試互相獨立
            _mockDbContext = null;
            _mockDbSet = null;
        }

        #region GetTags

        [TestMethod]
        public void GetTags_ReturnsListOfTagModels()
        {
            // 安排 (Arrange)
            var tagService = new TagService(_mockDbContext);

            // 執行 (Act)
            var result = tagService.GetTags();

            // 斷言 (Assert)
            Assert.IsTrue(result.IsSusses);
            Assert.IsNotNull(result.Data);
            Assert.IsInstanceOfType(result.Data, typeof(List<TagModel>));
            Assert.AreEqual(2, result.Data.Count);

            // 進一步檢查模型映射是否正確
            Assert.AreEqual(1, result.Data[0].Id);
            Assert.AreEqual("Tag1", result.Data[0].Name);
        }

        [TestMethod]
        public void GetTags_ExceptionOccurs_ReturnsError()
        {
            // 安排 (Arrange)
            var tagService = new TagService(_mockDbContext);
            _mockDbSet.AsNoTracking().Returns(_ => throw new Exception());

            // 執行 (Act)
            var result = tagService.GetTags();

            // 斷言 (Assert)
            Assert.IsFalse(result.IsSusses);
            Assert.IsNull(result.Data);
            Assert.IsNotNull(result.Message);
        }

        #endregion GetTags

        #region GetTag

        [TestMethod]
        public void GetTag_ExistingTag_ReturnsTagModel()
        {
            // 安排 (Arrange)
            var tagService = new TagService(_mockDbContext);
            var tagId = 1;

            // 執行 (Act)
            var result = tagService.GetTag(tagId);

            // 斷言 (Assert)
            Assert.IsTrue(result.IsSusses);
            Assert.IsNotNull(result.Data);
            Assert.IsInstanceOfType(result.Data, typeof(TagModel));
            Assert.AreEqual(tagId, result.Data.Id);
        }

        [TestMethod]
        public void GetTag_NonExistingTag_ReturnsError()
        {
            // 安排 (Arrange)
            var tagService = new TagService(_mockDbContext);
            var nonExistingTagId = 999; // 假設這個 ID 不存在

            // 執行 (Act)
            var result = tagService.GetTag(nonExistingTagId);

            // 斷言 (Assert)
            Assert.IsFalse(result.IsSusses);
            Assert.IsNull(result.Data);
            Assert.IsNotNull(result.Message);
        }

        [TestMethod]
        public void GetTag_ExceptionOccurs_ReturnsError()
        {
            // 安排 (Arrange)
            var tagService = new TagService(_mockDbContext);
            var tagId = 1;
            // 模擬當執行查詢時發生異常
            _mockDbSet.AsNoTracking().Returns(_ => throw new Exception());

            // 執行 (Act)
            var result = tagService.GetTag(tagId);

            // 斷言 (Assert)
            Assert.IsFalse(result.IsSusses);
            Assert.IsNull(result.Data);
            Assert.IsNotNull(result.Message);
        }

        #endregion GetTag
    }
}