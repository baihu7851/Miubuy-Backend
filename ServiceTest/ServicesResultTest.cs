using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.Models;
using System;

namespace ServiceTest
{
    [TestClass]
    public class ServicesResultTest
    {
        [TestMethod]
        public void SetSuccess_WithData_SetsPropertiesCorrectly()
        {
            // 安排 (Arrange)
            var serviceResult = new ServicesResultModel<object>();
            var data = new object();

            // 執行 (Act)
            serviceResult.SetSuccess(data);

            // 斷言 (Assert)
            Assert.IsTrue(serviceResult.IsSusses);
            Assert.AreEqual(0, serviceResult.Code);
            Assert.AreEqual(data, serviceResult.Data);
        }

        [TestMethod]
        public void SetSuccess_WithDataAndCode_SetsPropertiesCorrectly()
        {
            // 安排 (Arrange)
            var serviceResult = new ServicesResultModel<object>();
            var data = new object();

            // 執行 (Act)
            serviceResult.SetSuccess(data);

            // 斷言 (Assert)
            Assert.IsTrue(serviceResult.IsSusses);
            Assert.AreEqual(0, serviceResult.Code);
            Assert.AreEqual(data, serviceResult.Data);
        }

        [TestMethod]
        public void SetError_SetsPropertiesCorrectly()
        {
            // 安排 (Arrange)
            var serviceResult = new ServicesResultModel<object>();

            // 執行 (Act)
            serviceResult.SetError("Error Message", 9999);

            // 斷言 (Assert)
            Assert.IsFalse(serviceResult.IsSusses);
            Assert.AreEqual(9999, serviceResult.Code);
            Assert.AreEqual("Error Message", serviceResult.Message);
        }

        [TestMethod]
        public void SetException_SetsPropertiesCorrectly()
        {
            // 安排 (Arrange)
            var serviceResult = new ServicesResultModel<object>();
            var exception = new Exception("Test Exception");

            // 執行 (Act)
            serviceResult.SetException(exception, 9999);

            // 斷言 (Assert)
            Assert.IsFalse(serviceResult.IsSusses);
            Assert.AreEqual(9999, serviceResult.Code);
            Assert.AreEqual(exception.Message, serviceResult.Message);
            Assert.AreEqual(exception, serviceResult.Exception);
        }

        [TestMethod]
        public void NotSet_SetsPropertiesCorrectly()
        {
            // 安排 (Arrange)
            var serviceResult = new ServicesResultModel<object>(); // object 是 T 的類型

            // 執行 (Act)
            // (在這個測試中，不執行任何 Set 方法)

            // 斷言 (Assert)
            Assert.IsFalse(serviceResult.IsSusses);
            Assert.AreEqual(-1, serviceResult.Code);
            Assert.AreEqual("尚未設定回傳狀態", serviceResult.Message);
            Assert.IsNull(serviceResult.Data);
        }
    }
}