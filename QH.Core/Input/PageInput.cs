namespace QH.Core.Input
{
    /// <summary>
    /// 分页信息输入
    /// </summary>
    public class PageInput<T>
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
        /// 查询条件
        /// </summary>
        public T Filter { get; set; }

        /// <summary>
        /// 排序条件
        /// </summary>
        public string OrderBy { get; set; } = " id desc ";

        public string UserName { get; set; }

    }
}
