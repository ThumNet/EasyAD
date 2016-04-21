using System.Collections.Generic;
using System.Linq;
using ThumNet.EasyAD.Managers;
using ThumNet.EasyAD.Models;
using ThumNet.EasyAD.Repositories;
using Umbraco.Core.Models.Membership;
using Umbraco.Core.Services;

namespace ThumNet.EasyAD.Handlers
{
    public class RefreshGroupsHandler : GroupHandler
    {
        public RefreshGroupsHandler(IEasyADRepository repo, IGroupBasedUserManager groupManager, IUserService userService) : base(repo, groupManager, userService)
        {
        }

        public void Handle()
        {
            // TODO: test all code below!!!


            // 1) get the groups from db
            var groups = _repo.GetAll();

            // 2) get the users for each group from db
            var usersPerGroup = _repo.GetAllUsers();

            // 3) query the users in ad for each group (step 1)
            var adGroupedUsers = new Dictionary<int, IEnumerable<EasyADUser>>();
            foreach (var group in groups)
            {
                adGroupedUsers.Add(group.Id, _groupManager.GetUsersInGroup(group.Name));
            }

            var adUsers = GetUniqueAdUsers(adGroupedUsers);
            var backofficeUsers = GetUniqueBackofficeUsers(usersPerGroup).ToList();

            var deleteUsers = backofficeUsers.Select(u => u.Username).Except(adUsers.Select(u => u.Login)).ToList();
            var newUsers = adUsers.Select(u => u.Login).Except(backofficeUsers.Select(u => u.Username)).ToList();

            // Delete the users that aren't any group anymore
            foreach (var name in deleteUsers)
            {
                var user = backofficeUsers.First(u => u.Username == name);
                _repo.DeleteUser(user.Id);
                _userService.Delete(user, true); // TODO: remove deletePermanently
                backofficeUsers.Remove(user);

                usersPerGroup = usersPerGroup.Where(u => u.UserId != user.Id);
            }

            // Create the users that aren't in the backoffice yet
            foreach (var name in newUsers)
            {
                var user = adUsers.First(u => u.Login == name);
                var firstGroup = groups.Single(g => g.Id == adGroupedUsers.First(a => a.Value.Contains(user)).Key);
                var backofficeUser = _userService.CreateUserWithIdentity(user.Login, user.Email, _userService.GetUserTypeById(firstGroup.UserType));
                backofficeUser.Name = user.DiplayName;
                
                backofficeUsers.Add(backofficeUser);
            }

            // Update the current users
            foreach (var user in backofficeUsers)
            {
                var groupsUserIsIn = adGroupedUsers.Where(a => a.Value.Any(u => u.Login == user.Username)).Select(kvp => groups.First(g => g.Id == kvp.Key));
                ConfigureUserRights(user, groupsUserIsIn);
            }
        }

        private IEnumerable<EasyADUser> GetUniqueAdUsers(Dictionary<int, IEnumerable<EasyADUser>> adGroupedUsers)
        {
            return adGroupedUsers
                    .Where(a => a.Value != null)
                    .SelectMany(a => a.Value)
                    .GroupBy(user => user.Login)
                    .Select(group => group.First());
        }

        private IEnumerable<IUser> GetUniqueBackofficeUsers(IEnumerable<EasyADGroup2User> usersPerGroup)
        {
            return usersPerGroup
                .Select(g => g.UserId)
                .Distinct()
                .Select(id => _userService.GetUserById(id));
        }
    }
}
