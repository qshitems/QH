using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QH.Models
{
    /// <summary>
    /// 实体审计
    /// </summary>
    public class EntityBase
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Description("主键")]
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        [Description("是否删除")]
        public bool? IsDeleted { get; set; }

        /// <summary>
        /// 创建者Id
        /// </summary>
        [Description("创建者Id")]
        public long? CreatedUserId { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        [Description("创建者")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CreatedUserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        public DateTime? CreatedTime { get; set; }

        /// <summary>
        /// 修改者Id
        /// </summary>
        [Description("修改者Id")]
        public long? ModifiedUserId { get; set; }

        /// <summary>
        /// 修改者
        /// </summary>
        [Description("修改者")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ModifiedUserName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [Description("修改时间")]
        public DateTime? ModifiedTime { get; set; }
    }


}
