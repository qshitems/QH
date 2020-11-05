using AutoMapper;
using Dapper;
using Microsoft.Extensions.Options;
using QH.Core.Auth;
using QH.Core.Cache;
using QH.Core.Configs;
using QH.Core.Extensions;
using QH.Core.Helpers;
using QH.Core.Options;
using QH.Core.Output;
using QH.IRepository;
using QH.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using QH.Core.DbHelper;
using System.Linq;
using QH.Core.Result;

namespace QH.Services
{
    public partial class AuthService : IAuthService
    {

        private readonly IUser _user;
        private readonly ICache _cache;
        private readonly IMapper _mapper;

        private readonly VerifyCodeHelper _verifyCodeHelper;
        private readonly IOptionsSnapshot<DbOption> _dbOption;
        private readonly IUserRepository _userRepository;

        public AuthService(
         IUser user,
         ICache cache,
         IMapper mapper,
         VerifyCodeHelper verifyCodeHelper,
         IOptionsSnapshot<DbOption> dbOption,
         IUserRepository userRepository

     )
        {
            _user = user;
            _cache = cache;
            _mapper = mapper;
            _verifyCodeHelper = verifyCodeHelper;
            _dbOption = dbOption;
            _userRepository = userRepository;
        }



        public async Task<IResultModel> LoginAsync(AuthLoginInput input)
        {
            #region 验证码校验
            //if (_appConfig.VarifyCode.Enable)
            //{
            //    var verifyCodeKey = string.Format(CacheKey.VerifyCodeKey, input.VerifyCodeKey);
            //    var exists = await _cache.ExistsAsync(verifyCodeKey);
            //    if (exists)
            //    {
            //        var verifyCode = await _cache.GetAsync(verifyCodeKey);
            //        if (string.IsNullOrEmpty(verifyCode))
            //        {
            //            return ResponseOutput.NotOk("验证码已过期！", 1);
            //        }
            //        if (verifyCode.ToLower() != input.VerifyCode.ToLower())
            //        {
            //            return ResponseOutput.NotOk("验证码输入有误！", 2);
            //        }
            //        await _cache.DelAsync(verifyCodeKey);
            //    }
            //    else
            //    {
            //        return ResponseOutput.NotOk("验证码已过期！", 1);
            //    }
            //}
            #endregion
            string conditions = "where UserName=@UserName ";
            var user = await _userRepository.GetAsync(conditions, new { input.UserName });
            if (!(user?.Id > 0))
            {
                return ResultModel.Failed("账号输入有误!", 3);
            }

            #region 解密
            //if (input.PasswordKey.NotNull())
            //{
            //    var passwordEncryptKey = string.Format(CacheKey.PassWordEncryptKey, input.PasswordKey);
            //    var existsPasswordKey = await _cache.ExistsAsync(passwordEncryptKey);
            //    if (existsPasswordKey)
            //    {
            //        var secretKey = await _cache.GetAsync(passwordEncryptKey);
            //        if (secretKey.IsNull())
            //        {
            //            return ResponseOutput.NotOk("解密失败！", 1);
            //        }
            //        input.Password = DesEncrypt.Decrypt(input.Password, secretKey);
            //        await _cache.DelAsync(passwordEncryptKey);
            //    }
            //    else
            //    {
            //        return ResponseOutput.NotOk("解密失败！", 1);
            //    }
            //}
            #endregion

            var password = MD5Encrypt.Encrypt32(input.Password);
            if (user.Password != password)
            {
                return ResultModel.Failed("密码输入有误！", 4);
            }


            var authLoginOutput = _mapper.Map<AuthLoginOutput>(user);
            //await _cache.SetAsync(CacheKey.UserAuthLogin, authLoginOutput, TimeSpan.FromMinutes(5));

            return ResultModel.Success(authLoginOutput);
        }

        public async Task<IResultModel> LoginAsync1(AuthLoginInput input)
        {
            #region 验证码校验
            //if (_appConfig.VarifyCode.Enable)
            //{
            //    var verifyCodeKey = string.Format(CacheKey.VerifyCodeKey, input.VerifyCodeKey);
            //    var exists = await _cache.ExistsAsync(verifyCodeKey);
            //    if (exists)
            //    {
            //        var verifyCode = await _cache.GetAsync(verifyCodeKey);
            //        if (string.IsNullOrEmpty(verifyCode))
            //        {
            //            return ResponseOutput.NotOk("验证码已过期！", 1);
            //        }
            //        if (verifyCode.ToLower() != input.VerifyCode.ToLower())
            //        {
            //            return ResponseOutput.NotOk("验证码输入有误！", 2);
            //        }
            //        await _cache.DelAsync(verifyCodeKey);
            //    }
            //    else
            //    {
            //        return ResponseOutput.NotOk("验证码已过期！", 1);
            //    }
            //}
            #endregion
            string conditions = "where UserName=@UserName ";
            var user = await _userRepository.GetAsync(conditions, new { input.UserName });
            if (!(user?.Id > 0))
            {
                return ResultModel.Failed("账号输入有误!", 3);
            }

            #region 解密
            //if (input.PasswordKey.NotNull())
            //{
            //    var passwordEncryptKey = string.Format(CacheKey.PassWordEncryptKey, input.PasswordKey);
            //    var existsPasswordKey = await _cache.ExistsAsync(passwordEncryptKey);
            //    if (existsPasswordKey)
            //    {
            //        var secretKey = await _cache.GetAsync(passwordEncryptKey);
            //        if (secretKey.IsNull())
            //        {
            //            return ResponseOutput.NotOk("解密失败！", 1);
            //        }
            //        input.Password = DesEncrypt.Decrypt(input.Password, secretKey);
            //        await _cache.DelAsync(passwordEncryptKey);
            //    }
            //    else
            //    {
            //        return ResponseOutput.NotOk("解密失败！", 1);
            //    }
            //}
            #endregion

            var password = MD5Encrypt.Encrypt32(input.Password);
            if (user.Password != password)
            {
                return ResultModel.Failed("密码输入有误！", 4);
            }


            var authLoginOutput = _mapper.Map<AuthLoginOutput>(user);
            //await _cache.SetAsync(CacheKey.UserAuthLogin, authLoginOutput, TimeSpan.FromMinutes(5));

            return ResultModel.Success(authLoginOutput);
        }

        public async Task<IResultModel> GetUserInfoAsync()
        {
            if (!(_user?.Id > 0))
            {
                return ResultModel.Failed("未登录！");
            }
            var id = _user.Id;
            var key = string.Format(CacheKey.UserAuthLogin, id);
            if (await _cache.ExistsAsync(key))
            {
                return ResultModel.Success(await _cache.GetAsync<HomeModel>(key));
            }
            else
            {
                var _HomeModel = new HomeModel();
                try
                {

                    using (var _dbConn = ConnectionFactory.CreateConnection(_dbOption.Value.DbType, _dbOption.Value.ConnectionString))
                    {
                       
                        //用户信息
                        var sql = @"SELECT TOP 1 NickName , UserName , Avatar FROM ad_user WHERE IsDeleted=0 AND Id = @Id";
                        _HomeModel.user = await _dbConn.QueryFirstAsync<UserModel>(sql, new { Id =id });

                        //用户菜单
                        sql = @"SELECT a.[Id] , a.[ParentId] , a.[Path] ,  a.[Label] , a.[Icon] , a.[Opened] , a.[Closable] , a.[Hidden] , a.[NewWindow] , a.[External]  FROM[ad_permission] a
                        WHERE(a.[IsDeleted] = 0) AND(((a.[Type]) in (1, 2))) AND(exists(SELECT TOP 1 1   FROM[ad_role_permission] b
                           INNER JOIN[ad_user_role] c ON b.[RoleId] = c.[RoleId] AND c.[UserId] = @UserId
                           WHERE(b.[PermissionId] = a.[Id])))
                        ORDER BY a.[ParentId], a.[Sort]";
                        _HomeModel.menus =  await _dbConn.QueryAsync<MenusModel>(sql, new { UserId = id }) as List<MenusModel>;
                   
                               sql = @"SELECT a.[Code]  FROM[ad_permission] a
                                    WHERE(a.[IsDeleted] = 0) AND(((a.[Type]) in (3, 4))) AND(exists(SELECT TOP 1 1
                                       FROM[ad_role_permission] b
                                       INNER JOIN[ad_user_role] c ON b.[RoleId] = c.[RoleId] AND c.[UserId] =  @UserId
                                       WHERE(b.[PermissionId] = a.[Id])))";
                        _HomeModel.permissions = (List<string>)await _dbConn.QueryAsync<string>(sql, new { UserId = id });
                        await _cache.SetAsync(key, _HomeModel);
                        return ResultModel.Success(_HomeModel);
                    }
                }
                catch (Exception )
                {

                    return ResultModel.Failed("数据异常！");
                }
            }


        }

        public async Task<IResultModel> GetVerifyCodeAsync(string lastKey)
        {
            //var _loginOutput = _cache.Get<AuthLoginOutput>(CacheKey.UserAuthLogin);
            var img = _verifyCodeHelper.GetBase64String(out string code);

            //删除上次缓存的验证码
            if (lastKey.NotNull())
            {
                await _cache.DelAsync(lastKey);
            }

            //写入Redis
            var guid = Guid.NewGuid().ToString("N");
            var key = string.Format(CacheKey.VerifyCodeKey, guid);
            await _cache.SetAsync(key, code, TimeSpan.FromMinutes(5));

            var data = new AuthGetVerifyCodeOutput { Key = guid, Img = img };
            return ResultModel.Success(data);
        }

        public async Task<IResultModel> GetPassWordEncryptKeyAsync()
        {
            //写入Redis
            var guid = Guid.NewGuid().ToString("N");
            var key = string.Format(CacheKey.PassWordEncryptKey, guid);
            var encyptKey = StringHelper.GenerateRandom(8);
            await _cache.SetAsync(key, encyptKey, TimeSpan.FromMinutes(5));
            var data = new { key = guid, encyptKey };
            return ResultModel.Success(data);
        }
    }
}
