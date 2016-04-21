using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace ThumNet.EasyAD.Models
{
    [TableName(AppConstants.TableNames.EasyADGroup2Users)]
    public class EasyADGroup2User
    {
        public int GroupId { get; set; }
        public int UserId { get; set; }
    }
}
