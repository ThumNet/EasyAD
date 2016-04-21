using System;
using System.Collections.Generic;
using System.Linq;
using ThumNet.EasyAD.Models;

namespace ThumNet.EasyAD.Managers
{
    internal class SampleGroupBasedManager : IGroupBasedUserManager
    {
        #region Data
        private List<SampleGroup> _groups = new List<SampleGroup> {
            new SampleGroup
            {
                Name = "Developers",
                Users = new List<EasyADUser>
                {
                    new EasyADUser { Login = "tuje", DiplayName = "Tummers, Jeffrey", Email = "tuje@ad.local" },
                    new EasyADUser { Login = "josc", DiplayName = "Schermers, Joost", Email = "josc@ad.local" },
                    new EasyADUser { Login = "stkl", DiplayName = "Steeg van, Klaas", Email = "stkl@ad.local" },
                    new EasyADUser { Login = "wjoo", DiplayName = "Wassing, Joost", Email = "wjoo@ad.local" },
                }
            },
            new SampleGroup
            {
                Name = "Ops",
                Users = new List<EasyADUser>
                {
                    new EasyADUser { Login = "goge", DiplayName = "Goudriaan, Gerard", Email = "goge@ad.local" },
                    new EasyADUser { Login = "knni", DiplayName = "Knaaken, Nick", Email = "knni@ad.local" },
                }
            },
            new SampleGroup
            {
                Name = "Testers",
                Users = new List<EasyADUser>
                {
                    new EasyADUser { Login = "broe", DiplayName = "Broeder den, Rob", Email = "broe@ad.local" },
                    new EasyADUser { Login = "satj", DiplayName = "Doedens-van de Sande, Tanja", Email = "satj@ad.local" },
                }
            },
            new SampleGroup
            {
                Name = "Team",
                Users = new List<EasyADUser>
                {
                    new EasyADUser { Login = "tuje", DiplayName = "Tummers, Jeffrey", Email = "tuje@ad.local" },
                    new EasyADUser { Login = "josc", DiplayName = "Schermers, Joost", Email = "josc@ad.local" },
                    new EasyADUser { Login = "stkl", DiplayName = "Steeg van, Klaas", Email = "stkl@ad.local" },
                    new EasyADUser { Login = "wjoo", DiplayName = "Wassing, Joost", Email = "wjoo@ad.local" },
                    new EasyADUser { Login = "broe", DiplayName = "Broeder den, Rob", Email = "broe@ad.local" },
                    new EasyADUser { Login = "satj", DiplayName = "Doedens-van de Sande, Tanja", Email = "satj@ad.local" },
                    new EasyADUser { Login = "goge", DiplayName = "Goudriaan, Gerard", Email = "goge@ad.local" },
                }
            },
        };
        
        #endregion

        public IEnumerable<EasyADUser> GetUsersInGroup(string groupName)
        {
            if (!GroupExists(groupName))
            {
                return null;
            }

            return _groups.First(g => g.Name == groupName).Users;
        }

        public bool GroupExists(string groupName)
        {
            return _groups.Any(g => g.Name == groupName);
        }

        public bool CheckPassword(string userName, string password)
        {
            var uniqueUsers = _groups.SelectMany(g => g.Users).Select(u => u.Login).Distinct();
            return uniqueUsers.Contains(userName) && password == "Test123";
        }

        internal class SampleGroup
        {
            public string Name { get; set; }
            public List<EasyADUser> Users { get; set; }
        }
    }
}
