using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using ThumNet.EasyAD.Handlers;
using ThumNet.EasyAD.Managers;
using ThumNet.EasyAD.Repositories;
using Umbraco.Core.Services;

namespace ThumNet.EasyAD.UnitTests.Handlers
{
    public abstract class BaseHandlerTest
    {
        protected Mock<IEasyADRepository> Repo;
        protected Mock<IGroupBasedUserManager> GroupManager;
        protected Mock<IUserService> UserService;

        [TestInitialize]
        public void Init()
        {
            Repo = new Mock<IEasyADRepository>();
            GroupManager = new Mock<IGroupBasedUserManager>();
            UserService = new Mock<IUserService>();

            UserService
                .Setup(u => u.GetAllUserTypes())
                .Returns(TestData.Constants.UserTypes.All);
            UserService
                .Setup(u => u.GetUserTypeById(1))
                .Returns(TestData.Constants.UserTypes.Administrator);
            UserService
                .Setup(u => u.GetUserTypeById(2))
                .Returns(TestData.Constants.UserTypes.Editor);
            UserService
                .Setup(u => u.GetUserTypeById(3))
                .Returns(TestData.Constants.UserTypes.Writer);
            UserService
                .Setup(u => u.GetUserTypeById(4))
                .Returns(TestData.Constants.UserTypes.Translator);
        }
    }
}
