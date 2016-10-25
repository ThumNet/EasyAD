using System.Linq;
using ThumNet.EasyAD.Managers;
using ThumNet.EasyAD.Repositories;
using Umbraco.Core.Logging;
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
            LogHelper.Info<DeleteGroupHandler>(string.Format("Removing group {0} started", groupId));

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
                    LogHelper.Info<DeleteGroupHandler>(string.Format("Removing user '{0}' because he/she is in no other group", backofficeUser.Name));

                    // See: https://our.umbraco.org/forum/getting-started/questions-about-runway-and-modules/8848-Deleting-Users
                    _userService.Delete(backofficeUser, deletePermanently: false);
                }
                else
                {
                    // Update the sections and the usertype based on the remaining groups
                    ConfigureUserRights(backofficeUser, groupsUserIsIn);
                }
            }

            var rowCount = _repo.DeleteGroup(groupId);

            LogHelper.Info<DeleteGroupHandler>(string.Format("Removing group {0} completed", groupId));
            return rowCount;
        }
    }
}
