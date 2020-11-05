/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：用户                                                    
*│　作    者：QSH                                              
*│　版    本：1.0                                      
*│　创建时间：2020-10-28 10:12:25                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: QH.Models                                  
*│　类    名：UserEntity                                     
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
	/// 用户
	/// </summary>
	[Table("sys_user")]
	public partial class UserEntity
	{
				/// <summary>
		/// 编号
		/// </summary>
		[Key]
		public int Id {get;set;}

		/// <summary>
		/// 账号
		/// </summary>
		[MaxLength(60)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string UserName {get;set;}

		/// <summary>
		/// 密码
		/// </summary>
		[MaxLength(60)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Password {get;set;}

		/// <summary>
		/// 昵称
		/// </summary>
		[MaxLength(60)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string NickName {get;set;}

		/// <summary>
		/// 头像
		/// </summary>
		[MaxLength(100)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Avatar {get;set;}

		/// <summary>
		/// 状态
		/// </summary>
		[Required]
		[MaxLength(10)]
		public int? Status {get;set;}

		/// <summary>
		/// 备注
		/// </summary>
		[MaxLength(500)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Remark {get;set;}

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

		/// <summary>
		/// 真实姓名
		/// </summary>
		[MaxLength(50)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string RealName {get;set;}

		/// <summary>
		/// 性别
		/// </summary>
		[MaxLength(10)]
		public int? Gender {get;set;}

		/// <summary>
		/// 生日
		/// </summary>
		[MaxLength(23)]
		public DateTime? Birthday {get;set;}

		/// <summary>
		/// 手机
		/// </summary>
		[MaxLength(20)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string MobilePhone {get;set;}

		/// <summary>
		/// 邮箱
		/// </summary>
		[MaxLength(50)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Email {get;set;}

		/// <summary>
		/// 签名
		/// </summary>
		[MaxLength(500)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Signature {get;set;}

		/// <summary>
		/// 地址
		/// </summary>
		[MaxLength(500)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Address {get;set;}

		/// <summary>
		/// 领导ID
		/// </summary>
		[MaxLength(10)]
		public int? CompanyId {get;set;}

		/// <summary>
		/// 组织ID
		/// </summary>
		[MaxLength(10)]
		public int? DepartmentId {get;set;}

		/// <summary>
		/// 是否启用
		/// </summary>
		[MaxLength(10)]
		public int? IsEnabled {get;set;}

		/// <summary>
		/// 排序码
		/// </summary>
		[MaxLength(10)]
		public int? SortCode {get;set;}


	}
}
