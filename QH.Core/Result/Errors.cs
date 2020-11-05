using System.ComponentModel;

namespace QH.Core.Result
{
    public class Errors
    {
        /// <summary>
        /// 错误字段
        /// </summary>
        [Description("错误字段")]
        public string Id { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        [Description("错误信息")]
        public string Msg { get; set; }
    }
}