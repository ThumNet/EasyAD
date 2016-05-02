using System.Collections.Generic;
using System.Linq;
using ThumNet.EasyAD.Models;

namespace ThumNet.EasyAD.Resolvers
{
    public static class SectionResolver
    {
        /// <summary>
        /// Determines the combined sections of the supplied groups
        /// </summary>
        public static string[] Resolve(IEnumerable<EasyADGroup> groupsUserIsIn)
        {
            if (groupsUserIsIn.Count() == 1)
            {
                return groupsUserIsIn.First().SectionsArray;
            }

            var sections = groupsUserIsIn.SelectMany(g => g.SectionsArray)  // combine all group sections into one big array
                .Distinct();                                                // remove duplicates

            return sections.ToArray();
        }
    }
}
