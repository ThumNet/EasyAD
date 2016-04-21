using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThumNet.EasyAD.Models;

namespace ThumNet.EasyAD.Tests.Models
{
    [TestClass]
    public class EasyADUserTests
    {
        [TestMethod]
        public void Test_GetHashCode_HashesLogin()
        {
            //Arrange
            var user = new EasyADUser { Login = "test", DiplayName = "Test User", Email = "Test@User" };

            //Act

            //Assert
            Assert.AreEqual(user.Login.GetHashCode(), user.GetHashCode());
        }

        [TestMethod]
        public void Test_Equals_ComparesLoginOnly()
        {
            //Arrange
            var user1 = new EasyADUser { Login = "test", DiplayName = "Test User", Email = "Test@User" };
            var user2 = new EasyADUser { Login = "test", DiplayName = "Test User2", Email = "Test@User" };
            var user3 = new EasyADUser { Login = "test", DiplayName = "Test User", Email = "Test@User2" };
            var user4 = new EasyADUser { Login = "testA", DiplayName = "Test User", Email = "Test@User" };

            //Act

            //Assert
            Assert.AreEqual(user1, user2);
            Assert.AreEqual(user1, user3);
            Assert.AreEqual(user2, user3);
            Assert.AreNotEqual(user4, user1);
        }
    }
}
