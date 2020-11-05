﻿using System.ComponentModel.DataAnnotations;

namespace QH.Models
{
    /// <summary>
    /// 修改密码
    /// </summary>
    public class UserChangePasswordInput
    {

        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 旧密码
        /// </summary>
        [Required(ErrorMessage = "请输入旧密码")]
        public string OldPassword { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        [Required(ErrorMessage = "请输入新密码")]
        public string NewPassword { get; set; }

        /// <summary>
        /// 确认新密码
        /// </summary>
        [Required(ErrorMessage = "请输入确认新密码")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public long Version { get; set; }
    }
}
