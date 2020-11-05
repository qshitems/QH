using QH.Core.Input;
using QH.Core.Result;
using QH.Models;
using System.Threading.Tasks;

namespace QH.Services
{	
    public interface IRoleService
	{
        Task<IResultModel> GetAsync(int id);

        Task<IResultModel> PageAsync(RoleListInput input);

        Task<IResultModel> AddAsync(RoleAddInput input);

        Task<IResultModel> UpdateAsync(RoleUpdateInput input);

        Task<IResultModel> AddOrUpdateAsync(RoleEntity input);

        Task<IResultModel> DeleteAsync(int id);

        Task<IResultModel> SoftDeleteAsync(int id);
        Task<IResultModel> SoftDeleteAsync(int[] ids);

        Task<IResultModel> BatchSoftDeleteAsync(int[] ids);

        Task<IResultModel> IsEnable(int id, bool enabled);
    }
}
