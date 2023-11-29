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
    public class UserServiceTest
    {
        private Model _mockDbContext;
        private DbSet<User> _mockDbSet;

        [TestInitialize]
        public void TestInitialize()
        {
            // 初始化測試資料
            var testData = new List<User>
            {
                new User { Id = 1, Name = "User1", Account = "user1", Email = "user1@example.com" },
                new User { Id = 2, Name = "User2", Account = "user2", Email = "user2@example.com" }
            };

            _mockDbSet = Substitute.For<DbSet<User>, IQueryable<User>>();
            ((IQueryable<User>)_mockDbSet).Provider.Returns(testData.AsQueryable().Provider);
            ((IQueryable<User>)_mockDbSet).Expression.Returns(testData.AsQueryable().Expression);
            ((IQueryable<User>)_mockDbSet).ElementType.Returns(testData.AsQueryable().ElementType);
            ((IQueryable<User>)_mockDbSet).GetEnumerator().Returns(testData.GetEnumerator());

            _mockDbSet.AsNoTracking().Returns(_mockDbSet);

            _mockDbContext = Substitute.For<Model>();
            _mockDbContext.Users.Returns(_mockDbSet);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // 清理資料，以確保每個測試互相獨立
            _mockDbContext = null;
            _mockDbSet = null;
        }

        #region GetUsers

        [TestMethod]
        public void GetUsers_ReturnsListOfUserModels()
        {
            // 安排 (Arrange)
            var userService = new UserService(_mockDbContext);

            // 執行 (Act)
            var result = userService.GetUsers();

            // 斷言 (Assert)
            Assert.IsTrue(result.IsSusses);
            Assert.IsNotNull(result.Data);
            Assert.IsInstanceOfType(result.Data, typeof(List<UserModel>));
            Assert.AreEqual(2, result.Data.Count);

            // 進一步檢查模型映射是否正確
            Assert.AreEqual(1, result.Data[0].Id);
            Assert.AreEqual("User1", result.Data[0].Name);
        }

        [TestMethod]
        public void GetUsers_ExceptionOccurs_ReturnsError()
        {
            // 安排 (Arrange)
            var userService = new UserService(_mockDbContext);
            _mockDbSet.AsNoTracking().Returns(_ => throw new Exception());

            // 執行 (Act)
            var result = userService.GetUsers();

            // 斷言 (Assert)
            Assert.IsFalse(result.IsSusses);
            Assert.IsNull(result.Data);
            Assert.IsNotNull(result.Message);
        }

        #endregion GetUsers

        #region GetUser(byId)

        [TestMethod]
        public void GetUserById_ExistingUser_ReturnsUserModel()
        {
            // 安排 (Arrange)
            var userService = new UserService(_mockDbContext);
            var userId = 1;

            // 執行 (Act)
            var result = userService.GetUser(userId);

            // 斷言 (Assert)
            Assert.IsTrue(result.IsSusses);
            Assert.IsNotNull(result.Data);
            Assert.IsInstanceOfType(result.Data, typeof(UserModel));
            Assert.AreEqual(userId, result.Data.Id);
        }

        [TestMethod]
        public void GetUserById_NonExistingUser_ReturnsError()
        {
            // 安排 (Arrange)
            var userService = new UserService(_mockDbContext);
            var nonExistingUserId = 999; // 假設這個 ID 不存在

            // 執行 (Act)
            var result = userService.GetUser(nonExistingUserId);

            // 斷言 (Assert)
            Assert.IsFalse(result.IsSusses);
            Assert.IsNull(result.Data);
            Assert.IsNotNull(result.Message);
        }

        [TestMethod]
        public void GetUserById_ExceptionOccurs_ReturnsError()
        {
            // 安排 (Arrange)
            var userService = new UserService(_mockDbContext);
            var userId = 1;
            // 模擬當執行查詢時發生異常
            _mockDbSet.AsNoTracking().Returns(_ => throw new Exception());

            // 執行 (Act)
            var result = userService.GetUser(userId);

            // 斷言 (Assert)
            Assert.IsFalse(result.IsSusses);
            Assert.IsNull(result.Data);
            Assert.IsNotNull(result.Message);
        }

        #endregion GetUser(byId)

        #region GetUser(byAccount)

        [TestMethod]
        public void GetUserByAccount_ExistingUser_ReturnsUserModel()
        {
            // 安排 (Arrange)
            var userService = new UserService(_mockDbContext);
            var userAccount = "user1";

            // 執行 (Act)
            var result = userService.GetUser(userAccount);

            // 斷言 (Assert)
            Assert.IsTrue(result.IsSusses);
            Assert.IsNotNull(result.Data);
            Assert.IsInstanceOfType(result.Data, typeof(UserModel));
            Assert.AreEqual(userAccount, result.Data.Account);
        }

        [TestMethod]
        public void GetUserByAccount_NonExistingUser_ReturnsError()
        {
            // 安排 (Arrange)
            var userService = new UserService(_mockDbContext);
            var nonExistingUserAccount = "user999"; // 假設這個 ID 不存在

            // 執行 (Act)
            var result = userService.GetUser(nonExistingUserAccount);

            // 斷言 (Assert)
            Assert.IsFalse(result.IsSusses);
            Assert.IsNull(result.Data);
            Assert.IsNotNull(result.Message);
        }

        [TestMethod]
        public void GetUserByAccount_ExceptionOccurs_ReturnsError()
        {
            // 安排 (Arrange)
            var userService = new UserService(_mockDbContext);
            var userAccount = "user11";
            // 模擬當執行查詢時發生異常
            _mockDbSet.AsNoTracking().Returns(_ => throw new Exception());

            // 執行 (Act)
            var result = userService.GetUser(userAccount);

            // 斷言 (Assert)
            Assert.IsFalse(result.IsSusses);
            Assert.IsNull(result.Data);
            Assert.IsNotNull(result.Message);
        }

        #endregion GetUser(byAccount)
    }
}