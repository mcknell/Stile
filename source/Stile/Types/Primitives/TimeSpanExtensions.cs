#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using Stile.Readability;
#endregion

namespace Stile.Types.Primitives
{
	public static class TimeSpanExtensions
	{
		private static readonly double TicksPerDay;
		private static readonly double TicksPerHour;
		private static readonly double TicksPerMinute;
		private static readonly double TicksPerSecond;
		private static readonly double TicksPerMillisecond;

		static TimeSpanExtensions()
		{
			var tick = (double) TimeSpan.FromTicks(1).Ticks;
			TicksPerMillisecond = tick / TimeSpan.FromMilliseconds(1).Ticks;
			TicksPerSecond = tick / TimeSpan.FromSeconds(1).Ticks;
			TicksPerMinute = tick / TimeSpan.FromMinutes(1).Ticks;
			TicksPerHour = tick / TimeSpan.FromHours(1).Ticks;
			TicksPerDay = tick / TimeSpan.FromDays(1).Ticks;
		}

		public static string ToReadableUnits(this TimeSpan timeSpan)
		{
			string readable = timeSpan < TimeSpan.Zero
				? LocalizableStrings.TimeSpanExtensions_ToReadableUnits_Minus + " "
				: string.Empty;
			TimeSpan duration = timeSpan.Duration();
			Func<long, string, string> round = (x, y) => "{0}{1} {2}".CurrentFormat(readable, x, x.Pluralize(y));
			var lazy = new Lazy<string>(() => "{0}{1}".CurrentFormat(readable, timeSpan.ToString()));
			if (timeSpan.Days > 0)
			{
				if (timeSpan.TotalDays - timeSpan.Days < TicksPerDay)
				{
					return round(timeSpan.Days, LocalizableStrings.TimeSpanExtensions_ToReadableUnits_Day);
				}
				return lazy.Value;
			}
			if (timeSpan.Hours > 0)
			{
				if (timeSpan.TotalHours - timeSpan.Hours < TicksPerHour)
				{
					return round(timeSpan.Hours, LocalizableStrings.TimeSpanExtensions_ToReadableUnits_Hour);
				}
				return lazy.Value;
			}
			if (timeSpan.Minutes > 0)
			{
				if (timeSpan.TotalMinutes - timeSpan.Minutes < TicksPerMinute)
				{
					return round(timeSpan.Minutes, LocalizableStrings.TimeSpanExtensions_ToReadableUnits_Minute);
				}
				return lazy.Value;
			}
			if (timeSpan.Seconds > 0)
			{
				if (timeSpan.TotalSeconds - timeSpan.Seconds < TicksPerSecond)
				{
					return round(timeSpan.Seconds, LocalizableStrings.TimeSpanExtensions_ToReadableUnits_Second);
				}
				if ((timeSpan.TotalMilliseconds % 1000) - timeSpan.Milliseconds < TicksPerMillisecond)
				{
					double seconds = Math.Round(timeSpan.TotalMilliseconds / 1000, 3);
					return "{0}{1} {2}".CurrentFormat(readable,
						seconds,
						LocalizableStrings.TimeSpanExtensions_ToReadableUnits_Seconds);
				}
				return lazy.Value;
			}
			if (timeSpan == TimeSpan.Zero)
			{
				return LocalizableStrings.TimeSpanExtensions_ToReadableUnits_TimeSpanZero;
			}
			if (duration < TimeSpan.FromMilliseconds(1))
			{
				return round(timeSpan.Ticks, LocalizableStrings.TimeSpanExtensions_ToReadableUnits_Tick);
			}
			return "{0}{1}{2}".CurrentFormat(readable,
				timeSpan.TotalMilliseconds,
				LocalizableStrings.TimeSpanExtensions_ToReadableUnits_Ms);
		}
	}
}
