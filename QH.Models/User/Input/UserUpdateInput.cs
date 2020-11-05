
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QH.Models
{
    /// <summary>
    /// 修改
    /// </summary>
    public partial class UserUpdateInput
    {
     
        /// <summary>
        /// 主键Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        [Required(ErrorMessage = "请输入账号")]
        public string UserName { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        ///// <summary>
        ///// 头像
        ///// </summary>
        //public string Avatar { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public List<int> RoleIds { get; set; } 

        /// <summary>
        /// 版本
        /// </summary>
        public int Version { get; set; }
    }
}
