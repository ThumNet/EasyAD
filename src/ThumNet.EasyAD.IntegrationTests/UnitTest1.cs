using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Umbraco.Core;
using Umbraco.Core.Persistence;
using System.IO;
using System.Data.SqlServerCe;

namespace ThumNet.EasyAD.IntegrationTests
{
    [TestClass]
    public class UnitTest1
    {

        #region Test setup/cleanup
        private static TestApplicationBase _applicationBase;

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            _applicationBase = new TestApplicationBase();
            _applicationBase.Start(_applicationBase, new EventArgs());
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            if (_applicationBase != null)
            {
                _applicationBase.Dispose();
            }
        }
        
        #endregion

        [TestMethod]
        public void TestApplicationContext()
        {
            var context = ApplicationContext.Current;
            Assert.IsNotNull(context, "ApplicationContext should be available");
        }
        
        [TestMethod]
        public void TestAdminUser()
        {
            var context = ApplicationContext.Current;
            var userService = context.Services.UserService;
            var adminUser = userService.GetUserById(0);

            Assert.IsNotNull(adminUser, "Admin should be available");
            Assert.AreEqual(adminUser.Name, "Administrator");
        }

    }
}
