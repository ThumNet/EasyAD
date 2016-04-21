using System.Collections.Generic;
using System.Linq;
using ThumNet.EasyAD.Models;
using Umbraco.Core.Models.Membership;

namespace ThumNet.EasyAD.Resolvers
{
    public static class UserTypeResolver
    {
        public static IUserType Resolve(IEnumerable<EasyADGroup> groupsUserIsIn, IEnumerable<IUserType> allUserTypes)
        {
            if (groupsUserIsIn.Count() == 1)
            {
                return allUserTypes.First(u => u.Id == groupsUserIsIn.First().UserType);
            }

            // for now simple implementation, whichever usertype has more permission entries wins
            var best = allUserTypes.First(u => u.Id == groupsUserIsIn.FirstOrDefault().UserType);
            var otherGroups = groupsUserIsIn.Where(g => g.UserType != best.Id);
            foreach (var group in otherGroups)
            {                
                var type = allUserTypes.First(u => u.Id == group.UserType);
                if (type.Permissions.Count() > best.Permissions.Count())
                {
                    best = type;
                }
            }
            return best;
        }
    }
}
