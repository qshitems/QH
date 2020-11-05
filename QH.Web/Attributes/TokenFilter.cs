using QH.Core.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiClient.Attributes;
using WebApiClient.Contexts;

namespace QH.Web.Attributes
{
    public class TokenFilter : ApiActionFilterAttribute
    {
        public override Task OnBeginRequestAsync(ApiActionContext context)
        {
            if (context.Exception != null)
            {
                return Task.CompletedTask;
            }
            try
            {
                var _loginInfo = context.GetService<IUser>();
                context.RequestMessage.Headers.Add("Bearer ", _loginInfo.NickName);
            }
            catch (System.Exception)
            {
                return Task.CompletedTask;
            }
            return base.OnBeginRequestAsync(context);
        }
    }

}
