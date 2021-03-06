﻿#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using NUnit.Framework;
using Stile.Readability;
#endregion

namespace Stile.Tests.Readability
{
	[TestFixture]
	public class NullFixture
	{
		[Test]
		public void IsNullOrDefault()
		{
			Assert.That(1.IsNullOrDefault(), Is.False);
			int? i = 3;
			Assert.That(i.IsNullOrDefault(), Is.False);
			i = null;
			Assert.That(i.IsNullOrDefault(), Is.True);
			string s = string.Empty;
			Assert.That(s.IsNullOrDefault(), Is.False);
			s = "blorp";
			Assert.That(s.IsNullOrDefault(), Is.False);
			s = Null.String;
			Assert.That(s.IsNullOrDefault(), Is.True);
		}
	}
}
