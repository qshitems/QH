using QH.Core.Input;
using QH.Core.Output;
using QH.Core.Result;
using QH.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QH.Services
{
    public partial interface IUserService
    {
        Task<IResultModel> GetLoginUserAsync(long id);

        Task<IResultModel> GetAsync(long id);

        Task<IResultModel> PageAsync(PageInput<UserListInput> input);

        Task<IResultModel> AddAsync(UserAddInput input);

        Task<IResultModel> UpdateAsync(UserUpdateInput input);

        Task<IResultModel> DeleteAsync(int id);

        Task<IResultModel> SoftDeleteAsync(int id);

        Task<IResultModel> BatchSoftDeleteAsync(int[] ids);

        Task<IResultModel> ChangePasswordAsync(UserChangePasswordInput input);

        Task<IResultModel> UpdateBasicAsync(UserUpdateBasicInput input);

        Task<IResultModel> GetBasicAsync();

        Task<IResultModel> GetPermissionsAsync();
    }
}
