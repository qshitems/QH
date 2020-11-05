/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：角色权限                                                    
*│　作    者：QSH                                              
*│　版    本：1.0                                      
*│　创建时间：2020-10-28 10:12:25                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: QH.Models                                  
*│　类    名：RolePermissionEntity                                     
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
	/// 角色权限
	/// </summary>
	[Table("sys_role_permission")]
	public partial class RolePermissionEntity
	{
				/// <summary>
		/// 编号
		/// </summary>
		[Key]
		public int Id {get;set;}

		/// <summary>
		/// 角色Id
		/// </summary>
		[Required]
		[MaxLength(10)]
		public int? RoleId {get;set;}

		/// <summary>
		/// 权限Id
		/// </summary>
		[Required]
		[MaxLength(10)]
		public int? PermissionId {get;set;}

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


	}
}
