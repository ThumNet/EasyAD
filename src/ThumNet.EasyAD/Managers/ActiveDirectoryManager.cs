using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using ThumNet.EasyAD.Models;

namespace ThumNet.EasyAD.Managers
{
    internal class ActiveDirectoryManager : IGroupBasedUserManager
    {
        private string _domain;
        private string _ldapPath;

        public ActiveDirectoryManager()
        {
            try
            {
                _domain = ConfigurationManager.AppSettings["EasyADDomain"];
                _ldapPath = ConfigurationManager.AppSettings["EasyADPath"];
            }
            catch
            {
                _domain = string.Empty;
                _ldapPath = string.Empty;
            }
        }

        public bool CheckPassword(string userName, string password)
        {
            return CheckPasswordUsingPrincipalContext(userName, password);
        }

        public IEnumerable<EasyADUser> GetUsersInGroup(string groupName)
        {
            throw new NotImplementedException();
        }

        public bool GroupExists(string groupName)
        {
            throw new NotImplementedException();
        }

        internal bool CheckPasswordUsingPrincipalContext(string userName, string password)
        {
            // http://stackoverflow.com/questions/290548/validate-a-username-and-password-against-active-directory
            // TODO: Test with old passwords, read the topic above

            if (string.IsNullOrWhiteSpace(_domain))
            {
                return false;
            }

            try
            {
                using (var context = new PrincipalContext(ContextType.Domain, _domain))
                {
                    return context.ValidateCredentials(userName, password);
                }
            }
            catch
            {
                // TODO: log exception
                return false;
            }
        }

        internal bool CheckPasswordOldskool(string userName, string password)
        {
            bool resp = false;
            try
            {
                if (!String.IsNullOrWhiteSpace(_ldapPath))
                {
                    var domainAndUsername = _domain + @"\" + userName;
                    var entry = new DirectoryEntry(_ldapPath, domainAndUsername, password);
                    try
                    {
                        object obj = entry.NativeObject;
                        var search = new DirectorySearcher(entry);
                        search.Filter = "(SAMAccountName=" + userName + ")";
                        search.PropertiesToLoad.Add("cn");
                        var result = search.FindOne();
                        if (result != null)
                        {
                            // Login was successful
                            resp = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Login was invalid, you can examine the Exception object here to see why if you want
                    }
                }
            }
            catch (Exception ex)
            {
                // Login was invalid, you can examine the Exception object here to see why if you want
            }
            return resp;
        }
    }
}