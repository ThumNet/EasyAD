using FluentScheduler;
using ThumNet.EasyAD.Configuration;
using ThumNet.EasyAD.Managers;
using ThumNet.EasyAD.Models;
using Umbraco.Core;
using Umbraco.Core.Configuration;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;

namespace ThumNet.EasyAD.Startup
{
    /// <summary>
    /// Installs EasyAD into the Umbraco site.
    /// </summary>
    public class EasyADApplication : ApplicationEventHandler
    {
        protected override void ApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            GroupBasedUserManagerResolver.Current = new GroupBasedUserManagerResolver(new ActiveDirectoryManager());
        }

        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
        }

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            var config = UmbracoConfig.For.EasyAD();
            if (!config.Enable)
                return;

            CreateTables(applicationContext);
            TaskManager.Initialize(new UpdateEasyADRegistry(config));            
        }

        internal static void CreateTables(ApplicationContext applicationContext)
        {
            //Get the Umbraco Database context
            var ctx = applicationContext.DatabaseContext;
            var db = new DatabaseSchemaHelper(ctx.Database, applicationContext.ProfilingLogger.Logger, ctx.SqlSyntax);

            LogHelper.Info<EasyADApplication>("Creating EasyAD tables");

            //Check if the DB table does NOT exist
            if (!db.TableExist(AppConstants.TableNames.EasyADGroups))
            {
                //Create DB table - and set overwrite to false
                db.CreateTable<EasyADGroup>(false);
            }

            //Check if the DB table does NOT exist
            if (!db.TableExist(AppConstants.TableNames.EasyADGroup2Users))
            {
                //Create DB table - and set overwrite to false
                db.CreateTable<EasyADGroup2User>(false);
            }
        }
    }
}
