using Owin;
using Umbraco.Web;
using Umbraco.Web.Security.Identity;
using Umbraco.Core;
using Umbraco.Core.Models.Identity;
using Umbraco.Core.Security;
using ThumNet.EasyAD.Managers;

namespace ThumNet.EasyAD.Startup
{
    public class UmbracoEasyADOwinStartup : UmbracoDefaultOwinStartup
    {
        public override void Configuration(IAppBuilder app)
        {
            app.SetUmbracoLoggerFactory();

            var appContext = ApplicationContext.Current;

            app.ConfigureUserManagerForUmbracoBackOffice<BackOfficeUserManager, BackOfficeIdentityUser>(
                appContext,
                (options, context) =>
                {
                    var membershipProvider = MembershipProviderExtensions.GetUsersMembershipProvider().AsUmbracoMembershipProvider();
                    var store = new BackOfficeUserStore(appContext.Services.UserService, appContext.Services.ExternalLoginService, membershipProvider);
                    return UmbracoEasyADUserManager.InitUserManager(store, membershipProvider, options);
                });

            app.UseUmbracoBackOfficeCookieAuthentication(appContext)
               .UseUmbracoBackOfficeExternalCookieAuthentication(appContext);
        }
    }
}
