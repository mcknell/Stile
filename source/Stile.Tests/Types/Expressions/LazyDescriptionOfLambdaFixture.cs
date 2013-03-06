#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Linq.Expressions;
using NUnit.Framework;
using Stile.Types.Expressions;
#endregion

namespace Stile.Tests.Types.Expressions
{
	[TestFixture]
	public class LazyDescriptionOfLambdaFixture
	{
		[Test]
		public void AliasParametersIntoBody()
		{
			Expression<Func<int, string>> e = x => x.ToString("d");
			var descriptionOfLambda = new LazyDescriptionOfLambda(e);
			string body = descriptionOfLambda.AliasParametersIntoBody("i");
			Assert.That(body, Is.EqualTo("i.ToString(\"d\")"));
		}

		[Test]
		public void Parses()
		{
			Expression<Func<int, string>> e = x => x.ToString("d");
			var descriptionOfLambda = new LazyDescriptionOfLambda(e);
			Assert.That(descriptionOfLambda.Body, Is.EqualTo("x.ToString(\"d\")"));
			Assert.That(descriptionOfLambda.SubjectTokens, Is.EquivalentTo(new[] {"x"}));
		}
	}
}
