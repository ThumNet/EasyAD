using System.Collections.Generic;
using ThumNet.EasyAD.Models;

namespace ThumNet.EasyAD.Managers
{
    public interface IGroupBasedUserManager
    {
        bool GroupExists(string groupName);

        IEnumerable<EasyADUser> GetUsersInGroup(string groupName);

        bool CheckPassword(string userName, string password);
    }
}
