
using System.ComponentModel;

namespace QH.Models
{
    public interface IEntityDto
    {
    }

    public class EntityDto<TPrimaryKey> : IEntityDto
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Description("编号")]
        public virtual TPrimaryKey id { get; set; }
    }

    public class EntityDto : EntityDto<long>
    {

    }
}
