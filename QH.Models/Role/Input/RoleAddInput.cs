namespace QH.Models
{
    /// <summary>
    /// 添加
    /// </summary>
    public class RoleAddInput
    {

        public int Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
       public string Description { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
		//public int? Enabled { get; set; }
    }
}
