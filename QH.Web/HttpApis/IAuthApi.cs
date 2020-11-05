using QH.Core.Output;
using QH.Core.Result;
using QH.Models;
using QH.Web.Attributes;
using System.Threading.Tasks;
using WebApiClient;
using WebApiClient.Attributes;

namespace QH.Web.HttpApis
{
    [TokenFilter]
    [JsonReturn]
    public interface IAuthApi : IHttpApi
    {
        /// <summary>
        /// 登录处理
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/Admin/Auth/Login")]
        ITask<ResultModel<AuthLoginOutput>> Login([JsonContent] AuthLoginInput input);

        [HttpGet("api/Admin/Auth/GetTest")]
        ITask<string> GetTest( string input);


        [HttpGet("api/Admin/Auth/Test")]
        ITask<IResultModel<bool>> Test(int key);

    }
}
