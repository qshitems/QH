/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口管理                                                    
*│　作    者：QSH                                              
*│　版    本：1.0                                      
*│　创建时间：2020-10-28 10:12:25                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: QH.Models                                  
*│　类    名：ApiEntity                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QH.Models
{
	/// <summary>
	/// QSH
	/// 2020-10-28 10:12:25
	/// 接口管理
	/// </summary>
	[Table("sys_api")]
	public partial class ApiEntity
	{
				/// <summary>
		/// 编号
		/// </summary>
		[Key]
		public int Id {get;set;}

		/// <summary>
		/// 所属模块
		/// </summary>
		[Required]
		[MaxLength(10)]
		public int? ParentId {get;set;}

		/// <summary>
		/// 接口命名
		/// </summary>
		[MaxLength(50)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Name {get;set;}

		/// <summary>
		/// 接口名称
		/// </summary>
		[MaxLength(500)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Label {get;set;}

		/// <summary>
		/// 接口地址
		/// </summary>
		[MaxLength(500)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Path {get;set;}

		/// <summary>
		/// 接口提交方法
		/// </summary>
		[MaxLength(50)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string HttpMethods {get;set;}

		/// <summary>
		/// 说明
		/// </summary>
		[MaxLength(500)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Description {get;set;}

		/// <summary>
		/// 排序
		/// </summary>
		[Required]
		[MaxLength(10)]
		public int? Sort {get;set;}

		/// <summary>
		/// 启用
		/// </summary>
		[Required]
		[MaxLength(1)]
		public bool? Enabled {get;set;}

		/// <summary>
		/// 版本
		/// </summary>
		[Required]
		[MaxLength(10)]
		public int? Version {get;set;}

		/// <summary>
		/// 是否删除
		/// </summary>
		[Required]
		[MaxLength(1)]
		public bool? IsDeleted {get;set;}

		/// <summary>
		/// 创建者Id
		/// </summary>
		[MaxLength(10)]
		public int? CreatedUserId {get;set;}

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
		[MaxLength(10)]
		public int? ModifiedUserId {get;set;}

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
