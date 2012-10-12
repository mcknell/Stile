#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using NUnit.Framework;
using Stile.Prototypes.Specifications.DSL.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.Printable;
using Stile.Prototypes.Specifications.Printable.Construction;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.ResultHas;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SubjectBuilders;
#endregion

namespace Stile.Tests.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.ResultHas
{
	[TestFixture]
	public class HasExtensionsFixture
	{
		[Test]
		public void HashCode()
		{
			const DayOfWeek saturday = DayOfWeek.Saturday;
			int hashCode = saturday.ToString().GetHashCode();
			AssertHashCode(Specify.ThatAny<string>().Has.HashCode(7), Outcome.Failed, "foo");
			AssertHashCode(Specify.ThatAny<string>().Has.HashCode("foo".GetHashCode()), Outcome.Succeeded, "foo");
			AssertHashCode(Specify.ForAny<DayOfWeek>().That(x => x.ToString()).Has.HashCode(7), Outcome.Failed, saturday);
			AssertHashCode(Specify.ForAny<DayOfWeek>().That(x => x.ToString()).Has.HashCode(hashCode),
				Outcome.Succeeded,
				saturday);
		}

		private void AssertHashCode<T1, T2>(IFluentSpecification<T1, T2> specification, Outcome outcome, T1 value)
		{
			IPrintableEvaluation<T2> evaluation = specification.Evaluate(value);
			Assert.That(evaluation.Result.Outcome, Is.EqualTo(outcome));
			Assert.That(evaluation.Emitted.Retrieved.Value, Is.Not.Empty);
		}
	}
}
