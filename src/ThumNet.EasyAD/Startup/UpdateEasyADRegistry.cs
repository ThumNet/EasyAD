using FluentScheduler;
using ThumNet.EasyAD.Tasks;

namespace ThumNet.EasyAD.Startup
{
    public class UpdateEasyADRegistry : Registry
    {
        public UpdateEasyADRegistry()
        {
            Schedule<UpdateEasyADTask>().ToRunEvery(15).Minutes();
        }
    }
}
