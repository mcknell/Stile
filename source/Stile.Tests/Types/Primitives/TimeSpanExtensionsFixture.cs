#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using NUnit.Framework;
using Stile.Types.Primitives;
#endregion

namespace Stile.Tests.Types.Primitives
{
	[TestFixture]
	public class TimeSpanExtensionsFixture
	{
		[Test]
		public void ToReadableUnits()
		{
			Assert.That(TimeSpan.FromDays(3).ToReadableUnits(), Is.EqualTo("3 days"));
			Assert.That(TimeSpan.FromDays(1).ToReadableUnits(), Is.EqualTo("1 day"));
			Assert.That(TimeSpan.FromHours(3).ToReadableUnits(), Is.EqualTo("3 hours"));
			Assert.That(TimeSpan.FromHours(1).ToReadableUnits(), Is.EqualTo("1 hour"));
			Assert.That(TimeSpan.FromMinutes(3).ToReadableUnits(), Is.EqualTo("3 minutes"));
			Assert.That(TimeSpan.FromMinutes(1).ToReadableUnits(), Is.EqualTo("1 minute"));
			Assert.That(TimeSpan.FromSeconds(2).ToReadableUnits(), Is.EqualTo("2 seconds"));
			Assert.That(TimeSpan.FromSeconds(1).ToReadableUnits(), Is.EqualTo("1 second"));
			Assert.That(TimeSpan.FromTicks(7).ToReadableUnits(), Is.EqualTo("7 ticks"));
			Assert.That(TimeSpan.FromTicks(1).ToReadableUnits(), Is.EqualTo("1 tick"));
			Assert.That(TimeSpan.Zero.ToReadableUnits(), Is.EqualTo("TimeSpan.Zero"));
			Assert.That(TimeSpan.FromSeconds(122).ToReadableUnits(), Is.EqualTo("00:02:02"));
			Assert.That(TimeSpan.FromMilliseconds(2002).ToReadableUnits(), Is.EqualTo("2.002 seconds"));
			Assert.That(TimeSpan.FromMilliseconds(2).ToReadableUnits(), Is.EqualTo("2ms"));
			TimeSpan span = TimeSpan.FromMilliseconds(2) + TimeSpan.FromTicks(7);
			Assert.That(span.ToReadableUnits(), Is.EqualTo("2.0007ms"));
		}
	}
}
