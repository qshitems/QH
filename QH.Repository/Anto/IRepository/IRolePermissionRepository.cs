

/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：角色权限                                                    
*│　作    者：QSH                                              
*│　版    本：1.0                                               
*│　创建时间：2020-07-09 17:45:28                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： QH.IRepository                                   
*│　接口名称： IRolePermissionRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/

using QH.Models;
using System;
using System.Threading.Tasks;

namespace QH.IRepository
{
    public partial interface IRolePermissionRepository : IBaseRepository<RolePermissionEntity>
    {
	     /// <summary>
        /// 逻辑删除返回影响的行数
        /// </summary>
        /// <param name="ids">需要删除的主键数组</param>
        /// <returns>影响的行数</returns>
        int DeleteLogical(int[] ids);
        /// <summary>
        /// 逻辑删除返回影响的行数（异步操作）
        /// </summary>
        /// <param name="ids">需要删除的主键数组</param>
        /// <returns>影响的行数</returns>
        Task<int> DeleteLogicalAsync(int[] ids);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UpdateByRolePermission(RolePermissionEntity model);
        /// <summary>
        /// 更新 （异步操作）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> UpdateByRolePermissionAsync(RolePermissionEntity model);
    }
}