using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Umbraco.Core;
using Umbraco.Core.Persistence;
using System.IO;
using System.Data.SqlServerCe;
using ThumNet.EasyAD.IntegrationTests.Framework;
using ThumNet.EasyAD.Handlers;
using ThumNet.EasyAD.Repositories;
using ThumNet.EasyAD.Models;
using Umbraco.Core.Services;
using System.Linq;
using System.Collections.Generic;

namespace ThumNet.EasyAD.IntegrationTests
{
    [TestClass]
    public class UnitTest1
    {
        #region TestContext
        private TestContext testContextInstance;
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        #endregion

        #region Properties
        
        private TestGroupBasedUserManager Manager
        {
            get
            {
                return (TestGroupBasedUserManager)TestContext.Properties[TestSetup.ManagerPropertyKey];
            }
        }

        private IEasyADRepository _repo = null;
        private IEasyADRepository Repo
        {
            get
            {
                return _repo ?? (_repo = new EasyADRepository(ApplicationContext.Current.DatabaseContext.Database, ApplicationContext.Current.DatabaseContext.SqlSyntax));
            }
        }

        private ServiceContext Services
        {
            get
            {
                return ApplicationContext.Current.Services;
            }
        }

        private Dictionary<string, int> AllUserTypes { get; set; }
        private Dictionary<string, string> AllSections { get; set; }

        [TestInitialize]
        public void TestInit()
        {
            // Administrators, Writer, Editors, Translators
            AllUserTypes = Services.UserService.GetAllUserTypes().ToDictionary(t => t.Name, t => t.Id);

            // Content, Media, Settings, Developer, Users, Members, Forms, Translation
            AllSections = Services.SectionService.GetSections().ToDictionary(t => t.Name, t => t.Alias);
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
            var adminUser = Services.UserService.GetUserById(0);

            Assert.IsNotNull(adminUser, "Admin should be available");
            Assert.AreEqual(adminUser.Name, "Administrator");
        }

        [TestMethod]
        public void TestManager()
        {
            Assert.IsNotNull(Manager, "Manager should be available");
        }


        [TestMethod]
        public void TestSaveGroupHandler()
        {
            //Arrange
            var handler = new SaveGroupHandler(Repo, Manager, Services.UserService);
            var groupName = "AD-Editors";
            Manager.AddUsers(groupName,
                new EasyADUser { Login = "userA", DiplayName = "User A", Email = "usera@local" }
                );
            var group = new EasyADGroup { Name = groupName, UserType = AllUserTypes["Editors"], Sections = string.Join(";", AllSections["Content"], AllSections["Media"]) };

            //Act
            handler.Handle(group);
            var groups = Repo.GetAll();
            var user = Services.UserService.GetByUsername("userA");

            //Assert
            Assert.AreEqual(1, groups.Count());
            Assert.AreEqual(groupName, groups.First().Name);

            Assert.IsNotNull(user);
            Assert.AreEqual("User A", user.Name);
            Assert.AreEqual("usera@local", user.Email);
            Assert.AreEqual(group.UserType, user.UserType.Id);

            Assert.AreEqual(2, user.AllowedSections.Count());
            Assert.IsTrue(user.AllowedSections.Contains(AllSections["Content"]));
            Assert.IsTrue(user.AllowedSections.Contains(AllSections["Media"]));
        }
    }
}
