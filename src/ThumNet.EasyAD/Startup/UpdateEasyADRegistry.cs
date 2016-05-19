using FluentScheduler;
using ThumNet.EasyAD.Configuration;
using ThumNet.EasyAD.Tasks;

namespace ThumNet.EasyAD.Startup
{
    public class UpdateEasyADRegistry : Registry
    {
        public UpdateEasyADRegistry(Config config)
        {
            Schedule<UpdateEasyADTask>().ToRunEvery(config.SyncInterval).Minutes();
        }
    }
}
