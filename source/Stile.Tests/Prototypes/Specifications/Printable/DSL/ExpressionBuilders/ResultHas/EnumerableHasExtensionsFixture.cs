#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using NUnit.Framework;
using Stile.Prototypes.Specifications.DSL.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.Printable;
using Stile.Prototypes.Specifications.Printable.Construction;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SpecificationBuilders;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SubjectBuilders;
#endregion

namespace Stile.Tests.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.ResultHas
{
	[TestFixture]
	public class EnumerableHasExtensionsFixture
	{
		[Test]
		public void HasAll()
		{
			var foo = new Foo<string>();
			AssertOutcome(
				Specify.ForAny<Foo<string>>().That(x => x.ToArray()).Of("").Has.All.ItemsSatisfying(
					x => string.IsNullOrWhiteSpace(x) == false),
				Outcome.Succeeded,
				foo);
			AssertOutcome(
				Specify.For(foo).That(x => x.ToArray()).Of("").Has.All.ItemsSatisfying(x => string.IsNullOrWhiteSpace(x) == false),
				Outcome.Succeeded,
				foo);
		}

		private void AssertOutcome<T1, T2>(IFluentSpecification<T1, T2> specification, Outcome outcome, T1 value)
		{
			IPrintableEvaluation<T2> evaluation = specification.Evaluate(value);
			Assert.That(evaluation.Result.Outcome, Is.EqualTo(outcome));
			Assert.That(evaluation.Emitted.Retrieved.Value, Is.Not.Empty);
		}
	}
}
