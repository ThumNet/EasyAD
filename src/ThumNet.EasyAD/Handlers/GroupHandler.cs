using System.Collections.Generic;
using System.Linq;
using ThumNet.EasyAD.Managers;
using ThumNet.EasyAD.Models;
using ThumNet.EasyAD.Repositories;
using ThumNet.EasyAD.Resolvers;
using Umbraco.Core.Logging;
using Umbraco.Core.Models.Membership;
using Umbraco.Core.Services;

namespace ThumNet.EasyAD.Handlers
{
    public abstract class GroupHandler
    {
        protected IEasyADRepository _repo;
        protected IGroupBasedUserManager _groupManager;
        protected IUserService _userService;

        private IEnumerable<IUserType> _allUserTypes = null;
        protected IEnumerable<IUserType> AllUserTypes
        {
            get
            {
                return _allUserTypes ?? (_allUserTypes = _userService.GetAllUserTypes());
            }
        }

        public GroupHandler(IEasyADRepository repo, IGroupBasedUserManager groupManager, IUserService userService)
        {
            _repo = repo;
            _groupManager = groupManager;
            _userService = userService;
        }

        protected void ConfigureUserRights(IUser backofficeUser, IEnumerable<EasyADGroup> groupsUserIsIn)
        {            
            var desiredSections = SectionResolver.Resolve(groupsUserIsIn);
            var newUserType = UserTypeResolver.Resolve(groupsUserIsIn, AllUserTypes);

            var addSections = desiredSections.Except(backofficeUser.AllowedSections).ToList();
            var removeSections = backofficeUser.AllowedSections.Except(desiredSections).ToList();

            if (newUserType.Id != backofficeUser.UserType.Id || addSections.Any() || removeSections.Any())
            {
                addSections.ForEach(s => backofficeUser.AddAllowedSection(s));
                removeSections.ForEach(s => backofficeUser.RemoveAllowedSection(s));
                backofficeUser.UserType = newUserType;
            }

            if (backofficeUser.IsDirty())
            {
                LogHelper.Info<DeleteGroupHandler>(string.Format("Updating userrights for '{0}'", backofficeUser.Name));
                _userService.Save(backofficeUser);
            }
        }
    }
}
