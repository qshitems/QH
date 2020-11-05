

/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：权限                                                    
*│　作    者：QSH                                              
*│　版    本：1.0                                               
*│　创建时间：2020-07-09 17:45:28                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： QH.IRepository                                   
*│　接口名称： IPermissionRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using QH.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QH.IRepository
{
    public partial interface IPermissionRepository : IBaseRepository<PermissionEntity>
    {
        Task<IEnumerable<PermissionListOutput>> GetListAsync(string key);

        Task<bool> AssignAsync(PermissionAssignInput input);
    }
}