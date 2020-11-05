/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：用户                                                    
*│　作    者：QSH                                              
*│　版    本：1.0                                      
*│　创建时间：2020-10-27 14:23:57                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: QH.Models                                  
*│　类    名：UserEntity                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QH.Models
{
    /// <summary>
    /// QSH
    /// 2020-10-27 14:23:57
    /// 用户
    /// </summary>

    public partial class UserEntity
    {
        [NotMapped]
        public List<int> RoleIds { get; set; }
    }
}
