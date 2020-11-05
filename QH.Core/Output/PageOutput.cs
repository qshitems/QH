using System.Collections.Generic;

namespace QH.Core.Output
{
    /// <summary>
    /// 分页信息输出
    /// </summary>
    public class PageOutput<T>
    {
        /// <summary>
        /// 数据总数
        /// </summary>
        public long Count { get; set; }=0 ;
        /// <summary>
        /// 数据
        /// </summary>
        public IList<T> Data { get; set; }

        public string Msg { get; set; }

        public int Code { get; set; }=0 ;
    }
}
