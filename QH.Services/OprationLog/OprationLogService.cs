using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using QH.Core.Auth;
using QH.IRepository;
using QH.Core.Output;
using QH.Core.Input;
using QH.Models;
using QH.Core.Helpers;
using QH.Core.Result;

namespace QH.Services
{	
	public class OprationLogService : IOprationLogService
    {
        private readonly IUser _user;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _context;
        private readonly IOprationLogRepository _oprationLogRepository;
        public OprationLogService(
            IUser user,
            IMapper mapper,
            IHttpContextAccessor context,
            IOprationLogRepository oprationLogRepository
        )
        {
            _user = user;
            _mapper = mapper;
            _context = context;
            _oprationLogRepository = oprationLogRepository;
        }

        //public async Task<IResultModel> PageAsync(PageInput<OprationLogEntity> input)
        //{
        //    var userName = input.Filter?.CreatedUserName;

        //    var list = await _oprationLogRepository.Select
        //    .WhereIf(userName.NotNull(), a => a.CreatedUserName.Contains(userName))
        //    .Count(out var total)
        //    .OrderByDescending(true, c => c.Id)
        //    .Page(input.CurrentPage, input.PageSize)
        //    .ToListAsync<OprationLogListOutput>();

        //    var data = new PageOutput<OprationLogListOutput>()
        //    {
        //        List = list,
        //        Total = total
        //    };
            
        //    return ResponseOutput.Ok(data);
        //}

        public async Task<IResultModel> AddAsync(OprationLogAddInput input)
        {
            string ua = _context.HttpContext.Request.Headers["User-Agent"];
            var client = UAParser.Parser.GetDefault().Parse(ua);
            var device = client.Device.Family;
            device = device.ToLower() == "other" ? "" : device;
            input.Browser = client.UA.Family;
            input.Os = client.OS.Family;
            input.Device = device;
            input.BrowserInfo = ua;
            input.NickName = _user.NickName;
            input.IP = IPHelper.GetIP(_context?.HttpContext?.Request);
            var entity = _mapper.Map<OprationLogEntity>(input);
            var id = await _oprationLogRepository.InsertAsync(entity);
            return ResultModel.Result(id > 0);
        }
    }
}
