using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThumNet.EasyAD.Models;
using ThumNet.EasyAD.Resolvers;
using System.Collections.Generic;

namespace ThumNet.EasyAD.UnitTests.Resolvers
{
    [TestClass]
    public class SectionResolverTests
    {
        [TestMethod]
        public void Resolve_OneGroup()
        {
            //Arrange
            var group1 = new EasyADGroup { Id = 1, Name = "Group A", Sections = "content;media;", UserType = 1 };
            var groups = new List<EasyADGroup> { group1 };
            var expected = new[] { "content", "media" };

            //Act
            var actual = SectionResolver.Resolve(groups);

            //Assert
            Assert.AreEqual(2, actual.Length);             
        }

        [TestMethod]
        public void Resolve_TwoGroups()
        {
            //Arrange
            var group1 = new EasyADGroup { Id = 1, Name = "Group A", Sections = "content;media;", UserType = 1 };
            var group2 = new EasyADGroup { Id = 2, Name = "Group B", Sections = "translation;", UserType = 3 };
            var groups = new List<EasyADGroup> { group1, group2 };
            var expected = new[] { "content", "media", "translation" };

            //Act
            var actual = SectionResolver.Resolve(groups);

            //Assert
            Assert.AreEqual(3, actual.Length);
            Assert.IsTrue(expected.All(e => actual.Contains(e)));
        }

        [TestMethod]
        public void Resolve_MultipleGroups()
        {
            //Arrange
            var group1 = new EasyADGroup { Id = 1, Name = "Group A", Sections = "content;media;", UserType = 1 };
            var group2 = new EasyADGroup { Id = 2, Name = "Group B", Sections = "translation;", UserType = 3 };
            var group3 = new EasyADGroup { Id = 3, Name = "Group B", Sections = "content;media;users;members;", UserType = 2 };
            var groups = new List<EasyADGroup> { group1, group2, group3 };
            var expected = new[] { "content", "media", "translation", "users", "members" };

            //Act
            var actual = SectionResolver.Resolve(groups);

            //Assert
            Assert.AreEqual(5, actual.Length);
            Assert.IsTrue(expected.All(e => actual.Contains(e)));
        }
    }
}
