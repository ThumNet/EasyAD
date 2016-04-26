using FluentScheduler;
using ThumNet.EasyAD.Models;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;

namespace ThumNet.EasyAD.Startup
{
    public class ApplicationEvents : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            CreateTables(applicationContext);

            TaskManager.Initialize(new UpdateEasyADRegistry());
        }

        internal static void CreateTables(ApplicationContext applicationContext)
        {
            //Get the Umbraco Database context
            var ctx = applicationContext.DatabaseContext;
            var db = new DatabaseSchemaHelper(ctx.Database, applicationContext.ProfilingLogger.Logger, ctx.SqlSyntax);

            LogHelper.Info<ApplicationEvents>("Creating EasyAD tables");

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
