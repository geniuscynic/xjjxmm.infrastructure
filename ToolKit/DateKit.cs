using System;
using System.Text.RegularExpressions;

namespace XjjXmm.Infrastructure.ToolKit
{
	public static class DateKit
	{
		public static TimeSpan ToTimeSpan(this string value, TimeSpan defaultTimeSpan)
		{
			if (string.IsNullOrEmpty(value))
			{
				return defaultTimeSpan;
			}

			var format = "%d\\:%h\\:%m\\:%s";

			var pattern = "^(-?)([0-9]*[dD])?([0-9]*[hH])?([0-9]*[mM])?([0-9]*[sS])?$";

			Match m = Regex.Match(value, pattern);

			var test = m.Groups;

			var flag = test[1].Value;
			var day = test[2].Value.TimeSpanStringToInt(0);
			var hour = test[3].Value.TimeSpanStringToInt(0);
			var minute = test[4].Value.TimeSpanStringToInt(0);
			var second = test[5].Value.TimeSpanStringToInt(0);

			var results = $"{flag}{day}:{hour}:{minute}:{second}";

			var dt1 = DateTime.Now;
			
			var dt = DateTime.Now;
			
			var type = flag == "-"?-1:1;
			dt = dt.AddDays(type * day)
				.AddHours(type * hour)
				.AddMinutes(type * minute)
				.AddSeconds(type * second);
			
			//return TimeSpan.TryParseExact(results, format, null, out var timeSpan) ? timeSpan : defaultTimeSpan;
			return dt - dt1;
		}

		private static int TimeSpanStringToInt(this string value, int defaultValue)
		{
			value = value.ToLower()
				.Replace("d", "")
				.Replace("h", "")
				.Replace("m", "")
				.Replace("s", "");

			return int.TryParse(value, out var outValue) ? outValue : defaultValue;
		}


		/// <summary>
		/// 获取该时间相对于1970-01-01T00:00:00Z的秒数
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static long GetTotalSeconds(this in DateTime dt) =>
			new DateTimeOffset(dt).UtcDateTime.Ticks / 10_000_000L - 62135596800L;

		/// <summary>
		/// 获取该时间相对于1970-01-01T00:00:00Z的毫秒数
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static long ToTotalMilliseconds(this in DateTime dt)
		{

			// var s1 = (long) dt.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
			var s2 = new DateTimeOffset(dt).UtcDateTime.Ticks / 10000L - 62135596800000L;

			return s2;

		}
		/// <summary>
		/// 获取该时间相对于1970-01-01T00:00:00Z的时间
		/// </summary>
		/// <param name="millseconds"></param>
		/// <returns></returns>
		public static DateTime ToDateTime(this long millseconds)
		{
			//var timeSpan = new TimeSpan(millsecond);
			return new DateTime(1970, 1, 1).AddMilliseconds(millseconds);
		}
	}
}
