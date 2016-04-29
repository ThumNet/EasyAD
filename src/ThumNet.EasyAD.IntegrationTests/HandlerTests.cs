using Microsoft.VisualStudio.TestTools.UnitTesting;
using Umbraco.Core;
using Umbraco.Core.Persistence;
using ThumNet.EasyAD.IntegrationTests.Framework;
using ThumNet.EasyAD.Handlers;
using ThumNet.EasyAD.Models;
using System.Linq;
using System.Collections.Generic;

namespace ThumNet.EasyAD.IntegrationTests
{
    [TestClass]
    public class HandlerTests : TestsBase
    {
        #region Before and after each test
        private Dictionary<string, int> _userTypes = null;
        private Dictionary<string, int> AllUserTypes
        {
            get
            {
                // Administrators, Writer, Editors, Translators
                return _userTypes ?? (_userTypes = Services.UserService.GetAllUserTypes().ToDictionary(t => t.Name, t => t.Id));
            }
        }

        private Dictionary<string, string> _sections = null;
        private Dictionary<string, string> AllSections
        {
            get
            {
                // Content, Media, Settings, Developer, Users, Members, Forms, Translation
                return _sections ?? (_sections = Services.SectionService.GetSections().ToDictionary(t => t.Name, t => t.Alias));
            }
        }
        Transaction _transaction = null;

        [TestInitialize]
        public void TestInit()
        {
            //Use a transaction to provide a clean DB for each test, note: transaction is never commited
            //Recreating the entire DB for each test is to time consuming! 
            _transaction = ApplicationContext.Current.DatabaseContext.Database.GetTransaction();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
            }
        }

        #endregion

        private EasyADGroup CreateWritersGroup()
        {
            var handler = new SaveGroupHandler(Repo, Manager, Services.UserService);
            var groupName = "AD-Writers";
            Manager.AddUsers(groupName,
                new EasyADUser { Login = "userA", DiplayName = "User A", Email = "usera@local" }
            );
            var group = new EasyADGroup
            {
                Name = groupName,
                UserType = AllUserTypes["Writer"],
                Sections = string.Join(";", AllSections["Content"], AllSections["Media"])
            };
            handler.Handle(group);
            return group;
        }

        private EasyADGroup CreateEditorsGroup()
        {
            var handler = new SaveGroupHandler(Repo, Manager, Services.UserService);
            var groupName = "AD-Editors";
            Manager.AddUsers(groupName,
                new EasyADUser { Login = "userA", DiplayName = "User A", Email = "usera@local" },
                new EasyADUser { Login = "userB", DiplayName = "User B", Email = "userb@local" }
                );
            var group = new EasyADGroup
            {
                Name = groupName,
                UserType = AllUserTypes["Editors"],
                Sections = string.Join(";", AllSections["Content"], AllSections["Media"], AllSections["Members"], AllSections["Forms"])
            };
            handler.Handle(group);
            return group;
        }

        private EasyADGroup CreateAdminsGroup()
        {
            var handler = new SaveGroupHandler(Repo, Manager, Services.UserService);
            var groupName = "AD-Admins";
            Manager.AddUsers(groupName,
                new EasyADUser { Login = "userB", DiplayName = "User B", Email = "userB@local" },
                new EasyADUser { Login = "userC", DiplayName = "User C", Email = "userC@local" }
            );
            var group = new EasyADGroup
            {
                Name = groupName,
                UserType = AllUserTypes["Editors"],
                Sections = string.Join(";", AllSections.Values)
            };
            handler.Handle(group);
            return group;
        }

        private int GetUserCount()
        {
            int total = 0;
            Services.UserService.GetAll(0, 1, out total);
            return total;
        }

        private void AssertUserForGroup(Umbraco.Core.Models.Membership.IUser user, EasyADGroup group)
        {
            Assert.AreEqual(group.UserType, user.UserType.Id, "Expected the UserType to match the groups");

            Assert.AreEqual(group.SectionsArray.Length, user.AllowedSections.Count(), "Expected the user sections count to be the same as the groups");
            Assert.IsTrue(group.SectionsArray.ContainsAll(user.AllowedSections), "Expected the user sections count to be the same as the groups");
        }

        [TestMethod]
        public void SaveGroup()
        {
            //Arrange
            var group = CreateWritersGroup();

            //Act            
            var groups = Repo.GetAll();
            var userA = Services.UserService.GetByUsername("userA");

            //Assert
            Assert.AreEqual(1, groups.Count());
            Assert.AreEqual(group.Name, groups.First().Name);

            Assert.IsNotNull(userA);
            Assert.AreEqual("User A", userA.Name);
            Assert.AreEqual("usera@local", userA.Email);

            AssertUserForGroup(userA, group);
        }

        /// <summary>
        /// Create the Writers and Editors group.
        /// Expected:
        ///     - UserA had rights of Editors groups (this is highest).
        /// </summary>
        [TestMethod]
        public void SaveGroupsWithOverlap()
        {
            //Arrange
            int initialUserCount = GetUserCount();
            var group1 = CreateWritersGroup();
            var group2 = CreateEditorsGroup();
            var afterAddUserCount = GetUserCount();

            //Act            
            var groups = Repo.GetAll().ToList();
            var userA = Services.UserService.GetByUsername("userA");
            var userB = Services.UserService.GetByUsername("userB");

            //Assert
            Assert.AreEqual(2, groups.Count());
            Assert.AreEqual(group1.Name, groups[0].Name);
            Assert.AreEqual(group2.Name, groups[1].Name);

            Assert.AreEqual(1, initialUserCount);
            Assert.AreEqual(3, afterAddUserCount);

            AssertUserForGroup(userA, group2);
        }

        [TestMethod]
        public void DeleteGroup()
        {
            //Arrange
            int initialUserCount = GetUserCount();
            CreateWritersGroup();
            var handler = new DeleteGroupHandler(Repo, Manager, Services.UserService);

            //Act
            var groups = Repo.GetAll();
            Assert.AreEqual(1, groups.Count());

            var afterAddUserCount = GetUserCount();
            Assert.AreEqual(initialUserCount + 1, afterAddUserCount);

            var theGroup = groups.First();

            //Act
            handler.Handle(theGroup.Id);

            groups = Repo.GetAll();

            //Assert
            Assert.AreEqual(0, groups.Count());

            var afterDeleteUserCount = GetUserCount();
            Assert.AreEqual(initialUserCount, afterDeleteUserCount);
        }

        /// <summary>
        /// Create the Writers and Editors group.
        /// Delete the Editors group.
        /// Expected: 
        ///     - UserB is removed (in no other group after delete).
        ///     - UserA has rights of Writers group (only in Writers group after delete).
        /// </summary>
        [TestMethod]
        public void DeleteGroupWithOverlap()
        {
            //Arrange
            var group1 = CreateWritersGroup();
            var group2 = CreateEditorsGroup();

            var handler = new DeleteGroupHandler(Repo, Manager, Services.UserService);

            //Act
            var groups = Repo.GetAll();
            Assert.AreEqual(2, groups.Count());

            var afterAddUserCount = GetUserCount();
            Assert.AreEqual(3, afterAddUserCount);

            var theGroup = groups.First(g=> g.Name == group2.Name);

            //Act
            handler.Handle(theGroup.Id);
            groups = Repo.GetAll();
            var userA = Services.UserService.GetByUsername("userA");

            //Assert
            Assert.AreEqual(1, groups.Count());

            var afterDeleteUserCount = GetUserCount();
            Assert.AreEqual(2, afterDeleteUserCount);

            AssertUserForGroup(userA, group1);
        }
    }
}
