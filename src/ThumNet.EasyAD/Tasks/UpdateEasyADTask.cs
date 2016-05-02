using FluentScheduler;
using System.Web.Hosting;
using ThumNet.EasyAD.Handlers;
using ThumNet.EasyAD.Managers;
using ThumNet.EasyAD.Repositories;
using Umbraco.Core;

namespace ThumNet.EasyAD.Tasks
{
    public class UpdateEasyADTask : ITask, IRegisteredObject
    {
        private readonly object _lock = new object();
        private bool _shuttingDown;

        public UpdateEasyADTask()
        {
            // Register this task with the hosting environment.
            // Allows for a more graceful stop of the task, in the case of IIS shutting down.
            HostingEnvironment.RegisterObject(this);
        }

        public void Execute()
        {
            lock (_lock)
            {
                if (_shuttingDown)
                {
                    return;
                }

                var appContext = ApplicationContext.Current;
                var repo = new EasyADRepository(appContext.DatabaseContext.Database, appContext.DatabaseContext.SqlSyntax);
                var groupManager = new ActiveDirectoryManager();
                var handler = new RefreshGroupsHandler(repo, groupManager, appContext.Services.UserService);
                handler.Handle();
            }
        }

        public void Stop(bool immediate)
        {
            // Locking here will wait for the lock in Execute to be released until this code can continue.
            lock (_lock)
            {
                _shuttingDown = true;
            }

            HostingEnvironment.UnregisterObject(this);
        }
    }
}
