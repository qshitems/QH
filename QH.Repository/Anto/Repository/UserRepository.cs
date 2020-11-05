
/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：用户接口实现                                                    
*│　作    者：QSH                                            
*│　版    本：1.0                                              
*│　创建时间：2020-07-09 17:45:28                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： QH.Repository                                  
*│　类    名： UserEntityRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using QH.Core.DbHelper;
using QH.Core.Options;
using QH.IRepository;
using QH.Models;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using QH.Core.Auth;
using QH.Core.Helpers;

namespace QH.Repository
{
    public partial class UserRepository : BaseRepository<UserEntity>, IUserRepository
    {
       

    }
}