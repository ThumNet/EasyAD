using System.Collections.Generic;
using ThumNet.EasyAD.Models;

namespace ThumNet.EasyAD.Repositories
{
    public interface IEasyADRepository
    {
        void AddUserToGroup(int groupId, int backofficeUserId);
        int DeleteGroup(int id);
        void DeleteUser(int id);
        void DeleteGroupUsers(int groupId);
        IEnumerable<EasyADGroup> GetAll();
        IEnumerable<EasyADGroup2User> GetAllUsers();
        IEnumerable<EasyADGroup2User> GetUsersInGroup(int groupId);
        EasyADGroup GetById(int id);
        IEnumerable<EasyADGroup> GetGroupsForUser(int userId);
        void SetGroupsForUser(int userId, IEnumerable<int> groupIds);
        void SaveOrUpdate(EasyADGroup group);        
    }
}