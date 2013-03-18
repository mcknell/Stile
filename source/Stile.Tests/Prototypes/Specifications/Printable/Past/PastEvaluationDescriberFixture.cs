#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using NUnit.Framework;
using Stile.Prototypes.Specifications.Printable.Past;
#endregion

namespace Stile.Tests.Prototypes.Specifications.Printable.Past
{
	[TestFixture]
	public class PastEvaluationDescriberFixture
	{
		[Test]
		public void IsSingleToken()
		{
			AssertSingleToken("x", true);
			AssertSingleToken("xYz", true);
			AssertSingleToken(" x", true);
			AssertSingleToken("x ", true);
			AssertSingleToken("x1", true);
			AssertSingleToken("@xy", true);
			AssertSingleToken("@xy1", true);
			AssertSingleToken("new string()", false);
			AssertSingleToken("x.Y", false);
			AssertSingleToken("x[i]", false);
			AssertSingleToken("i++", false);
			AssertSingleToken("Func<int>", true);
			AssertSingleToken("Func<int, string>", true);
			AssertSingleToken("Func<int, Foo<string>>", true);
		}

		private static void AssertSingleToken(string sourceName, bool expected)
		{
			Assert.That(new PastEvaluationDescriber(null).IsSingleToken(sourceName),
				Is.EqualTo(expected),
				string.Format("]{0}[ was expected to be {1}", sourceName, expected));
		}
	}
}
