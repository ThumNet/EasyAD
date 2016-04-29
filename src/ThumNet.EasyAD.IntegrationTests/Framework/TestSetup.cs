using Microsoft.VisualStudio.TestTools.UnitTesting;
using Umbraco.Core;

namespace ThumNet.EasyAD.IntegrationTests.Framework
{
    [TestClass]
    public class TestSetup
    {
        #region Test setup/cleanup
        private static TestUmbracoApplication _application;

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            _application = new TestUmbracoApplication();
            _application.Start();

            //Because the ApplicationEventHandler isn't recognized from the Test application call the CreateTables method manually 
            Startup.ApplicationEvents.CreateTables(ApplicationContext.Current);

            //Configure the GroupBasedUserManager to use the TestGroupBasedUserManager instead of the real ActiveDirectory
            Managers.ManagerFactory.SetManager<TestGroupBasedUserManager>();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            if (_application != null)
            {
                _application.Dispose();
            }
        }

        #endregion
    }
}
