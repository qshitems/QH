using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using QH.IRepository;
using QH.Core.Output;
using QH.Core.Input;
using QH.Models;
using System.Collections.Generic;
using QH.Core.Helpers;
using QH.Core.Extensions;
using System;
using QH.Core.Result;

namespace QH.Services
{
    public class LoginLogService : ILoginLogService
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _context;
        private readonly ILoginLogRepository _loginLogRepository;
        public LoginLogService(
            IMapper mapper,
            IHttpContextAccessor context, ILoginLogRepository loginLogRepository
        )
        {
            _mapper = mapper;
            _context = context;
            _loginLogRepository = loginLogRepository;
        }

        public async Task<IResultModel> PageAsync(PageInput<LoginLogEntity> input)
        {
            var userName = input.Filter?.CreatedUserName;
            var list = await _loginLogRepository.GetListPagedAsync(input.Page, input.Limit, "", input.OrderBy);
            var total = await _loginLogRepository.RecordCountAsync("", input.Filter);
            var data = new PageOutput<LoginLogListOutput>()
            {
                 Data = _mapper.Map<List<LoginLogListOutput>>(list),
                 Count = total
            };
            return ResultModel.Success();
        }

        public async Task<IResultModel> AddAsync(LoginLogAddInput input)
        {
            input.IP = IPHelper.GetIP(_context?.HttpContext?.Request);
            string ua = _context.HttpContext.Request.Headers["User-Agent"];
            if (ua.NotNull())
            {
                var client = UAParser.Parser.GetDefault().Parse(ua);
                var device = client.Device.Family;
                device = device.ToLower() == "other" ? "" : device;
                input.Browser = client.UA.Family;
                input.Os = client.OS.Family;
                input.Device = device;
                input.BrowserInfo = ua;
            }
            var entity = _mapper.Map<LoginLogEntity>(input);
            entity.CreatedTime = DateTime.UtcNow;
            var id = (await _loginLogRepository.InsertAsync(entity));
            return  ResultModel.Result(id>0);
        }
    }
}
