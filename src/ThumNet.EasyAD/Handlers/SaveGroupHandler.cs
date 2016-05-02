using System.Linq;
using ThumNet.EasyAD.Managers;
using ThumNet.EasyAD.Models;
using ThumNet.EasyAD.Repositories;
using Umbraco.Core.Logging;
using Umbraco.Core.Services;

namespace ThumNet.EasyAD.Handlers
{
    public class SaveGroupHandler : GroupHandler
    {
        public SaveGroupHandler(IEasyADRepository repo, IGroupBasedUserManager groupManager, IUserService userService) : base(repo, groupManager, userService)
        {
        }

        public int Handle(EasyADGroup group)
        {
            LogHelper.Info<DeleteGroupHandler>(string.Format("Saving group {0} started", group.Name));

            _repo.SaveOrUpdate(group);
            _repo.DeleteGroupUsers(group.Id);

            var groupUsers = _groupManager.GetUsersInGroup(group.Name);           
            foreach (var groupUser in groupUsers)
            {
                var backofficeUser = _userService.GetByUsername(groupUser.Login);
                if (backofficeUser == null)
                {
                    // create the backoffice user
                    LogHelper.Info<DeleteGroupHandler>(string.Format("Creating user '{0}' in backoffice", groupUser.DiplayName));
                    backofficeUser = _userService.CreateUserWithIdentity(groupUser.Login, groupUser.Email, _userService.GetUserTypeById(group.UserType));                    
                }
                backofficeUser.Name = groupUser.DiplayName;

                _repo.AddUserToGroup(group.Id, backofficeUser.Id);
                var groupsUserIsIn = _repo.GetGroupsForUser(backofficeUser.Id).ToList();

                ConfigureUserRights(backofficeUser, groupsUserIsIn);                
            }

            LogHelper.Info<DeleteGroupHandler>(string.Format("Saving group '{0}' completed", group.Name));

            return group.Id;
        }
    }
}
