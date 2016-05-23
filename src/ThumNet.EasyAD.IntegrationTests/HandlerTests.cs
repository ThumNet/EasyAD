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
                // Administrators, Writer, Editors, Translator
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

        private Transaction _transaction = null;

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

        internal class ADUsers
        {
            internal static EasyADUser AndrewDavis = new EasyADUser { DisplayName = "Andrew Davis", Login = "DavisAn", Email = "DavisAn@test.local" };
            internal static EasyADUser ChristopherMoore = new EasyADUser { DisplayName = "Christopher Moore", Login = "MooreCh", Email = "MooreCh@test.local" };
            internal static EasyADUser DanielMiller = new EasyADUser { DisplayName = "Daniel Miller", Login = "MilleDa", Email = "MilleDa@test.local" };
            internal static EasyADUser JesusBrooks = new EasyADUser { DisplayName = "Jesus Brooks", Login = "BrookJe", Email = "BrookJe@test.local" };
            internal static EasyADUser GracieMuhlberg = new EasyADUser { DisplayName = "Gracie Muhlberg", Login = "MuhlbGr", Email = "MuhlbGr@test.local" };
            internal static EasyADUser MichaelJohnson = new EasyADUser { DisplayName = "Michael Johnson", Login = "JohnsMi", Email = "JohnsMi@test.local" };
            internal static EasyADUser SethButler = new EasyADUser { DisplayName = "Seth Butler", Login = "ButleSe", Email = "ButleSe@test.local" };
        }

        #endregion

        private EasyADGroup CreateWritersGroup()
        {
            var handler = new SaveGroupHandler(Repo, Manager, Services.UserService);
            var groupName = "AD-Writers";
            Manager.AddUsers(groupName,
                ADUsers.AndrewDavis
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
                ADUsers.AndrewDavis,
                ADUsers.ChristopherMoore
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

        private EasyADGroup CreateTranslatorsGroup()
        {
            var handler = new SaveGroupHandler(Repo, Manager, Services.UserService);
            var groupName = "AD-Translators";
            Manager.AddUsers(groupName,
                ADUsers.ChristopherMoore,
                ADUsers.SethButler
                );
            var group = new EasyADGroup
            {
                Name = groupName,
                UserType = AllUserTypes["Translator"],
                Sections = AllSections["Translation"]
            };
            handler.Handle(group);
            return group;
        }

        private EasyADGroup CreateAdminsGroup()
        {
            var handler = new SaveGroupHandler(Repo, Manager, Services.UserService);
            var groupName = "AD-Admins";
            Manager.AddUsers(groupName,
                ADUsers.ChristopherMoore,
                ADUsers.DanielMiller
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

        private void AssertUserForGroup(Umbraco.Core.Models.Membership.IUser user, EasyADGroup group, string[] extraSections = null)
        {
            Assert.AreEqual(group.UserType, user.UserType.Id, "Expected the UserType to match the groups");

            var expectedSections = group.SectionsArray;
            if (extraSections != null) { expectedSections = expectedSections.Concat(extraSections).ToArray(); }

            Assert.AreEqual(expectedSections.Length, user.AllowedSections.Count(), "Expected the user sections count to be the same as the groups");
            Assert.IsTrue(expectedSections.ContainsAll(user.AllowedSections), "Expected the user sections count to be the same as the groups");
        }

        [TestMethod]
        public void SaveGroup()
        {
            //Arrange
            var group = CreateWritersGroup();

            //Act            
            var groups = Repo.GetAll();
            var userA = Services.UserService.GetByUsername(ADUsers.AndrewDavis.Login);

            //Assert
            Assert.AreEqual(1, groups.Count());
            Assert.AreEqual(group.Name, groups.First().Name);

            Assert.IsNotNull(userA);
            Assert.AreEqual(ADUsers.AndrewDavis.DisplayName, userA.Name);
            Assert.AreEqual(ADUsers.AndrewDavis.Email, userA.Email);

            AssertUserForGroup(userA, group);
        }

        /// <summary>
        /// Create the Writers and Editors group.
        /// Expected:
        ///     - AndrewDavis had rights of Editors groups (this is highest).
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
            var userA = Services.UserService.GetByUsername(ADUsers.AndrewDavis.Login);
            var userB = Services.UserService.GetByUsername(ADUsers.ChristopherMoore.Login);

            //Assert
            Assert.AreEqual(2, groups.Count());
            Assert.AreEqual(group1.Name, groups[0].Name);
            Assert.AreEqual(group2.Name, groups[1].Name);

            Assert.AreEqual(1, initialUserCount);
            Assert.AreEqual(3, afterAddUserCount);

            AssertUserForGroup(userA, group2);
        }

        /// <summary>
        /// Create the Editors and Translators group.
        /// Expected:
        ///     - AndrewDavis had rights of Editors groups (this is highest).
        /// </summary>
        [TestMethod]
        public void SaveGroupsWithOverlapAndExtraSection()
        {
            //Arrange
            int initialUserCount = GetUserCount();
            var group1 = CreateEditorsGroup();
            var group2 = CreateTranslatorsGroup();
            var afterAddUserCount = GetUserCount();

            //Act            
            var groups = Repo.GetAll().ToList();
            var userA = Services.UserService.GetByUsername(ADUsers.ChristopherMoore.Login);
            var userB = Services.UserService.GetByUsername(ADUsers.SethButler.Login);

            //Assert
            Assert.AreEqual(2, groups.Count());
            Assert.AreEqual(group1.Name, groups[0].Name);
            Assert.AreEqual(group2.Name, groups[1].Name);

            Assert.AreEqual(1, initialUserCount);
            Assert.AreEqual(4, afterAddUserCount);

            AssertUserForGroup(userA, group1, group2.SectionsArray);
            AssertUserForGroup(userB, group2);
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
            var userA = Services.UserService.GetByUsername(ADUsers.AndrewDavis.Login);

            //Assert
            Assert.AreEqual(1, groups.Count());

            var afterDeleteUserCount = GetUserCount();
            Assert.AreEqual(2, afterDeleteUserCount);

            AssertUserForGroup(userA, group1);
        }

        [TestMethod]
        public void RefreshGroupNewUserGetsAddedToGroup()
        {
            //Arrange
            int initialUserCount = GetUserCount();
            var group = CreateWritersGroup();
            var newUser = ADUsers.MichaelJohnson;
            var handler = new RefreshGroupsHandler(Repo, Manager, Services.UserService);

            //Act
            var groups = Repo.GetAll();
            Manager.AddUsers(group.Name, newUser);
            handler.Handle();
            var afterRefreshUserCount = GetUserCount();
            var user = Services.UserService.GetByUsername(newUser.Login);

            //Assert
            Assert.AreEqual(1, initialUserCount, "Initial count");
            Assert.AreEqual(3, afterRefreshUserCount, "After refresh count");

            Assert.IsNotNull(user);
            Assert.AreEqual(newUser.DisplayName, user.Name);
            Assert.AreEqual(newUser.Email, user.Email);

            AssertUserForGroup(user, group);
        }

        [TestMethod]
        public void RefreshGroupExistingUserGetsRemovedFromGroup()
        {
            //Arrange
            int initialUserCount = GetUserCount();
            var group = CreateWritersGroup();
            
            var handler = new RefreshGroupsHandler(Repo, Manager, Services.UserService);

            //Act
            var groups = Repo.GetAll();
            Manager.RemoveUsers(group.Name, ADUsers.AndrewDavis.Login);
            handler.Handle();            
            var user = Services.UserService.GetByUsername(ADUsers.AndrewDavis.Login);
            var afterRefreshUserCount = GetUserCount();

            //Assert
            Assert.AreEqual(1, initialUserCount, "Initial count");
            Assert.AreEqual(1, afterRefreshUserCount, "After refresh count");

            Assert.IsNull(user);
        }

        [TestMethod]
        public void RefreshGroupGroupNoLongerExistsRemovesUser()
        {
            //Arrange
            int initialUserCount = GetUserCount();
            var group = CreateWritersGroup();
            int afterAddUserCount = GetUserCount();
            Manager.RemoveGroup(group.Name);

            var handler = new RefreshGroupsHandler(Repo, Manager, Services.UserService);

            //Act
            handler.Handle();
            var afterRefreshUserCount = GetUserCount();

            //Assert
            Assert.AreEqual(1, initialUserCount, "Initial count");
            Assert.AreEqual(2, afterAddUserCount, "After add count");
            Assert.AreEqual(1, afterRefreshUserCount, "After refresh count");            
        }

        [TestMethod]
        public void RefreshGroupExistingUserInMultipleGroupsGetsRemovedFromGroup()
        {
            //Arrange
            int initialUserCount = GetUserCount();
            var group1 = CreateWritersGroup();
            var group2 = CreateEditorsGroup();
            int afterAddUserCount = GetUserCount();            

            var handler = new RefreshGroupsHandler(Repo, Manager, Services.UserService);

            //Act
            var groups = Repo.GetAll();
            Manager.RemoveUsers(group2.Name, ADUsers.AndrewDavis.Login);
            handler.Handle();
            var afterRefreshUserCount = GetUserCount();
            var userA = Services.UserService.GetByUsername(ADUsers.AndrewDavis.Login);
            var userB = Services.UserService.GetByUsername(ADUsers.ChristopherMoore.Login);

            //Assert
            Assert.AreEqual(1, initialUserCount, "Initial count");
            Assert.AreEqual(3, afterAddUserCount, "After add count");
            Assert.AreEqual(3, afterRefreshUserCount, "After refresh count");

            AssertUserForGroup(userA, group1);
            AssertUserForGroup(userB, group2);
        }

        [TestMethod]
        public void RefreshGroupExistingUserGetsAddedToLesserRightsGroup()
        {
            //Arrange
            int initialUserCount = GetUserCount();
            var group1 = CreateWritersGroup();
            var group2 = CreateEditorsGroup();
            int afterAddUserCount = GetUserCount();
            var handler = new RefreshGroupsHandler(Repo, Manager, Services.UserService);

            //Act
            var groups = Repo.GetAll();
            Manager.AddUsers(group1.Name, ADUsers.AndrewDavis);
            handler.Handle();
            var afterRefreshUserCount = GetUserCount();
            var userA = Services.UserService.GetByUsername(ADUsers.AndrewDavis.Login);
            var userB = Services.UserService.GetByUsername(ADUsers.ChristopherMoore.Login);

            //Assert
            Assert.AreEqual(1, initialUserCount, "Initial count");
            Assert.AreEqual(3, afterAddUserCount, "After add count");
            Assert.AreEqual(3, afterRefreshUserCount, "After refresh count");

            AssertUserForGroup(userA, group2);
            AssertUserForGroup(userB, group2);
        }

        [TestMethod]
        public void RefreshGroupExistingUserGetsAddedToGreaterRightsGroup()
        {
            //Arrange
            int initialUserCount = GetUserCount();
            var adminGroup = CreateAdminsGroup();
            var writersGroup = CreateWritersGroup();
            int afterAddUserCount = GetUserCount();

            var handler = new RefreshGroupsHandler(Repo, Manager, Services.UserService);

            //Act
            var groups = Repo.GetAll();
            Manager.AddUsers(adminGroup.Name, ADUsers.AndrewDavis);
            handler.Handle();

            var afterRefreshUserCount = GetUserCount();
            var userA = Services.UserService.GetByUsername(ADUsers.AndrewDavis.Login);

            //Assert
            Assert.AreEqual(1, initialUserCount, "Initial count");
            Assert.AreEqual(4, afterAddUserCount, "After add count");
            Assert.AreEqual(4, afterRefreshUserCount, "After refresh count");

            AssertUserForGroup(userA, adminGroup);
        }
    }
}
