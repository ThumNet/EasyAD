using System.Linq;
using ThumNet.EasyAD.Managers;
using ThumNet.EasyAD.Models;
using ThumNet.EasyAD.Repositories;
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
            _repo.SaveOrUpdate(group);
            _repo.DeleteGroupUsers(group.Id);

            var groupUsers = _groupManager.GetUsersInGroup(group.Name);
            foreach (var groupUser in groupUsers)
            {
                var backofficeUser = _userService.GetByUsername(groupUser.Login);
                if (backofficeUser == null)
                {
                    // create the backoffice user
                    backofficeUser = _userService.CreateUserWithIdentity(groupUser.Login, groupUser.Email, _userService.GetUserTypeById(group.UserType));
                    backofficeUser.Name = groupUser.DiplayName;
                }

                _repo.AddUserToGroup(group.Id, backofficeUser.Id);
                var groupsUserIsIn = _repo.GetGroupsForUser(backofficeUser.Id).ToList();

                ConfigureUserRights(backofficeUser, groupsUserIsIn);
            }

            return group.Id;
        }
    }
}
