using System;
using System.Collections.Generic;
using System.Text;

namespace QH.Models
{
   public class PageBase
    {
        /// <summary>
        /// 当前页标
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// 每页大小
        /// </summary>
        public int Limit { set; get; } = 10;

        /// <summary>
        /// 排序字段
        /// </summary>
        public string Field { get; set; } = "id";
        /// <summary>
        /// 排序类型：desc（降序）、asc（升序）、null（空对象，默认排序）
        /// </summary>
        public string Order { get; set; } = "desc";

    }
}
