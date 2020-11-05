using System;
using System.Collections.Generic;
using System.Text;

namespace QH.Models
{
  public  class PermissionPageOutput
    {
        /// <summary>
        /// 权限Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 父级节点
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 权限类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 访问地址
        /// </summary>
        public string Path { get; set; }


        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 启用
        /// </summary>
		public bool Enabled { get; set; }

        public int Sort { get; set; }


        public string CheckArr { get; set; } = "0";
    }
}
