using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models.Membership;

namespace ThumNet.EasyAD.Tests.TestData
{
    internal class TestUser : IUser
    {
        internal List<string> Sections { get; set; }

        public int Id { get; set; }
        public IUserType UserType { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }

        public IEnumerable<string> AllowedSections
        {
            get
            {
                return Sections;
            }
        }

        public string Comments
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public DateTime CreateDate
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<string> DefaultPermissions
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string Email
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public int FailedPasswordAttempts
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool HasIdentity
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsApproved
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsLockedOut
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public Guid Key
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string Language
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public DateTime LastLockoutDate
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public DateTime LastLoginDate
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public DateTime LastPasswordChangeDate
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }        

        public string PasswordQuestion
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public IProfile ProfileData
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public object ProviderUserKey
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string RawPasswordAnswerValue
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string RawPasswordValue
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string SecurityStamp
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public int SessionTimeout
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public int StartContentId
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public int StartMediaId
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public DateTime UpdateDate
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public void AddAllowedSection(string sectionAlias)
        {
            Sections.Add(sectionAlias);
        }

        public object DeepClone()
        {
            throw new NotImplementedException();
        }

        public void ForgetPreviouslyDirtyProperties()
        {
            throw new NotImplementedException();
        }

        public bool IsDirty()
        {
            throw new NotImplementedException();
        }

        public bool IsPropertyDirty(string propName)
        {
            throw new NotImplementedException();
        }

        public void RemoveAllowedSection(string sectionAlias)
        {
            Sections.Remove(sectionAlias);
        }

        public void ResetDirtyProperties()
        {
            throw new NotImplementedException();
        }

        public void ResetDirtyProperties(bool rememberPreviouslyChangedProperties)
        {
            throw new NotImplementedException();
        }

        public bool WasDirty()
        {
            throw new NotImplementedException();
        }

        public bool WasPropertyDirty(string propertyName)
        {
            throw new NotImplementedException();
        }
    }
}
