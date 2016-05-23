using Umbraco.Core.ObjectResolution;

namespace ThumNet.EasyAD.Managers
{
    public class GroupBasedUserManagerResolver : SingleObjectResolverBase<GroupBasedUserManagerResolver, IGroupBasedUserManager>
    {
        protected GroupBasedUserManagerResolver()
            : base()
        {
        }

        internal GroupBasedUserManagerResolver(IGroupBasedUserManager value)
            : base(value)
        {
        }

        public void SetManager(IGroupBasedUserManager manager)
        {
            Value = manager;
        }

        public IGroupBasedUserManager Manager
        {
            get { return Value; }
        }
    }
}
