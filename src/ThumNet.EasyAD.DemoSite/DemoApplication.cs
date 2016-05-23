using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ThumNet.EasyAD.Managers;
using Umbraco.Core;

namespace ThumNet.EasyAD.DemoSite
{
    public class DemoApplication : ApplicationEventHandler
    {
        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            var manager = new DemoGroupBasedUserManager();
            GroupBasedUserManagerResolver.Current.SetManager(manager);
        }
    }
}