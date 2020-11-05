using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using QH.Core.Auth;
using QH.Core.DbHelper;
using QH.Core.Extensions;
using QH.Core.Input;
using QH.Core.Output;
using QH.Core.Result;
using QH.IRepository;
using QH.Models;

namespace QH.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUser _user;
        private readonly IMapper _mapper;
        private readonly IRoleRepository _roleRepository;
        public RoleService(
            IUser user,
            IMapper mapper,
            IRoleRepository roleRepository
        )
        {
            _user = user;
            _mapper = mapper;
            _roleRepository = roleRepository;
        }

        public async Task<IResultModel> GetAsync(int id)
        {
            var result = await _roleRepository.GetAsync(id);
            return ResultModel.Success(result);
        }

        public async Task<IResultModel> PageAsync(RoleListInput input)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("where IsDeleted=0 ");
            if (input.Name.NotNull())
            {
                sb.Append(" and Name=@Name ");
            }
            if (input.Enabled != null)
                sb.Append(" and Enabled=@Enabled ");
            var list = await _roleRepository.GetListPagedAsync(input.Page, input.Limit, sb.ToString(), $"{input.Field} {input.Order}", new { input.Enabled, input.Name });
            var total = await _roleRepository.RecordCountAsync(sb.ToString(), new { input.Enabled, input.Name });
            var data = new PageOutput<RoleListOutput>()
            {
                Data = _mapper.Map<List<RoleListOutput>>(list),
                Count = total
            };
            return ResultModel.Success(data);


        }


        public async Task<IResultModel> AddAsync(RoleAddInput input)
        {
            var entity = _mapper.Map<RoleEntity>(input);
            entity.IsDeleted = false;
            entity.ModifiedTime = entity.CreatedTime = DateTime.UtcNow;
            entity.CreatedUserId = entity.ModifiedUserId = _user.Id;
            entity.CreatedUserName = entity.ModifiedUserName = _user.Name;
            entity.Version = 0;
            entity.Sort = 0;
            var id = await _roleRepository.InsertAsync(entity);

            return ResultModel.Result(id > 0);
        }

        public async Task<IResultModel> UpdateAsync(RoleUpdateInput input)
        {
            if (!(input?.Id > 0))
            {
                return ResultModel.NotExists;
            }

            var entity = await _roleRepository.GetAsync(input.Id);
            if (!(entity?.Id > 0))
            {
                return ResultModel.Failed("角色不存在！");
            }
            _mapper.Map(input, entity);
            entity.ModifiedTime = DateTime.UtcNow;
            entity.ModifiedUserId = _user.Id;
            entity.ModifiedUserName = _user.Name;
            var result = await _roleRepository.UpdateAsync(entity);
            return ResultModel.Result(result > 0);
        }

        public async Task<IResultModel> AddOrUpdateAsync(RoleEntity input)
        {
            input.ModifiedTime = DateTime.UtcNow;
            input.ModifiedUserId = _user.Id;
            input.ModifiedUserName = _user.Name;
            int? result;
            if (!(input.Id > 0))
            {
                //校验名称是否唯一


                input.IsDeleted = false;
                input.CreatedTime = DateTime.UtcNow;
                input.CreatedUserId = _user.Id;
                input.CreatedUserName = _user.Name;
                input.Version = 0;
                input.Sort = 0;
                result = await _roleRepository.InsertAsync(input);
            }
            else
            {
                result = await _roleRepository.UpdateAsync(input);
            }
            return ResultModel.Result(result > 0);
        }

        public async Task<IResultModel> DeleteAsync(int id)
        {
            var result = false;
            if (id > 0)
                result = (await _roleRepository.DeleteAsync(id)) > 0;
            return ResultModel.Result(result);
        }

        public async Task<IResultModel> SoftDeleteAsync(int id)
        {
            var result = await _roleRepository.SoftDeleteAsync(id, _user);
            return ResultModel.Result(result);
        }

        public async Task<IResultModel> SoftDeleteAsync(int[] ids)
        {
            var result = await _roleRepository.SoftDeleteAsync(ids, _user);
            return ResultModel.Result(result);
        }

        public async Task<IResultModel> BatchSoftDeleteAsync(int[] ids)
        {
           // var result = await _roleRepository.BatchSoftDeleteAsync(ids);
            return ResultModel.Result(true);
        }

        public async Task<IResultModel> IsEnable(int id, bool enabled)
        {
          //  var result = await _roleRepository.IsEnable(id, enabled);
            return ResultModel.Result(true);
        }
    }
}
