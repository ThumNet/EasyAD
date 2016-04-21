using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThumNet.EasyAD.Handlers;
using System.Collections.Generic;
using ThumNet.EasyAD.Models;
using Umbraco.Core.Models.Membership;
using ThumNet.EasyAD.Tests.TestData;

namespace ThumNet.EasyAD.Tests.Handlers
{
    [TestClass]
    public class RefreshGroupsHandlerTests : BaseHandlerTest
    {
        #region Does nothing

        [TestMethod]
        public void Handle_NoGroups_DoesNothing()
        {
            // Arrange
            var handler = new RefreshGroupsHandler(Repo.Object, GroupManager.Object, UserService.Object);

            // Act
            handler.Handle();

            Repo.Verify(r => r.GetAll());
            Repo.Verify(r => r.GetAllUsers());

            GroupManager.Verify(u => u.GetUsersInGroup(It.IsAny<string>()), Times.Never);
            UserService.Verify(r => r.GetUserById(It.IsAny<int>()), Times.Never);
            Repo.Verify(r => r.DeleteUser(It.IsAny<int>()), Times.Never);
            UserService.Verify(u => u.Delete(It.IsAny<IUser>(), It.IsAny<bool>()), Times.Never);
            
            UserService.Verify(u => u.CreateUserWithIdentity(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IUserType>()), Times.Never);
            UserService.Verify(u => u.Save(It.IsAny<IUser>(), It.IsAny<bool>()), Times.Never);
        }

        [TestMethod]
        public void Handle_NoUsers_DoesNothing()
        {
            // Arrange
            var handler = new RefreshGroupsHandler(Repo.Object, GroupManager.Object, UserService.Object);
            Repo.Setup(c => c.GetAll()).Returns(new[] { Constants.BackofficeGroups.Developers });

            // Act
            handler.Handle();

            Repo.Verify(r => r.GetAll());
            Repo.Verify(r => r.GetAllUsers());


            GroupManager.Verify(u => u.GetUsersInGroup(Constants.BackofficeGroups.Developers.Name), Times.Once);
            UserService.Verify(r => r.GetUserById(It.IsAny<int>()), Times.Never);
            Repo.Verify(r => r.DeleteUser(It.IsAny<int>()), Times.Never);
            UserService.Verify(u => u.Delete(It.IsAny<IUser>(), It.IsAny<bool>()), Times.Never);

            UserService.Verify(u => u.CreateUserWithIdentity(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IUserType>()), Times.Never);
            UserService.Verify(u => u.Save(It.IsAny<IUser>(), It.IsAny<bool>()), Times.Never);
        }

        [TestMethod]
        public void Handle_2Groups_NoUsers_DoesNothing()
        {
            // Arrange
            var handler = new RefreshGroupsHandler(Repo.Object, GroupManager.Object, UserService.Object);
            Repo.Setup(c => c.GetAll()).Returns(new[] { Constants.BackofficeGroups.Developers, Constants.BackofficeGroups.Testers });

            // Act
            handler.Handle();

            Repo.Verify(r => r.GetAll());
            Repo.Verify(r => r.GetAllUsers());


            GroupManager.Verify(u => u.GetUsersInGroup(Constants.BackofficeGroups.Developers.Name), Times.Once);
            GroupManager.Verify(u => u.GetUsersInGroup(Constants.BackofficeGroups.Testers.Name), Times.Once);
            UserService.Verify(r => r.GetUserById(It.IsAny<int>()), Times.Never);
            Repo.Verify(r => r.DeleteUser(It.IsAny<int>()), Times.Never);
            UserService.Verify(u => u.Delete(It.IsAny<IUser>(), It.IsAny<bool>()), Times.Never);

            UserService.Verify(u => u.CreateUserWithIdentity(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IUserType>()), Times.Never);
            UserService.Verify(u => u.Save(It.IsAny<IUser>(), It.IsAny<bool>()), Times.Never);
        }

        [TestMethod]
        public void Handle_SameUser_DoesNothing()
        {
            // Arrange
            var handler = new RefreshGroupsHandler(Repo.Object, GroupManager.Object, UserService.Object);
            var users = new List<EasyADGroup2User> { new EasyADGroup2User { GroupId = Constants.BackofficeGroups.Developers.Id, UserId = Constants.BackofficeUsers.JohnsMiAsDeveloper.Id } };
            Repo.Setup(c => c.GetAll()).Returns(new[] { Constants.BackofficeGroups.Developers });
            Repo.Setup(c => c.GetAllUsers()).Returns(users);
            UserService.Setup(c => c.GetUserById(Constants.BackofficeUsers.JohnsMiAsDeveloper.Id)).Returns(Constants.BackofficeUsers.JohnsMiAsDeveloper);
            GroupManager.Setup(c => c.GetUsersInGroup(Constants.BackofficeGroups.Developers.Name)).Returns(new[] { Constants.ADUsers.JohnsMi });

            // Act
            handler.Handle();

            Repo.Verify(r => r.GetAll());
            Repo.Verify(r => r.GetAllUsers());

            GroupManager.Verify(u => u.GetUsersInGroup(It.IsAny<string>()), Times.Once);
            UserService.Verify(r => r.GetUserById(It.IsAny<int>()), Times.Once);
            Repo.Verify(r => r.DeleteUser(It.IsAny<int>()), Times.Never);
            UserService.Verify(u => u.Delete(It.IsAny<IUser>(), It.IsAny<bool>()), Times.Never);

            UserService.Verify(u => u.CreateUserWithIdentity(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IUserType>()), Times.Never);
            UserService.Verify(u => u.Save(It.IsAny<IUser>(), It.IsAny<bool>()), Times.Never);
        }

        [TestMethod]
        public void Handle_2Groups_SameUser_DoesNothing()
        {
            // Arrange
            var handler = new RefreshGroupsHandler(Repo.Object, GroupManager.Object, UserService.Object);
            var users = new List<EasyADGroup2User> {
                new EasyADGroup2User { GroupId = Constants.BackofficeGroups.Developers.Id, UserId = Constants.BackofficeUsers.JohnsMiAsDeveloper.Id },
                new EasyADGroup2User { GroupId = Constants.BackofficeGroups.Testers.Id, UserId = Constants.BackofficeUsers.JohnsMiAsDeveloper.Id }
            };
            Repo.Setup(c => c.GetAll()).Returns(new[] { Constants.BackofficeGroups.Developers, Constants.BackofficeGroups.Testers });
            Repo.Setup(c => c.GetAllUsers()).Returns(users);
            UserService.Setup(c => c.GetUserById(Constants.BackofficeUsers.JohnsMiAsDeveloper.Id)).Returns(Constants.BackofficeUsers.JohnsMiAsDeveloper);
            GroupManager.Setup(c => c.GetUsersInGroup(Constants.BackofficeGroups.Developers.Name)).Returns(new[] { Constants.ADUsers.JohnsMi });
            GroupManager.Setup(c => c.GetUsersInGroup(Constants.BackofficeGroups.Testers.Name)).Returns(new[] { Constants.ADUsers.JohnsMi });

            // Act
            handler.Handle();

            Repo.Verify(r => r.GetAll());
            Repo.Verify(r => r.GetAllUsers());

            GroupManager.Verify(u => u.GetUsersInGroup(It.IsAny<string>()), Times.Exactly(2));
            UserService.Verify(r => r.GetUserById(It.IsAny<int>()), Times.Once);
            Repo.Verify(r => r.DeleteUser(It.IsAny<int>()), Times.Never);
            UserService.Verify(u => u.Delete(It.IsAny<IUser>(), It.IsAny<bool>()), Times.Never);

            UserService.Verify(u => u.CreateUserWithIdentity(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IUserType>()), Times.Never);
            UserService.Verify(u => u.Save(It.IsAny<IUser>(), It.IsAny<bool>()), Times.Never);
        }

        [TestMethod]
        public void Handle_2Groups_DifferentUser_DoesNothing()
        {
            // Arrange
            var handler = new RefreshGroupsHandler(Repo.Object, GroupManager.Object, UserService.Object);
            var users = new List<EasyADGroup2User> {
                new EasyADGroup2User { GroupId = Constants.BackofficeGroups.Developers.Id, UserId = Constants.BackofficeUsers.JohnsMiAsDeveloper.Id },
                new EasyADGroup2User { GroupId = Constants.BackofficeGroups.Testers.Id, UserId = Constants.BackofficeUsers.MuhlbGrAsTester.Id }
            };
            Repo.Setup(c => c.GetAll()).Returns(new[] { Constants.BackofficeGroups.Developers, Constants.BackofficeGroups.Testers });
            Repo.Setup(c => c.GetAllUsers()).Returns(users);
            UserService.Setup(c => c.GetUserById(Constants.BackofficeUsers.JohnsMiAsDeveloper.Id)).Returns(Constants.BackofficeUsers.JohnsMiAsDeveloper);
            UserService.Setup(c => c.GetUserById(Constants.BackofficeUsers.MuhlbGrAsTester.Id)).Returns(Constants.BackofficeUsers.MuhlbGrAsTester);
            GroupManager.Setup(c => c.GetUsersInGroup(Constants.BackofficeGroups.Developers.Name)).Returns(new[] { Constants.ADUsers.JohnsMi });
            GroupManager.Setup(c => c.GetUsersInGroup(Constants.BackofficeGroups.Testers.Name)).Returns(new[] { Constants.ADUsers.MuhlbGr });

            // Act
            handler.Handle();

            Repo.Verify(r => r.GetAll());
            Repo.Verify(r => r.GetAllUsers());

            GroupManager.Verify(u => u.GetUsersInGroup(It.IsAny<string>()), Times.Exactly(2));
            UserService.Verify(r => r.GetUserById(It.IsAny<int>()), Times.Exactly(2));
            Repo.Verify(r => r.DeleteUser(It.IsAny<int>()), Times.Never);
            UserService.Verify(u => u.Delete(It.IsAny<IUser>(), It.IsAny<bool>()), Times.Never);

            UserService.Verify(u => u.CreateUserWithIdentity(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IUserType>()), Times.Never);
            UserService.Verify(u => u.Save(It.IsAny<IUser>(), It.IsAny<bool>()), Times.Never);
        }

        [TestMethod]
        public void Handle_UserAddedToLessRightsGroup_DoesNothingUpdatesUser()
        {
            // Arrange
            var handler = new RefreshGroupsHandler(Repo.Object, GroupManager.Object, UserService.Object);
            var users = new List<EasyADGroup2User> { new EasyADGroup2User { GroupId = Constants.BackofficeGroups.Developers.Id, UserId = Constants.BackofficeUsers.ButleSeAsDeveloper.Id } };
            Repo.Setup(c => c.GetAll()).Returns(new[] { Constants.BackofficeGroups.Developers, Constants.BackofficeGroups.Writers });
            Repo.Setup(c => c.GetAllUsers()).Returns(users);
            UserService.Setup(c => c.GetUserById(Constants.BackofficeUsers.ButleSeAsDeveloper.Id)).Returns(Constants.BackofficeUsers.ButleSeAsDeveloper);
            GroupManager.Setup(c => c.GetUsersInGroup(Constants.BackofficeGroups.Developers.Name)).Returns(new[] { Constants.ADUsers.ButleSe });
            GroupManager.Setup(c => c.GetUsersInGroup(Constants.BackofficeGroups.Writers.Name)).Returns(new[] { Constants.ADUsers.ButleSe });

            // Act
            handler.Handle();

            Repo.Verify(r => r.GetAll());
            Repo.Verify(r => r.GetAllUsers());

            GroupManager.Verify(u => u.GetUsersInGroup(It.IsAny<string>()), Times.Exactly(2));
            UserService.Verify(r => r.GetUserById(It.IsAny<int>()), Times.Once);

            Repo.Verify(r => r.DeleteUser(It.IsAny<int>()), Times.Never);
            UserService.Verify(u => u.Delete(It.IsAny<IUser>(), It.IsAny<bool>()), Times.Never);

            UserService.Verify(u => u.CreateUserWithIdentity(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IUserType>()), Times.Never);
            UserService.Verify(u => u.Save(It.IsAny<IUser>(), It.IsAny<bool>()), Times.Never);
        }

        [TestMethod]
        public void Handle_UserRemovedFromLessRights_DoesNothing()
        {
            // Arrange
            var handler = new RefreshGroupsHandler(Repo.Object, GroupManager.Object, UserService.Object);
            var users = new List<EasyADGroup2User> {
                new EasyADGroup2User { GroupId = Constants.BackofficeGroups.Writers.Id, UserId = Constants.BackofficeUsers.ButleSeAsDeveloper.Id },
                new EasyADGroup2User { GroupId = Constants.BackofficeGroups.Developers.Id, UserId = Constants.BackofficeUsers.ButleSeAsDeveloper.Id },
            };
            Repo.Setup(c => c.GetAll()).Returns(new[] { Constants.BackofficeGroups.Developers, Constants.BackofficeGroups.Writers });
            Repo.Setup(c => c.GetAllUsers()).Returns(users);
            UserService.Setup(c => c.GetUserById(Constants.BackofficeUsers.ButleSeAsDeveloper.Id)).Returns(Constants.BackofficeUsers.ButleSeAsDeveloper);
            GroupManager.Setup(c => c.GetUsersInGroup(Constants.BackofficeGroups.Developers.Name)).Returns(new[] { Constants.ADUsers.ButleSe });

            // Act
            handler.Handle();

            Repo.Verify(r => r.GetAll());
            Repo.Verify(r => r.GetAllUsers());

            GroupManager.Verify(u => u.GetUsersInGroup(It.IsAny<string>()), Times.Exactly(2));
            UserService.Verify(r => r.GetUserById(It.IsAny<int>()), Times.Once);

            Repo.Verify(r => r.DeleteUser(It.IsAny<int>()), Times.Never);
            UserService.Verify(u => u.Delete(It.IsAny<IUser>(), It.IsAny<bool>()), Times.Never);

            UserService.Verify(u => u.CreateUserWithIdentity(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IUserType>()), Times.Never);
            UserService.Verify(u => u.Save(It.IsAny<IUser>(), It.IsAny<bool>()), Times.Never);
        }

        #endregion

        #region Remove User(s) In DB

        [TestMethod]
        public void Handle_RemovedUserInAD_RemovesUser()
        {
            // Arrange
            var handler = new RefreshGroupsHandler(Repo.Object, GroupManager.Object, UserService.Object);
            var users = new List<EasyADGroup2User> { new EasyADGroup2User { GroupId = Constants.BackofficeGroups.Developers.Id, UserId = Constants.BackofficeUsers.JohnsMiAsDeveloper.Id } };
            Repo.Setup(c => c.GetAll()).Returns(new[] { Constants.BackofficeGroups.Developers });
            Repo.Setup(c => c.GetAllUsers()).Returns(users);
            UserService.Setup(c => c.GetUserById(Constants.BackofficeUsers.JohnsMiAsDeveloper.Id)).Returns(Constants.BackofficeUsers.JohnsMiAsDeveloper);

            // Act
            handler.Handle();

            Repo.Verify(r => r.GetAll());
            Repo.Verify(r => r.GetAllUsers());

            GroupManager.Verify(u => u.GetUsersInGroup(Constants.BackofficeGroups.Developers.Name), Times.Once);
            UserService.Verify(r => r.GetUserById(It.IsAny<int>()), Times.Once);
            Repo.Verify(r => r.DeleteUser(It.IsAny<int>()), Times.Once);
            UserService.Verify(u => u.Delete(It.IsAny<IUser>(), It.IsAny<bool>()), Times.Once);

            UserService.Verify(u => u.CreateUserWithIdentity(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IUserType>()), Times.Never);
            UserService.Verify(u => u.Save(It.IsAny<IUser>(), It.IsAny<bool>()), Times.Never);
        }

        [TestMethod]
        public void Handle_RemovedUsersInAD_RemovesUsers()
        {
            // Arrange
            var handler = new RefreshGroupsHandler(Repo.Object, GroupManager.Object, UserService.Object);
            var users = new List<EasyADGroup2User> {
                new EasyADGroup2User { GroupId = Constants.BackofficeGroups.Developers.Id, UserId = Constants.BackofficeUsers.JohnsMiAsDeveloper.Id },
                new EasyADGroup2User { GroupId = Constants.BackofficeGroups.Developers.Id, UserId = Constants.BackofficeUsers.NergeMyAsDeveloper.Id }
            };
            Repo.Setup(c => c.GetAll()).Returns(new[] { Constants.BackofficeGroups.Developers });
            Repo.Setup(c => c.GetAllUsers()).Returns(users);
            UserService.Setup(c => c.GetUserById(Constants.BackofficeUsers.JohnsMiAsDeveloper.Id)).Returns(Constants.BackofficeUsers.JohnsMiAsDeveloper);
            UserService.Setup(c => c.GetUserById(Constants.BackofficeUsers.NergeMyAsDeveloper.Id)).Returns(Constants.BackofficeUsers.NergeMyAsDeveloper);

            // Act
            handler.Handle();

            Repo.Verify(r => r.GetAll());
            Repo.Verify(r => r.GetAllUsers());

            GroupManager.Verify(u => u.GetUsersInGroup(Constants.BackofficeGroups.Developers.Name), Times.Once);
            UserService.Verify(r => r.GetUserById(It.IsAny<int>()), Times.Exactly(2));
            Repo.Verify(r => r.DeleteUser(It.IsAny<int>()), Times.Exactly(2));
            UserService.Verify(u => u.Delete(It.IsAny<IUser>(), It.IsAny<bool>()), Times.Exactly(2));

            UserService.Verify(u => u.CreateUserWithIdentity(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IUserType>()), Times.Never);
            UserService.Verify(u => u.Save(It.IsAny<IUser>(), It.IsAny<bool>()), Times.Never);
        }

        [TestMethod]
        public void Handle_1RemovedUserInAD_RemovesUser()
        {
            // Arrange
            var handler = new RefreshGroupsHandler(Repo.Object, GroupManager.Object, UserService.Object);
            var users = new List<EasyADGroup2User> {
                new EasyADGroup2User { GroupId = Constants.BackofficeGroups.Developers.Id, UserId = Constants.BackofficeUsers.JohnsMiAsDeveloper.Id },
                new EasyADGroup2User { GroupId = Constants.BackofficeGroups.Developers.Id, UserId = Constants.BackofficeUsers.NergeMyAsDeveloper.Id }
            };
            Repo.Setup(c => c.GetAll()).Returns(new[] { Constants.BackofficeGroups.Developers });
            Repo.Setup(c => c.GetAllUsers()).Returns(users);
            UserService.Setup(c => c.GetUserById(Constants.BackofficeUsers.JohnsMiAsDeveloper.Id)).Returns(Constants.BackofficeUsers.JohnsMiAsDeveloper);
            UserService.Setup(c => c.GetUserById(Constants.BackofficeUsers.NergeMyAsDeveloper.Id)).Returns(Constants.BackofficeUsers.NergeMyAsDeveloper);
            GroupManager.Setup(c => c.GetUsersInGroup(Constants.BackofficeGroups.Developers.Name)).Returns(new[] { Constants.ADUsers.NergeMy });

            // Act
            handler.Handle();

            Repo.Verify(r => r.GetAll());
            Repo.Verify(r => r.GetAllUsers());

            GroupManager.Verify(u => u.GetUsersInGroup(Constants.BackofficeGroups.Developers.Name), Times.Once);
            UserService.Verify(r => r.GetUserById(It.IsAny<int>()), Times.Exactly(2));
            Repo.Verify(r => r.DeleteUser(It.IsAny<int>()), Times.Once);
            UserService.Verify(u => u.Delete(It.IsAny<IUser>(), It.IsAny<bool>()), Times.Once);

            UserService.Verify(u => u.CreateUserWithIdentity(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IUserType>()), Times.Never);
            UserService.Verify(u => u.Save(It.IsAny<IUser>(), It.IsAny<bool>()), Times.Never);
        }

        #endregion

        #region Add User(s) From AD

        [TestMethod]
        public void Handle_NewUserInAD_AddsUser()
        {
            // Arrange
            var handler = new RefreshGroupsHandler(Repo.Object, GroupManager.Object, UserService.Object);
            Repo.Setup(c => c.GetAll()).Returns(new[] { Constants.BackofficeGroups.Developers });
            UserService.Setup(c => c.CreateUserWithIdentity(Constants.ADUsers.DavisAn.Login, Constants.ADUsers.DavisAn.Email, Constants.UserTypes.Editor))
                .Returns(new TestUser { Username = Constants.ADUsers.DavisAn.Login, UserType = Constants.UserTypes.Editor, Sections = new List<string>() });
            GroupManager.Setup(c => c.GetUsersInGroup(Constants.BackofficeGroups.Developers.Name)).Returns(new[] { Constants.ADUsers.DavisAn });

            // Act
            handler.Handle();

            Repo.Verify(r => r.GetAll());
            Repo.Verify(r => r.GetAllUsers());

            GroupManager.Verify(u => u.GetUsersInGroup(Constants.BackofficeGroups.Developers.Name), Times.Once);
            UserService.Verify(r => r.GetUserById(It.IsAny<int>()), Times.Never);
            Repo.Verify(r => r.DeleteUser(It.IsAny<int>()), Times.Never);
            UserService.Verify(u => u.Delete(It.IsAny<IUser>(), It.IsAny<bool>()), Times.Never);

            UserService.Verify(u => u.CreateUserWithIdentity(Constants.ADUsers.DavisAn.Login, It.IsAny<string>(), Constants.UserTypes.Editor), Times.Once);
            UserService.Verify(u => u.Save(It.IsAny<IUser>(), It.IsAny<bool>()), Times.Once);
        }

        [TestMethod]
        public void Handle_2NewUsersInAD_AddsUsers()
        {
            // Arrange
            var handler = new RefreshGroupsHandler(Repo.Object, GroupManager.Object, UserService.Object);
            Repo.Setup(c => c.GetAll()).Returns(new[] { Constants.BackofficeGroups.Developers });
            UserService.Setup(c => c.CreateUserWithIdentity(Constants.ADUsers.DavisAn.Login, Constants.ADUsers.DavisAn.Email, Constants.UserTypes.Editor))
                .Returns(new TestUser { Username = Constants.ADUsers.DavisAn.Login, UserType = Constants.UserTypes.Editor, Sections = new List<string>() });
            UserService.Setup(c => c.CreateUserWithIdentity(Constants.ADUsers.MilleDa.Login, Constants.ADUsers.MilleDa.Email, Constants.UserTypes.Editor))
                .Returns(new TestUser { Username = Constants.ADUsers.MilleDa.Login, UserType = Constants.UserTypes.Editor, Sections = new List<string>() });
            GroupManager.Setup(c => c.GetUsersInGroup(Constants.BackofficeGroups.Developers.Name)).Returns(new[] { Constants.ADUsers.DavisAn, Constants.ADUsers.MilleDa });

            // Act
            handler.Handle();

            Repo.Verify(r => r.GetAll());
            Repo.Verify(r => r.GetAllUsers());

            GroupManager.Verify(u => u.GetUsersInGroup(Constants.BackofficeGroups.Developers.Name), Times.Once);
            UserService.Verify(r => r.GetUserById(It.IsAny<int>()), Times.Never);
            Repo.Verify(r => r.DeleteUser(It.IsAny<int>()), Times.Never);
            UserService.Verify(u => u.Delete(It.IsAny<IUser>(), It.IsAny<bool>()), Times.Never);

            UserService.Verify(u => u.CreateUserWithIdentity(Constants.ADUsers.DavisAn.Login, It.IsAny<string>(), Constants.UserTypes.Editor), Times.Once);
            UserService.Verify(u => u.CreateUserWithIdentity(Constants.ADUsers.MilleDa.Login, It.IsAny<string>(), Constants.UserTypes.Editor), Times.Once);
            UserService.Verify(u => u.Save(It.IsAny<IUser>(), It.IsAny<bool>()), Times.Exactly(2));
        }

        [TestMethod]
        public void Handle_1NewUserInAD_AddsUser()
        {
            // Arrange
            var handler = new RefreshGroupsHandler(Repo.Object, GroupManager.Object, UserService.Object);
            var users = new List<EasyADGroup2User> { new EasyADGroup2User { GroupId = Constants.BackofficeGroups.Developers.Id, UserId = Constants.BackofficeUsers.JohnsMiAsDeveloper.Id } };
            Repo.Setup(c => c.GetAll()).Returns(new[] { Constants.BackofficeGroups.Developers });
            Repo.Setup(c => c.GetAllUsers()).Returns(users);
            UserService.Setup(c => c.GetUserById(Constants.BackofficeUsers.JohnsMiAsDeveloper.Id)).Returns(Constants.BackofficeUsers.JohnsMiAsDeveloper);
            UserService.Setup(c => c.CreateUserWithIdentity(Constants.ADUsers.DavisAn.Login, Constants.ADUsers.DavisAn.Email, Constants.UserTypes.Editor))
                .Returns(new TestUser { Username = Constants.ADUsers.DavisAn.Login, UserType = Constants.UserTypes.Editor, Sections = new List<string>() });            
            GroupManager.Setup(c => c.GetUsersInGroup(Constants.BackofficeGroups.Developers.Name)).Returns(new[] { Constants.ADUsers.DavisAn, Constants.ADUsers.JohnsMi });

            // Act
            handler.Handle();

            Repo.Verify(r => r.GetAll());
            Repo.Verify(r => r.GetAllUsers());

            GroupManager.Verify(u => u.GetUsersInGroup(Constants.BackofficeGroups.Developers.Name), Times.Once);
            UserService.Verify(r => r.GetUserById(It.IsAny<int>()), Times.Once);
            Repo.Verify(r => r.DeleteUser(It.IsAny<int>()), Times.Never);
            UserService.Verify(u => u.Delete(It.IsAny<IUser>(), It.IsAny<bool>()), Times.Never);

            UserService.Verify(u => u.CreateUserWithIdentity(Constants.ADUsers.DavisAn.Login, It.IsAny<string>(), Constants.UserTypes.Editor), Times.Once);
            UserService.Verify(u => u.Save(It.IsAny<IUser>(), It.IsAny<bool>()), Times.Exactly(1));
        }

        #endregion

        [TestMethod]
        public void Handle_1NewUserAnd1RemovedInAD_RemovesAndAddsUser()
        {
            // Arrange
            var handler = new RefreshGroupsHandler(Repo.Object, GroupManager.Object, UserService.Object);
            var users = new List<EasyADGroup2User> { new EasyADGroup2User { GroupId = Constants.BackofficeGroups.Developers.Id, UserId = Constants.BackofficeUsers.JohnsMiAsDeveloper.Id } };
            Repo.Setup(c => c.GetAll()).Returns(new[] { Constants.BackofficeGroups.Developers });
            Repo.Setup(c => c.GetAllUsers()).Returns(users);
            UserService.Setup(c => c.GetUserById(Constants.BackofficeUsers.JohnsMiAsDeveloper.Id)).Returns(Constants.BackofficeUsers.JohnsMiAsDeveloper);
            UserService.Setup(c => c.CreateUserWithIdentity(Constants.ADUsers.DavisAn.Login, Constants.ADUsers.DavisAn.Email, Constants.UserTypes.Editor))
                .Returns(new TestUser { Username = Constants.ADUsers.DavisAn.Login, UserType = Constants.UserTypes.Editor, Sections = new List<string>() });
            GroupManager.Setup(c => c.GetUsersInGroup(Constants.BackofficeGroups.Developers.Name)).Returns(new[] { Constants.ADUsers.DavisAn });

            // Act
            handler.Handle();

            Repo.Verify(r => r.GetAll());
            Repo.Verify(r => r.GetAllUsers());

            GroupManager.Verify(u => u.GetUsersInGroup(It.IsAny<string>()), Times.Once);
            UserService.Verify(r => r.GetUserById(It.IsAny<int>()), Times.Once);

            Repo.Verify(r => r.DeleteUser(Constants.BackofficeUsers.JohnsMiAsDeveloper.Id), Times.Once);
            UserService.Verify(u => u.Delete(Constants.BackofficeUsers.JohnsMiAsDeveloper, It.IsAny<bool>()), Times.Once);

            UserService.Verify(u => u.CreateUserWithIdentity(Constants.ADUsers.DavisAn.Login, It.IsAny<string>(), Constants.UserTypes.Editor), Times.Once);
            UserService.Verify(u => u.Save(It.IsAny<IUser>(), It.IsAny<bool>()), Times.Once);
        }

        [TestMethod]
        public void Handle_UserAddedToMoreRightsGroup_UpdatesUser()
        {
            // Arrange
            var handler = new RefreshGroupsHandler(Repo.Object, GroupManager.Object, UserService.Object);
            var users = new List<EasyADGroup2User> { new EasyADGroup2User { GroupId = Constants.BackofficeGroups.Writers.Id, UserId = Constants.BackofficeUsers.BrookJeAsWriter.Id } };
            Repo.Setup(c => c.GetAll()).Returns(new[] { Constants.BackofficeGroups.Developers, Constants.BackofficeGroups.Writers });
            Repo.Setup(c => c.GetAllUsers()).Returns(users);
            UserService.Setup(c => c.GetUserById(Constants.BackofficeUsers.BrookJeAsWriter.Id)).Returns(Constants.BackofficeUsers.BrookJeAsWriter);
            GroupManager.Setup(c => c.GetUsersInGroup(Constants.BackofficeGroups.Developers.Name)).Returns(new[] { Constants.ADUsers.BrookJe });
            GroupManager.Setup(c => c.GetUsersInGroup(Constants.BackofficeGroups.Writers.Name)).Returns(new[] { Constants.ADUsers.BrookJe });

            // Act
            handler.Handle();

            Repo.Verify(r => r.GetAll());
            Repo.Verify(r => r.GetAllUsers());

            GroupManager.Verify(u => u.GetUsersInGroup(It.IsAny<string>()), Times.Exactly(2));
            UserService.Verify(r => r.GetUserById(It.IsAny<int>()), Times.Once);

            Repo.Verify(r => r.DeleteUser(It.IsAny<int>()), Times.Never);
            UserService.Verify(u => u.Delete(It.IsAny<IUser>(), It.IsAny<bool>()), Times.Never);

            UserService.Verify(u => u.CreateUserWithIdentity(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IUserType>()), Times.Never);
            UserService.Verify(u => u.Save(It.IsAny<IUser>(), It.IsAny<bool>()), Times.Once);
        }

        [TestMethod]
        public void Handle_UserRemovedFromMoreRightsGroup_UpdatesUser()
        {
            // Arrange
            var handler = new RefreshGroupsHandler(Repo.Object, GroupManager.Object, UserService.Object);
            var users = new List<EasyADGroup2User> {
                new EasyADGroup2User { GroupId = Constants.BackofficeGroups.Writers.Id, UserId = Constants.BackofficeUsers.ButleSeAsDeveloper.Id },
                new EasyADGroup2User { GroupId = Constants.BackofficeGroups.Developers.Id, UserId = Constants.BackofficeUsers.ButleSeAsDeveloper.Id },
            };
            Repo.Setup(c => c.GetAll()).Returns(new[] { Constants.BackofficeGroups.Developers, Constants.BackofficeGroups.Writers });
            Repo.Setup(c => c.GetAllUsers()).Returns(users);
            UserService.Setup(c => c.GetUserById(Constants.BackofficeUsers.ButleSeAsDeveloper.Id)).Returns(Constants.BackofficeUsers.ButleSeAsDeveloper);
            GroupManager.Setup(c => c.GetUsersInGroup(Constants.BackofficeGroups.Writers.Name)).Returns(new[] { Constants.ADUsers.ButleSe });

            // Act
            handler.Handle();

            Repo.Verify(r => r.GetAll());
            Repo.Verify(r => r.GetAllUsers());

            GroupManager.Verify(u => u.GetUsersInGroup(It.IsAny<string>()), Times.Exactly(2));
            UserService.Verify(r => r.GetUserById(It.IsAny<int>()), Times.Once);

            Repo.Verify(r => r.DeleteUser(It.IsAny<int>()), Times.Never);
            UserService.Verify(u => u.Delete(It.IsAny<IUser>(), It.IsAny<bool>()), Times.Never);

            UserService.Verify(u => u.CreateUserWithIdentity(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IUserType>()), Times.Never);
            UserService.Verify(u => u.Save(It.IsAny<IUser>(), It.IsAny<bool>()), Times.Once);
        }
    }
}
