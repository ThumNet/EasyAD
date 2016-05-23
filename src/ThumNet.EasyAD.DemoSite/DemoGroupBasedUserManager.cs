using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ThumNet.EasyAD.Managers;
using ThumNet.EasyAD.Models;

namespace ThumNet.EasyAD.DemoSite
{
    public class DemoGroupBasedUserManager : IGroupBasedUserManager
    {
        #region _groupsJson
        private const string _groupsJson = @"[
  {
    'Name': 'AD-Writers',
    'Users': [
      {
        'Login': 'DavisAn',
        'DisplayName': 'Andrew Davis',
        'Email': 'DavisAn@test.local'
      },
      {
        'Login': 'MilleDa',
        'DisplayName': 'Daniel Miller',
        'Email': 'MilleDa@test.local'
      }
    ]
  },
  {
    'Name': 'AD-Editors',
    'Users': [
      {
        'Login': 'DavisAn',
        'DisplayName': 'Andrew Davis',
        'Email': 'DavisAn@test.local'
      },
      {
        'Login': 'MooreCh',
        'DisplayName': 'Christopher Moore',
        'Email': 'MooreCh@test.local'
      }
    ]
  },
  {
    'Name': 'AD-Translators',
    'Users': [
      {
        'Login': 'MooreCh',
        'DisplayName': 'Christopher Moore',
        'Email': 'MooreCh@test.local'
      },
      {
        'Login': 'ButleSe',
        'DisplayName': 'Seth Butler',
        'Email': 'ButleSe@test.local'
      }
    ]
  },
  {
    'Name': 'AD-Admins',
    'Users': [
      {
        'Login': 'ButleSe',
        'DisplayName': 'Seth Butler',
        'Email': 'ButleSe@test.local'
      },
      {
        'Login': 'MuhlbGr',
        'DisplayName': 'Gracie Muhlberg',
        'Email': 'MuhlbGr@test.local'
      },
      {
        'Login': 'JohnsMi',
        'DisplayName': 'Michael Johnson',
        'Email': 'JohnsMi@test.local'
      }
    ]
  }
]";
        #endregion

        private List<DemoGroup> _groups;

        private List<DemoGroup> Groups
        {
            get
            {
                if (_groups == null)
                {
                    _groups = JsonConvert.DeserializeObject<List<DemoGroup>>(_groupsJson);
                }
                return _groups;
            }
        }

        public bool CheckPassword(string userName, string password)
        {
            return false;
        }

        public IEnumerable<EasyADUser> GetUsersInGroup(string groupName)
        {
            return Groups.First(g => g.Name == groupName).Users;
        }

        public bool GroupExists(string groupName)
        {
            return Groups.Any(g => g.Name == groupName);
        }
    }

    internal class DemoGroup
    {
        public string Name { get; set; }
        public List<EasyADUser> Users { get; set; }
    }
}