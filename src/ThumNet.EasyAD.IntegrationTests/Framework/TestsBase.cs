using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThumNet.EasyAD.Repositories;
using Umbraco.Core;
using Umbraco.Core.Services;

namespace ThumNet.EasyAD.IntegrationTests.Framework
{
    public class TestsBase
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

        protected TestGroupBasedUserManager Manager
        {
            get
            {
                return Managers.ManagerFactory.GetManager() as TestGroupBasedUserManager;
            }
        }

        private IEasyADRepository _repo = null;
        protected IEasyADRepository Repo
        {
            get
            {
                return _repo ?? (_repo = new EasyADRepository(ApplicationContext.Current.DatabaseContext.Database, ApplicationContext.Current.DatabaseContext.SqlSyntax));
            }
        }

        protected ServiceContext Services
        {
            get
            {
                return ApplicationContext.Current.Services;
            }
        }

        #endregion
    }
}
