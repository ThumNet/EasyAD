using System.Collections.Generic;
using System.Linq;
using ThumNet.EasyAD.Models;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.SqlSyntax;

namespace ThumNet.EasyAD.Repositories
{
    public class EasyADRepository : IEasyADRepository
    {
        private UmbracoDatabase _database;
        private ISqlSyntaxProvider _sqlSyntax;

        public EasyADRepository(UmbracoDatabase database, ISqlSyntaxProvider sqlSyntax)
        {
            _database = database;
            _sqlSyntax = sqlSyntax;
        }

        public EasyADGroup GetById(int id)
        {
            var query = new Sql().Select("*").From(AppConstants.TableNames.EasyADGroups).Where<EasyADGroup>(x => x.Id == id);
            return _database.Fetch<EasyADGroup>(query).FirstOrDefault();
        }

        public IEnumerable<EasyADGroup> GetAll()
        {
            var query = new Sql().Select("*").From(AppConstants.TableNames.EasyADGroups);
            return _database.Fetch<EasyADGroup>(query);
        }

        public IEnumerable<EasyADGroup> GetGroupsForUser(int userId)
        {
            string sql = string.Format(@"SELECT * FROM [{0}]
                                         INNER JOIN [{1}] ON [{1}].[GroupId] = [{0}].[Id]
                                         WHERE [{1}].[UserId] = @0",
                                         AppConstants.TableNames.EasyADGroups,
                                         AppConstants.TableNames.EasyADGroup2Users);
            return _database.Fetch<EasyADGroup>(sql, userId);
        }

        public IEnumerable<EasyADGroup2User> GetAllUsers()
        {
            var query = new Sql().Select("*").From(AppConstants.TableNames.EasyADGroup2Users);
            return _database.Fetch<EasyADGroup2User>(query);
        }

        public void DeleteUser(int id)
        {
            var query = new Sql().Where<EasyADGroup2User>(e => e.UserId == id);
            _database.Delete<EasyADGroup2User>(query);
        }

        public int DeleteGroup(int id)
        {
            var query = new Sql().Where<EasyADGroup2User>(e => e.GroupId == id);
            _database.Delete<EasyADGroup2User>(query);

            return _database.Delete<EasyADGroup>(id);
        }

        public void SaveOrUpdate(EasyADGroup group)
        {
            if (group.Id > 0)
            {
                _database.Update(group);
            }
            else
            {
                _database.Save(group);
            }
        }

        public void AddUserToGroup(int groupId, int backofficeUserId)
        {
            var toInsert = new EasyADGroup2User { GroupId = groupId, UserId = backofficeUserId };
            _database.Insert(toInsert);
        }

        public void DeleteGroupUsers(int groupId)
        {
            var sql = new Sql().Where<EasyADGroup2User>(u => u.GroupId == groupId);
            _database.Delete<EasyADGroup2User>(sql);
        }
    }
}
