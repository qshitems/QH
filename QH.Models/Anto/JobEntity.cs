/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：QSH                                              
*│　版    本：1.0                                      
*│　创建时间：2020-10-28 10:12:25                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: QH.Models                                  
*│　类    名：JobEntity                                     
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
	/// 
	/// </summary>
	[Table("sys_Job")]
	public partial class JobEntity
	{
				/// <summary>
		///  
		/// </summary>
		[Key]
		public int Id {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Group {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Name {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(10)]
		public int? Status {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string CronExpression {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(200)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Description {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(23)]
		public DateTime? CreateTime {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(23)]
		public DateTime? NextOpTime {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(23)]
		public DateTime? LastOpTime {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(100)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string RequestUrl {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(10)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string RequestType {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string AuthKey {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(100)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string AuthValue {get;set;}


	}
}
