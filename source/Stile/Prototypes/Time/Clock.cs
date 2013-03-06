#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
#endregion

namespace Stile.Prototypes.Time
{
	public interface IClock
	{
		DateTime UtcNow { get; }
	}

	public class Clock : IClock
	{
		private static readonly Lazy<Clock> lazy = new Lazy<Clock>(() => new Clock());
		protected Clock() {}
		public static Clock SystemClock
		{
			get { return lazy.Value; }
		}
		public DateTime UtcNow
		{
			get { return DateTime.UtcNow; }
		}
	}
}
