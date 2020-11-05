

using QH.Core.DbHelper;
using QH.Core.Options;

using QH.IRepository;
using QH.Models;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using QH.Core.Auth;
using System.Collections.Generic;
using QH.Models.Enums;

namespace QH.Repository
{
    public partial class UserRepository : BaseRepository<UserEntity>, IUserRepository
    {

 
   

        public async Task<List<long>> GetRole(long userid)
        {
            string sql = @" SELECT midtb.[RoleId]
                                  FROM[ad_role] a
                                  INNER JOIN[ad_user_role] midtb ON midtb.[RoleId] = a.[Id]
                                  WHERE(midtb.[UserId] = @UserId)";
            using var _dbConnection = Connection;
            return (List<long>)await _dbConnection.QueryAsync<long>(sql, new { UserId = userid });
        }

        public async Task<List<string>> GetPermissionsAsync(long id)
        {

            var sql = @"SELECT DISTINCT a__Permission__Api.[Path] 
                            FROM [ad_role_permission] a
                            LEFT JOIN [ad_permission] a__Permission ON a__Permission.[Id] = a.[PermissionId]
                            INNER JOIN [ad_user_role] b ON a.[RoleId] = b.[RoleId] AND b.[UserId] = @UserId AND a__Permission.[Type] = @Type
                            LEFT JOIN [ad_api] a__Permission__Api ON a__Permission__Api.[Id] = a__Permission.[ApiId]";
            using var _dbConnection = Connection;
            return await _dbConnection.QueryAsync<string>(sql, new { UserId =id, Type = PermissionType.Api }) as List<string>;

        }
    }
}