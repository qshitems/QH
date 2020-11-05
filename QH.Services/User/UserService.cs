using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using QH.Core.Auth;
using QH.Core.Cache;
using Microsoft.Extensions.Options;
using QH.Core.Options;
using QH.Core.Output;
using QH.Services;
using QH.IRepository;
using QH.Models;
using QH.Core.DbHelper;
using QH.Core.Input;
using QH.Core.Extensions;
using QH.Core.Helpers;
using System.Data;
using System;
using QH.Models.Enums;
using Dapper;
using QH.Core.Result;

namespace QH.Service
{
    /// <summary>
    /// 用户服务
    /// </summary>	
    public partial class UserService : IUserService
    {
        private readonly IUser _user;
        private readonly ICache _cache;
        private readonly IMapper _mapper;
        private readonly IOptionsSnapshot<DbOption> _dbOption;
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        public UserService(
            IUser user,
            ICache cache,
            IMapper mapper,
            IOptionsSnapshot<DbOption> dbOption,
            IUserRepository userRepository,
            IUserRoleRepository userRoleRepository
        )
        {
            _user = user;
            _cache = cache;
            _mapper = mapper;
            _dbOption = dbOption;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
        }

        public async Task<IResultModel> GetLoginUserAsync(long id)
        {
            var entityDto = await _userRepository.GetAsync(id);
            return ResultModel.Success(entityDto);
        }
        public async Task<IResultModel> GetAsync(long id)
        {
            try
            {
                var entity = _mapper.Map<UserGetOutput>(await _userRepository.GetAsync(id));
                if (entity != null)
                {
                    using (var _dbConnection = _userRepository.Connection)
                    {
                        string sql = @" SELECT midtb.[RoleId]
                                  FROM[ad_role] a
                                  INNER JOIN[ad_user_role] midtb ON midtb.[RoleId] = a.[Id]
                                  WHERE(midtb.[UserId] = @UserId)";
                        entity.RoleIds = (List<int>)await _dbConnection.QueryAsync<int>(sql, new { UserId = id });
                    }
                }
                return ResultModel.Success(entity);
            }
            catch (Exception exx)
            {

                throw;
            }
        }

        /// <summary>
        /// 用户基本信息
        /// </summary>
        /// <returns></returns>
        public async Task<IResultModel> GetBasicAsync()
        {
            if (!(_user?.Id > 0))
            {
                return ResultModel.Failed("未登录！");
            }

            var data = _mapper.Map<UserUpdateBasicInput>(await _userRepository.GetAsync(_user.Id));
            return ResultModel.Success(data);
        }

        public async Task<IResultModel> GetPermissionsAsync()
        {
            return await GetPermissionsAsync(_user.Id);
        }
        public async Task<IResultModel> GetPermissionsAsync(long id)
        {
            var key = string.Format(CacheKey.UserPermissions, id);
            if (await _cache.ExistsAsync(key))
            {
                return ResultModel.Success(await _cache.GetAsync<List<string>>(key));
            }
            else
            {
                var sql = @"SELECT DISTINCT a__Permission__Api.[Path] 
                            FROM [ad_role_permission] a
                            LEFT JOIN [ad_permission] a__Permission ON a__Permission.[Id] = a.[PermissionId]
                            INNER JOIN [ad_user_role] b ON a.[RoleId] = b.[RoleId] AND b.[UserId] = @UserId AND a__Permission.[Type] = @Type
                            LEFT JOIN [ad_api] a__Permission__Api ON a__Permission__Api.[Id] = a__Permission.[ApiId]";
                using (var _dbConnection = ConnectionFactory.CreateConnection(_dbOption.Value))
                {
                    var userPermissoins = await _dbConnection.QueryAsync<string>(sql, new { UserId = _user.Id, Type = PermissionType.Api }) as List<string>;
                    await _cache.SetAsync(key, userPermissoins);
                    return ResultModel.Success(userPermissoins);
                }
            }
        }
        public async Task<IResultModel> PageAsync(PageInput<UserListInput> input)
        {
            var _conditions = "";
            if (input.Filter != null)
                _conditions = input.Filter.ModelToWhere();
            var list = await _userRepository.GetListPagedAsync(input.Page, input.Limit, _conditions, input.OrderBy, input.Filter);
             var total = await _userRepository.RecordCountAsync(_conditions, input.Filter);
            var data = new PageOutput<UserListOutput>()
            {
                Data = _mapper.Map<List<UserListOutput>>(list),
                Count = total
            };
            return ResultModel.Success(data);
        }
        public async Task<IResultModel> AddAsync(UserAddInput input)
        {
            if (input.Password.IsNull())
            {
                input.Password = "000000";
            }
            input.Password = MD5Encrypt.Encrypt32(input.Password);
            var entity = _mapper.Map<UserEntity>(input);
            entity.IsDeleted = false;
            entity.ModifiedTime = entity.CreatedTime = DateTime.UtcNow;
            entity.CreatedUserId = entity.ModifiedUserId = _user.Id;
            entity.CreatedUserName = entity.ModifiedUserName = _user.Name;
            using (var _dbConnection = ConnectionFactory.CreateConnection(_dbOption.Value))
            {
                IDbTransaction transaction = _dbConnection.BeginTransaction();
                try
                {
                    var _id = await _dbConnection.InsertAsync<UserEntity>(entity, transaction);
                    if (!(_id > 0))
                    {
                        return ResultModel.Failed();
                    }
                    UserRoleEntity _userRoleEntity;
                    if (input.RoleIds != null && input.RoleIds.Any())
                    {

                        foreach (var _ro in input.RoleIds)
                        {
                            _userRoleEntity = new UserRoleEntity
                            {
                                RoleId = _ro,
                                UserId = _id,
                                CreatedTime = DateTime.UtcNow,
                                CreatedUserId = _user.Id,
                                CreatedUserName = _user.Name
                            };
                            await _dbConnection.InsertAsync<UserRoleEntity>(_userRoleEntity, transaction);
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
            return ResultModel.Success();
        }


        public async Task<IResultModel> AddAsync(UserEntity input)
        {
            if (input.Password.IsNull())
            {
                input.Password = "000000";
            }

            input.Password = MD5Encrypt.Encrypt32(input.Password);

            var entity = _mapper.Map<UserEntity>(input);
            entity.IsDeleted = false;
            entity.ModifiedTime = entity.CreatedTime = DateTime.UtcNow;
            entity.CreatedUserId = entity.ModifiedUserId = _user.Id;
            entity.CreatedUserName = entity.ModifiedUserName = _user.Name;

            try
            {
                _userRepository.BeginTrans();
                var _id = await _userRepository.InsertAsync(entity);
                if (!(_id > 0))
                {
                    return ResultModel.Failed();
                }
                UserRoleEntity _userRoleEntity;
                if (input.RoleIds != null && input.RoleIds.Any())
                {

                    foreach (var _ro in input.RoleIds)
                    {
                        _userRoleEntity = new UserRoleEntity
                        {
                            RoleId = _ro,
                            UserId = _id,
                            CreatedTime = DateTime.UtcNow,
                            CreatedUserId = _user.Id,
                            CreatedUserName = _user.Name
                        };
                        await _userRepository.Transaction.Connection.InsertAsync<UserRoleEntity>(_userRoleEntity, _userRepository.Transaction);
                    }
                }
                _userRepository.Commit();
            }
            catch (Exception ex)
            {
                _userRepository.Rollback();
            }
            return ResultModel.Success();
        }



        public async Task<IResultModel> UpdateAsync(UserUpdateInput input)
        {
            if (!(input?.Id > 0))
            {
                return ResultModel.Failed();
            }
            using (var _dbConnection = ConnectionFactory.CreateConnection(_dbOption.Value))
            {
                IDbTransaction transaction = _dbConnection.BeginTransaction();
                try
                {
                    var user = await _userRepository.GetAsync(input.Id);
                    if (!(user?.Id > 0))
                    {
                        return ResultModel.NotExists;
                    }
                    _mapper.Map(input, user);
                    user.ModifiedTime = DateTime.UtcNow;
                    user.ModifiedUserId = _user.Id;
                    user.ModifiedUserName = _user.Name;
                    await _dbConnection.UpdateAsync(user, transaction);
                    await _dbConnection.DeleteListAsync<UserRoleEntity>(new { Userid = input.Id }, transaction);
                    UserRoleEntity _userRoleEntity;
                    foreach (var _ro in input.RoleIds)
                    {
                        _userRoleEntity = new UserRoleEntity
                        {
                            RoleId = _ro,
                            UserId = input.Id,
                            CreatedTime = DateTime.UtcNow,
                            CreatedUserId = _user.Id,
                            CreatedUserName = _user.Name
                        };
                        await _dbConnection.InsertAsync<UserRoleEntity>(_userRoleEntity, transaction);
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
                return ResultModel.Success();
            }
        }

        public async Task<IResultModel> UpdateBasicAsync(UserUpdateBasicInput input)
        {
            var entity = await _userRepository.GetAsync(input.Id);
            entity = _mapper.Map(input, entity);
            var result = (await _userRepository.UpdateAsync(entity)) > 0;
            return ResultModel.Result(result);
        }

        public async Task<IResultModel> ChangePasswordAsync(UserChangePasswordInput input)
        {
            if (input.ConfirmPassword != input.NewPassword)
            {
                return ResultModel.Failed("新密码和确认密码不一致！");
            }

            var entity = await _userRepository.GetAsync(input.Id);
            var oldPassword = MD5Encrypt.Encrypt32(input.OldPassword);
            if (oldPassword != entity.Password)
            {
                return ResultModel.Failed("旧密码不正确！");
            }

            input.Password = MD5Encrypt.Encrypt32(input.NewPassword);

            entity = _mapper.Map(input, entity);
            var result = (await _userRepository.UpdateAsync(entity)) > 0;

            return ResultModel.Result(result);
        }

        public async Task<IResultModel> DeleteAsync(int id)
        {
            var result = false;
            if (id > 0)
            {
                result = (await _userRepository.DeleteAsync(id)) > 0;
            }

            return ResultModel.Result(result);
        }

        public async Task<IResultModel> SoftDeleteAsync(int id)
        {
            //await _userRepository.DeleteLogicalAsync((id);

            using (var _dbConnection = ConnectionFactory.CreateConnection(_dbOption.Value))
            {
                string sql = "update ad_user set IsDelete=1,ModifiedUserId=@ModifiedUserId,ModifiedUserName=@ModifiedUserName,ModifiedTime=@ModifiedTime where Id = @Id";
                var result = await _dbConnection.ExecuteAsync(sql, new
                {
                    Id = id,
                    ModifiedUserId = _user.Id,
                    ModifiedUserName = _user.Name,
                    ModifiedTime = DateTime.UtcNow
                }) > 0;
                return ResultModel.Result(result);
            }
        }

        public async Task<IResultModel> BatchSoftDeleteAsync(int[] ids)
        {
            using (var _dbConnection = ConnectionFactory.CreateConnection(_dbOption.Value))
            {
                string sql = "update ad_user set IsDelete=1,ModifiedUserId=@ModifiedUserId,ModifiedUserName=@ModifiedUserName,ModifiedTime=@ModifiedTime where Id in  @ids";
                var result = await _dbConnection.ExecuteAsync(sql, new
                {
                    ids = ids,
                    ModifiedUserId = _user.Id,
                    ModifiedUserName = _user.Name,
                    ModifiedTime = DateTime.UtcNow
                }) > 0;
                return ResultModel.Result(result);
            }
        }
    }
}

