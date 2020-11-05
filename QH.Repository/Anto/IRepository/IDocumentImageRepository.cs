

/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：文档图片                                                    
*│　作    者：QSH                                              
*│　版    本：1.0                                               
*│　创建时间：2020-07-09 17:45:28                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： QH.IRepository                                   
*│　接口名称： IDocumentImageRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/

using QH.Models;
using System;
using System.Threading.Tasks;

namespace QH.IRepository
{
    public partial interface IDocumentImageRepository : IBaseRepository<DocumentImageEntity>
    {
	    
    }
}