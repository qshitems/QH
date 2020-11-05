/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：系统异常日志表                                                    
*│　作    者：QSH                                              
*│　版    本：1.0                                      
*│　创建时间：2020-10-28 10:12:25                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: QH.Models                                  
*│　类    名：LogEntity                                     
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
	/// 系统异常日志表
	/// </summary>
	[Table("Sys_Log")]
	public partial class LogEntity
	{
				/// <summary>
		/// 主键（默认值是newid()）
		/// </summary>
		[Key]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Id {get;set;}

		/// <summary>
		/// 日志所属应用
		/// </summary>
		[MaxLength(200)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Application {get;set;}

		/// <summary>
		/// 日志所属类
		/// </summary>
		[MaxLength(200)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Logger {get;set;}

		/// <summary>
		/// 日志级别
		/// </summary>
		[MaxLength(50)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Level {get;set;}

		/// <summary>
		/// 日志信息
		/// </summary>
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Message {get;set;}

		/// <summary>
		/// 异常信息
		/// </summary>
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Exception {get;set;}

		/// <summary>
		/// 日志所属方法
		/// </summary>
		[MaxLength(200)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Callsite {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[MaxLength(23)]
		public DateTime? CreateDate {get;set;}


	}
}
