using System;
using System.Collections.Generic;
using System.Linq;
using ThumNet.EasyAD.Managers;
using ThumNet.EasyAD.Models;
using Umbraco.Core;

namespace ThumNet.EasyAD.IntegrationTests.Framework
{
    public class TestGroupBasedUserManager : IGroupBasedUserManager
    {
        private Dictionary<string, HashSet<EasyADUser>> _groups;

        public TestGroupBasedUserManager()
        {
            _groups = new Dictionary<string, HashSet<EasyADUser>>();
        }

        public bool CheckPassword(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EasyADUser> GetUsersInGroup(string groupName)
        {
            if (GroupExists(groupName))
            {
                return _groups[groupName];
            }
            return Enumerable.Empty<EasyADUser>();
        }

        public bool GroupExists(string groupName)
        {
            return _groups.ContainsKey(groupName);
        }

        internal void AddUsers(string groupName, params EasyADUser[] users)
        {
            if (!_groups.ContainsKey(groupName))
            {
                _groups.Add(groupName, new HashSet<EasyADUser>());
            }
            users.ToList().ForEach(u => _groups[groupName].Add(u));
        }

        internal void RemoveUsers(string groupName, params string[] logins)
        {
            if (_groups.ContainsKey(groupName))
            {
                logins.ToList().ForEach(l => _groups[groupName].RemoveWhere(u => u.Login.InvariantEquals(l)));
            }
        }

        internal HashSet<EasyADUser> this[string groupName]
        { 
            get
            {
                return _groups[groupName];
            }
            set
            {
                _groups[groupName] = value;
            }
        }

        internal void RemoveGroup(string groupName)
        {
            _groups.Remove(groupName);
        }
    }
}