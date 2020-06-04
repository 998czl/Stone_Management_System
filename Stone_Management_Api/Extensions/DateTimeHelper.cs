using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stone_Management_Api.Extensions
{
	public class DateTimeHelper
	{
		/// <summary>
		/// 时间戳
		/// </summary>
		public static TimeSpan Timestamp => DateTime.UtcNow.Subtract(UtcDateTime.MinValue);
		/// <summary>
		/// 时间戳（毫秒）
		/// </summary>
		public static double TimestampOfMilliseconds => Timestamp.TotalMilliseconds;
		/// <summary>
		/// 时间戳（秒）
		/// </summary>
		public static double TimestampOfSeconds => Timestamp.TotalSeconds;
	}
}
