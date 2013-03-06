#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using NUnit.Framework;
using Stile.Prototypes.Specifications;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Is;
using Stile.Prototypes.Specifications.Builders.OfInstruments;
using Stile.Prototypes.Specifications.Printable.Past;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
#endregion

namespace Stile.Tests.Prototypes.Specifications.Printable.Past
{
	[TestFixture]
	public class PrintablePastAcceptanceTests
	{
		[Test]
		[Explicit]
		public void Bound()
		{
			int i = 4;
			IBoundEvaluation<int, TypeCode> boundEvaluation =
				Specify.For(() => i).That(x => x.GetTypeCode()).Is.EqualTo(TypeCode.DBNull).Evaluate();
			string pastTense = boundEvaluation.ToPastTense();
			Assert.That(pastTense, Is.EqualTo(@"Expected that i.GetTypeCode() would be TypeCode.DBNull
but was Int32"));
		}
	}
}
