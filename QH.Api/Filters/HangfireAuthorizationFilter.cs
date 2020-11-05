using Hangfire.Annotations;
using Hangfire.Dashboard;
using QH.Core.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QH.Api.Filters
{
    public class HangfireAuthorizationFilter: IDashboardAuthorizationFilter
    {
    
     
        public bool Authorize( DashboardContext context)
        {

            //return  IUser.IsAdmin(); //True 可以访问，False拒绝访问，结合系统原有逻辑设置
            return false;
           //return (bool)(context.GetHttpContext()?.User?.Identity.IsAuthenticated);
       
        }

    }
}
