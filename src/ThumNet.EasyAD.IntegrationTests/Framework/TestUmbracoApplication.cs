using System;
using Umbraco.Core;
using System.IO;
using System.Linq;
using Umbraco.Core.Persistence;
using System.Data.SqlServerCe;
using System.Diagnostics;

namespace ThumNet.EasyAD.IntegrationTests
{
    /// <summary>
    /// Extends the UmbracoApplicationBase, which is needed to start the application with our own bootmanager.
    /// <see cref="https://github.com/sitereactor/umbraco-console-example/blob/master/UmbConsole/ConsoleApplicationBase.cs"/>
    /// </summary>
    public class TestUmbracoApplication : UmbracoApplicationBase
    {
        private const string DemoSiteFolderName = "ThumNet.EasyAD.DemoSite";

        public string DataDirectory { get; private set; }

        protected override IBootManager GetBootManager()
        {
            var binDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            DataDirectory = Path.Combine(binDirectory.FullName, "App_Data");

            CopySiteConfigFiles(binDirectory);

            AppDomain.CurrentDomain.SetData("DataDirectory", DataDirectory);

            return new TestBootManager(this, binDirectory.FullName);
        }

        protected override void OnApplicationStarting(object sender, EventArgs e)
        {
            base.OnApplicationStarting(sender, e);
            CreateDatabase();
        }

        public void Start()
        {
            Application_Start(this, new EventArgs());
        }

        private void CreateDatabase()
        {
            var context = ApplicationContext.Current;
            var databaseProvider = context.DatabaseContext.DatabaseProvider;
            var dataDirectory = DataDirectory;
            var db = new DatabaseSchemaHelper(context.DatabaseContext.Database, context.ProfilingLogger.Logger, context.DatabaseContext.SqlSyntax);

            if (databaseProvider == DatabaseProviders.SqlServerCE)
            {
                var dbPath = Path.Combine(dataDirectory, "Umbraco.sdf");
                if (File.Exists(dbPath))
                {
                    File.Delete(dbPath);
                }
                var engine = new SqlCeEngine(@"Data Source=|DataDirectory|\Umbraco.sdf;Flush Interval=1;");
                engine.CreateDatabase();
            }

            db.CreateDatabaseSchema(false, context);

            Debug.WriteLine("The database schema has been installed");
            Debug.WriteLine("Note: This is just an example, so no backoffice user has been created.");
        }

        private void CopySiteConfigFiles(DirectoryInfo binDir)
        {
            var sitePath = ResolveSitePath(binDir);
            var binPath = binDir.FullName;
            var appDomainConfigPath = new DirectoryInfo(Path.Combine(binPath, "config"));

            //Copy config files to AppDomain's base directory
            if (binPath.Equals(sitePath) == false &&
                appDomainConfigPath.Exists == false)
            {
                appDomainConfigPath.Create();
                var siteConfigPath = new DirectoryInfo(Path.Combine(sitePath, "config"));
                var sourceFiles = siteConfigPath.GetFiles("*.config", SearchOption.TopDirectoryOnly);
                foreach (var sourceFile in sourceFiles)
                {
                    sourceFile.CopyTo(sourceFile.FullName.Replace(siteConfigPath.FullName, appDomainConfigPath.FullName), true);
                }
            }
        }

        private string ResolveSitePath(DirectoryInfo currentFolder)
        {
            var folders = currentFolder.GetDirectories();
            var siteFolder = folders.FirstOrDefault(x => x.Name.InvariantEquals(DemoSiteFolderName));
            if (siteFolder != null)
            {
                folders = siteFolder.GetDirectories();
                if (folders.Any(x => x.Name.InvariantEquals("app_data")) &&
                    folders.Any(x => x.Name.InvariantEquals("config")))
                {
                    return siteFolder.FullName;
                }
            }

            if (currentFolder.Parent == null)
            {
                throw new Exception("Base directory containing an 'App_Data' and 'Config' folder was not found." +
                    " These folders are required to run this console application as it relies on the normal umbraco configuration files.");
            }

            return ResolveSitePath(currentFolder.Parent);
        }
    }
}
