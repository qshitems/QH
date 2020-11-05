using QH.Core.Input;
using QH.Core.Output;
using QH.Core.Result;
using QH.Models;
using System.Threading.Tasks;

namespace QH.Services
{	
    public interface ILoginLogService
    {
        Task<IResultModel> PageAsync(PageInput<LoginLogEntity> input);

        Task<IResultModel> AddAsync(LoginLogAddInput input);
    }
}
