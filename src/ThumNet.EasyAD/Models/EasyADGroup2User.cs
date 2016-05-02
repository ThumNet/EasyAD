using Umbraco.Core.Persistence;

namespace ThumNet.EasyAD.Models
{
    [TableName(AppConstants.TableNames.EasyADGroup2Users)]
    public class EasyADGroup2User
    {
        public int GroupId { get; set; }
        public int UserId { get; set; }
    }
}
