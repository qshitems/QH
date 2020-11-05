/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：操作日志                                                    
*│　作    者：QSH                                              
*│　版    本：1.0                                      
*│　创建时间：2020-10-28 10:21:20                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: QH.Models                                  
*│　类    名：LoginLogEntity                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QH.Models
{
	/// <summary>
	/// QSH
	/// 2020-10-28 10:21:20
	/// 操作日志
	/// </summary>
	[Table("sys_login_log")]
	public partial class LoginLogEntity
	{
				/// <summary>
		/// 编号
		/// </summary>
		[Key]
		public long Id {get;set;}

		/// <summary>
		/// 昵称
		/// </summary>
		[MaxLength(60)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string NickName {get;set;}

		/// <summary>
		/// IP
		/// </summary>
		[MaxLength(100)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string IP {get;set;}

		/// <summary>
		/// 浏览器
		/// </summary>
		[MaxLength(100)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Browser {get;set;}

		/// <summary>
		/// 操作系统
		/// </summary>
		[MaxLength(100)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Os {get;set;}

		/// <summary>
		/// 设备
		/// </summary>
		[MaxLength(50)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Device {get;set;}

		/// <summary>
		/// 浏览器信息
		/// </summary>
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string BrowserInfo {get;set;}

		/// <summary>
		/// 耗时（毫秒）
		/// </summary>
		[Required]
		[MaxLength(19)]
		public long? ElapsedMilliseconds {get;set;}

		/// <summary>
		/// 操作状态
		/// </summary>
		[Required]
		[MaxLength(1)]
		public bool? Status {get;set;}

		/// <summary>
		/// 操作消息
		/// </summary>
		[MaxLength(500)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Msg {get;set;}

		/// <summary>
		/// 操作结果
		/// </summary>
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Result {get;set;}

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
