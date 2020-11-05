
/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：文档图片                                                    
*│　作    者：QSH                                              
*│　版    本：1.0                                      
*│　创建时间：2020-07-09 17:45:28                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: QH.Models                                  
*│　类    名：DocumentImageEntity                                     
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
	/// 文档图片
	/// </summary>
	[Table("ad_document_image")]
	public partial class DocumentImageEntity
	{
				/// <summary>
		/// 编号
		/// </summary>
		[Key]
		public int Id {get;set;}

		/// <summary>
		/// 用户Id
		/// </summary>
		[Required]
		[MaxLength(19)]
		public long? DocumentId {get;set;}

		/// <summary>
		/// 请求路径
		/// </summary>
		[MaxLength(255)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Url {get;set;}

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


	}
}
