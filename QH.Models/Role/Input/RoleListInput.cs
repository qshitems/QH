using System;
using System.Collections.Generic;
using System.Text;

namespace QH.Models
{
   public class RoleListInput:PageBase
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
		public bool? Enabled { get; set; } 
    }
}
