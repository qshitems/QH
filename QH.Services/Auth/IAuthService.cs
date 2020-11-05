using QH.Core.Output;
using QH.Core.Result;
using QH.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QH.Services
{
    public interface IAuthService
    {
        Task<IResultModel> LoginAsync(AuthLoginInput input);

        Task<IResultModel> LoginAsync1(AuthLoginInput input);
        Task<IResultModel> GetUserInfoAsync();

        Task<IResultModel> GetVerifyCodeAsync(string lastKey);

        Task<IResultModel> GetPassWordEncryptKeyAsync();
    }
}
