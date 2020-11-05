namespace QH.Models
{
    /// <summary>
    /// 修改
    /// </summary>
    public  class RoleUpdateInput :RoleAddInput
    {
        /// <summary>
        /// 接口Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public long Version { get; set; }
    }
}
