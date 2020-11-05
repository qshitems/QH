using System.Threading.Tasks;
using QH.Core.Output;
using QH.Core.Result;
using QH.Models;

namespace QH.Services
{	
    public interface IOprationLogService
    {
      //  Task<IResultModel> PageAsync(PageInput<OprationLogEntity> input);

        Task<IResultModel> AddAsync(OprationLogAddInput input);
    }
}
