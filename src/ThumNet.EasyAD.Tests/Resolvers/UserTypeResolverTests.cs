using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThumNet.EasyAD.Models;
using ThumNet.EasyAD.Resolvers;
using System.Collections.Generic;
using Umbraco.Core.Models.Membership;
using Moq;

namespace ThumNet.EasyAD.UnitTests.Resolvers
{
    [TestClass]
    public class UserTypeResolverTests
    {
        private List<IUserType> _allUserTypes = new List<IUserType>();

        public UserTypeResolverTests()
        {
            var type1Mock = new Mock<IUserType>();
            type1Mock.SetupAllProperties();
            type1Mock.Object.Id = 1;
            type1Mock.Object.Alias = "writer";
            type1Mock.Object.Name = "Writer";
            type1Mock.Object.Permissions = new List<string> { "Browse Node", "Create", "Send To Publish", "Update" };
            _allUserTypes.Add(type1Mock.Object);
            
            var type2Mock = new Mock<IUserType>();
            type2Mock.SetupAllProperties();
            type2Mock.Object.Id = 2;
            type2Mock.Object.Alias = "editor";
            type2Mock.Object.Name = "Editors";
            type2Mock.Object.Permissions = new List<string> { "Audit Trail", "Browse Node", "Copy", "Delete", "Move", "Create", "Public access", "Publish", "Rollback", "Send To Translation", "Sort", "Update" };
            _allUserTypes.Add(type2Mock.Object);

            var type3Mock = new Mock<IUserType>();
            type3Mock.SetupAllProperties();
            type3Mock.Object.Id = 3;
            type3Mock.Object.Alias = "translator";
            type3Mock.Object.Alias = "Translator";
            type3Mock.Object.Permissions = new List<string> { "Browse Node", "Update" };
            _allUserTypes.Add(type3Mock.Object);
        }
        
        [TestMethod]
        public void Resolve_OneType()
        {
            //Arrange
            var group1 = new EasyADGroup { Id = 1, Name = "Group A", Sections = "content;media;", UserType = 1 };
            var groups = new List<EasyADGroup> { group1 };
            var expected = 1;

            //Act
            var actual = UserTypeResolver.Resolve(groups, _allUserTypes).Id;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Resolve_TwoGroups()
        {
            //Arrange
            var group1 = new EasyADGroup { Id = 1, Name = "Group A", Sections = "content;media;", UserType = 1 };
            var group2 = new EasyADGroup { Id = 2, Name = "Group B", Sections = "translation;", UserType = 3 };
            var groups = new List<EasyADGroup> { group1, group2 };
            var expected = 1;

            //Act
            var actual = UserTypeResolver.Resolve(groups, _allUserTypes).Id;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Resolve_MultipleGroups()
        {
            //Arrange
            var group1 = new EasyADGroup { Id = 1, Name = "Group A", Sections = "content;media;", UserType = 1 };
            var group2 = new EasyADGroup { Id = 2, Name = "Group B", Sections = "translation;", UserType = 3 };
            var group3 = new EasyADGroup { Id = 3, Name = "Group B", Sections = "content;media;users;members;", UserType = 2 };
            var groups = new List<EasyADGroup> { group1, group2, group3 };
            var expected = 2;

            //Act
            var actual = UserTypeResolver.Resolve(groups, _allUserTypes).Id;

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
