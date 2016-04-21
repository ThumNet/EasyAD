using System;
using System.Collections.Generic;
using Umbraco.Core.Models.Membership;

namespace ThumNet.EasyAD.Tests.TestData
{
    internal class TestUserType : IUserType
    {
        public string Alias { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> Permissions { get; set; }

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

        public bool HasIdentity
        {
            get
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

        public object DeepClone()
        {
            throw new NotImplementedException();
        }
    }
}
