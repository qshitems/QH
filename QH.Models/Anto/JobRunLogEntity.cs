/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：QSH                                              
*│　版    本：1.0                                      
*│　创建时间：2020-10-28 10:21:20                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: QH.Models                                  
*│　类    名：JobRunLogEntity                                     
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
	/// 
	/// </summary>
	[Table("sys_JobRunLog")]
	public partial class JobRunLogEntity
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
		public string JobGroup {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string JobName {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(23)]
		public DateTime? StartTime {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(1)]
		public bool? Succ {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(500)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Exception {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(10)]
		public int? TotalSeconds {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(10)]
		public int? StatusCode {get;set;}

		/// <summary>
		///  
		/// </summary>
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string RequestMessage {get;set;}


	}
}
