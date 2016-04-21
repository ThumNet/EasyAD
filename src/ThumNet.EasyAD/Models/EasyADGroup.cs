using System;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace ThumNet.EasyAD.Models
{
    [TableName(AppConstants.TableNames.EasyADGroups)]
    public class EasyADGroup
    {
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        public string Name { get; set; }

        public int UserType { get; set; }

        public string Sections { get; set; }

        [Ignore]
        public string[] SectionsArray
        {
            get
            {
                return Sections.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            }
        }
    }
}
