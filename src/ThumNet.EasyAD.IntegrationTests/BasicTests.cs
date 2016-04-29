using Microsoft.VisualStudio.TestTools.UnitTesting;
using Umbraco.Core;
using ThumNet.EasyAD.IntegrationTests.Framework;

namespace ThumNet.EasyAD.IntegrationTests
{
    [TestClass]
    public class BasicTests : TestsBase
    {
        [TestMethod]
        public void TestApplicationContext()
        {
            var context = ApplicationContext.Current;
            Assert.IsNotNull(context, "ApplicationContext should be available");
        }

        [TestMethod]
        public void TestAdminUser()
        {
            var adminUser = Services.UserService.GetUserById(0);

            Assert.IsNotNull(adminUser, "Admin should be available");
            Assert.AreEqual(adminUser.Name, "Administrator");
        }

        [TestMethod]
        public void TestManager()
        {
            Assert.IsNotNull(Manager, "Manager should be available");
        }
    }
}
