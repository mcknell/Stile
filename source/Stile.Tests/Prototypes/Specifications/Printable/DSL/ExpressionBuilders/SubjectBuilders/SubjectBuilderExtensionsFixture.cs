#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Linq;
using NUnit.Framework;
using Stile.Prototypes.Specifications.DSL.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.Printable;
using Stile.Prototypes.Specifications.Printable.Construction;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.ResultIs;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SubjectBuilders;
#endregion

namespace Stile.Tests.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SubjectBuilders
{
	[TestFixture]
	public class SubjectBuilderExtensionsFixture
	{
		[Test]
		public void Of()
		{
			var ints = new Foo<int>();

			AssertEvaluation(Specify.For(ints).That(x => x.Jump()).Is.Not.EqualTo(0),
				Outcome.Succeeded);
		}

		private void AssertEvaluation<T1, T2>(IFluentBoundSpecification<T1, T2> specification, Outcome outcome)
		{
			IPrintableEvaluation<T2> evaluation = specification.Evaluate();
			Assert.That(evaluation.Result.Outcome, Is.EqualTo(outcome));
			Assert.That(evaluation.Emitted.Retrieved.Value, Is.Not.Empty);
		}
	}
}
