
using System;

namespace Discord_Channel_Importer.Extensions
{
	internal static class TimeSpanExtensions
	{
		/// <summary>
		/// Displays the TimeSpan in an easy to read format
		/// </summary>
		public static string ToReadableFormat(this TimeSpan span)
		{
			var dayCount = (int)span.TotalDays;
			var hourCount = (int)span.TotalHours;
			var minCount = (int)span.TotalMinutes;
			var secCount = (int)span.TotalSeconds;
			string estimatedDays = dayCount == 0 ? "" : string.Format("{0:%d} day{1},", span, dayCount != 1 ? "s" : "");
			string estimatedHours = hourCount == 0 ? "" : string.Format("{0:%h} hour{1}, ", span, hourCount != 1 ? "s" : "");
			string estimatedMins = minCount == 0 ? "" : string.Format("{0:%m} minute{1}, ", span, minCount != 1 ? "s" : "");
			string estimatedSecs = string.Format("{0:%s} second{1}", span, secCount != 1 ? "s" : "");

			return string.Format("{0}{1}{2}{3}", estimatedDays, estimatedHours, estimatedMins, estimatedSecs);
		}
	}
}
