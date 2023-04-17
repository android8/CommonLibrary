using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VsscHelper_CoreLibrary
{
	public static class HtmlHelperExtensions
	{
		private static string _CachedCurrentVersionDate;

		/// <summary>
		/// Return the Current Version from the AssemblyInfo.cs file.
		/// </summary>
		public static string CurrentVersion(this IHtmlHelper helper)
		{
			try
			{
				var version = Assembly.GetExecutingAssembly().GetName().Version;
				return version.ToString();
			}
			catch
			{
				return "?.?.?.?";
			}
		}

		public static string CurrentVersionDate(this IHtmlHelper helper)
		{
			try
			{
				if (_CachedCurrentVersionDate == null)
				{
					// Ignores concurrency issues - assuming not locking this is faster than 
					// locking it, and we don't care if it's set twice to the same value.
					var version = Assembly.GetExecutingAssembly().GetName().Version;
					var ticksForDays = TimeSpan.TicksPerDay * version.Build; // days since 1 January 2000
					var ticksForSeconds = TimeSpan.TicksPerSecond * 2 * version.Revision; // seconds since midnight, (multiply by 2 to get original)
					_CachedCurrentVersionDate = new DateTime(2000, 1, 1).Add(new TimeSpan(ticksForDays + ticksForSeconds)).ToString();
				}

				return _CachedCurrentVersionDate;
			}
			catch
			{
				return "Unknown Version Date";
			}
		}
	}
}
