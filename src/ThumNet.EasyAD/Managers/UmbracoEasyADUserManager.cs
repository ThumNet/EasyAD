using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Umbraco.Core.Models.Identity;
using Umbraco.Core.Security;

namespace ThumNet.EasyAD.Managers
{
    internal class UmbracoEasyADUserManager : BackOfficeUserManager
    {
        public UmbracoEasyADUserManager(IUserStore<BackOfficeIdentityUser, int> store) 
            : base(store)
        {
        }

        public async override Task<bool> CheckPasswordAsync(BackOfficeIdentityUser user, string password)
        {
            var groupManager = new ActiveDirectoryManager();

            // First check the user against Active Directory
            bool validLogin = groupManager.CheckPassword(user.UserName, password);

            if (!validLogin)
            {
                // Now check the Umbraco Backoffice user manager
                validLogin = await base.CheckPasswordAsync(user, password);
            }

            return validLogin;
        }

        internal static BackOfficeUserManager InitUserManager(BackOfficeUserStore store, UmbracoMembershipProviderBase membershipProvider, IdentityFactoryOptions<BackOfficeUserManager> options)
        {
            var manager = new UmbracoEasyADUserManager(store);
            
            // Just use the InitUserManager method in Umbraco!
            manager.InitUserManager(manager, membershipProvider, options);

            return manager;
        }
    }
}