

/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口管理                                                    
*│　作    者：QSH                                              
*│　版    本：1.0                                               
*│　创建时间：2020-07-09 17:45:28                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： QH.IRepository                                   
*│　接口名称： IApiRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using QH.Models;
using System;
using System.Threading.Tasks;

namespace QH.IRepository
{
    public partial interface IApiRepository : IBaseRepository<ApiEntity>
    {
	 
    }
}