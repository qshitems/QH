
/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：文档                                                    
*│　作    者：QSH                                              
*│　版    本：1.0                                      
*│　创建时间：2020-07-09 17:45:28                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: QH.Models                                  
*│　类    名：DocumentEntity                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QH.Models
{
	/// <summary>
	/// QSH
	/// 2020-07-09 17:45:28
	/// 文档
	/// </summary>
	[Table("ad_document")]
	public partial class DocumentEntity
	{
				/// <summary>
		/// 编号
		/// </summary>
		[Key]
		public int Id {get;set;}

		/// <summary>
		/// 父级节点
		/// </summary>
		[Required]
		[MaxLength(19)]
		public long? ParentId {get;set;}

		/// <summary>
		/// 名称
		/// </summary>
		[MaxLength(50)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Label {get;set;}

		/// <summary>
		/// 类型
		/// </summary>
		[Required]
		[MaxLength(10)]
		public int? Type {get;set;}

		/// <summary>
		/// 命名
		/// </summary>
		[MaxLength(500)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Name {get;set;}

		/// <summary>
		/// 内容
		/// </summary>
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Content {get;set;}

		/// <summary>
		/// Html
		/// </summary>
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Html {get;set;}

		/// <summary>
		/// 启用
		/// </summary>
		[Required]
		[MaxLength(1)]
		public Boolean? Enabled {get;set;}

		/// <summary>
		/// 打开组
		/// </summary>
		[MaxLength(1)]
		public Boolean? Opened {get;set;}

		/// <summary>
		/// 排序
		/// </summary>
		[MaxLength(10)]
		public int? Sort {get;set;}

		/// <summary>
		/// 描述
		/// </summary>
		[MaxLength(100)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Description {get;set;}

		/// <summary>
		/// 版本
		/// </summary>
		[Required]
		[MaxLength(19)]
		public long? Version {get;set;}

		/// <summary>
		/// 是否删除
		/// </summary>
		[Required]
		[MaxLength(1)]
		public Boolean? IsDeleted {get;set;}

		/// <summary>
		/// 创建者Id
		/// </summary>
		[MaxLength(19)]
		public long? CreatedUserId {get;set;}

		/// <summary>
		/// 创建者
		/// </summary>
		[MaxLength(50)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string CreatedUserName {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[MaxLength(23)]
		public DateTime? CreatedTime {get;set;}

		/// <summary>
		/// 修改者Id
		/// </summary>
		[MaxLength(19)]
		public long? ModifiedUserId {get;set;}

		/// <summary>
		/// 修改者
		/// </summary>
		[MaxLength(50)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string ModifiedUserName {get;set;}

		/// <summary>
		/// 修改时间
		/// </summary>
		[MaxLength(23)]
		public DateTime? ModifiedTime {get;set;}


	}
}
