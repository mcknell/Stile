#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using NUnit.Framework;
using Stile.Prototypes.Specifications;
using Stile.Prototypes.Specifications.Builders.OfExpectations;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Is;
using Stile.Prototypes.Specifications.Builders.OfProcedures;
using Stile.Prototypes.Specifications.Printable;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
#endregion

namespace Stile.Tests.Prototypes.Specifications.Printable.Past
{
	[TestFixture]
	public class PrintablePastAcceptanceTests
	{
		[Test]
		public void Bound()
		{
			int i = 4;
			IFluentBoundExpectationBuilder<int, TypeCode> builder = Specify.For(() => i).That(x => x.GetTypeCode());
			IEvaluation<int, TypeCode> boundEvaluation = builder.Is.EqualTo(TypeCode.DBNull).Evaluate();
			Assert.That(boundEvaluation.ToPastTense(), Is.EqualTo(@"i.GetTypeCode() should be DBNull
but was Int32"));
			Assert.That(builder.Is.EqualTo(TypeCode.Int32).Evaluate().ToPastTense(),
				Is.EqualTo(@"i.GetTypeCode() should be Int32"));
		}
	}
}
