using System.Linq;
using ThumNet.EasyAD.Managers;
using ThumNet.EasyAD.Repositories;
using Umbraco.Core.Services;

namespace ThumNet.EasyAD.Handlers
{
    public class DeleteGroupHandler : GroupHandler
    {
        public DeleteGroupHandler(IEasyADRepository repo, IGroupBasedUserManager groupManager, IUserService userService) : base(repo, groupManager, userService)
        {
        }

        public int Handle(int groupId)
        {                       
            var groupUsers = _repo.GetUsersInGroup(groupId);
            _repo.DeleteGroupUsers(groupId);
            foreach (var groupUser in groupUsers)
            {
                var backofficeUser = _userService.GetUserById(groupUser.UserId);
                if (backofficeUser == null)
                {
                    continue;
                }

                var groupsUserIsIn = _repo.GetGroupsForUser(backofficeUser.Id).ToList();
                if (groupsUserIsIn.Count == 0)
                {
                    // user is no longer needed
                    // See: https://our.umbraco.org/forum/getting-started/questions-about-runway-and-modules/8848-Deleting-Users
                    _userService.Delete(backofficeUser, deletePermanently: true); // TODO: remove deletePermanently
                }
                else
                {
                    // Update the sections and the usertype based on the remaining groups
                    ConfigureUserRights(backofficeUser, groupsUserIsIn);                    
                }
            }

            var rowCount = _repo.DeleteGroup(groupId);
            

            return rowCount;
        }
    }
}
