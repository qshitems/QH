/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：权限                                                    
*│　作    者：QSH                                              
*│　版    本：1.0                                      
*│　创建时间：2020-10-28 10:12:25                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: QH.Models                                  
*│　类    名：PermissionEntity                                     
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
	/// 权限
	/// </summary>
	[Table("sys_permission")]
	public partial class PermissionEntity
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
		[MaxLength(10)]
		public int? ParentId {get;set;}

		/// <summary>
		/// 权限名称
		/// </summary>
		[MaxLength(50)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Label {get;set;}

		/// <summary>
		/// 权限编码
		/// </summary>
		[MaxLength(550)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Code {get;set;}

		/// <summary>
		/// 权限类型
		/// </summary>
		[Required]
		[MaxLength(10)]
		public int? Type {get;set;}

		/// <summary>
		/// 视图
		/// </summary>
		[MaxLength(10)]
		public int? ViewId {get;set;}

		/// <summary>
		/// 接口
		/// </summary>
		[MaxLength(10)]
		public int? ApiId {get;set;}

		/// <summary>
		/// 菜单访问地址
		/// </summary>
		[MaxLength(500)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Path {get;set;}

		/// <summary>
		/// 图标
		/// </summary>
		[MaxLength(100)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Icon {get;set;}

		/// <summary>
		/// 隐藏
		/// </summary>
		[Required]
		[MaxLength(1)]
		public bool? Hidden {get;set;}

		/// <summary>
		/// 启用
		/// </summary>
		[Required]
		[MaxLength(1)]
		public bool? Enabled {get;set;}

		/// <summary>
		/// 可关闭
		/// </summary>
		[MaxLength(1)]
		public bool? Closable {get;set;}

		/// <summary>
		/// 打开组
		/// </summary>
		[MaxLength(1)]
		public bool? Opened {get;set;}

		/// <summary>
		/// 打开新窗口
		/// </summary>
		[MaxLength(1)]
		public bool? NewWindow {get;set;}

		/// <summary>
		/// 链接外显
		/// </summary>
		[MaxLength(1)]
		public bool? External {get;set;}

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
