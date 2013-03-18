#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using Stile.Prototypes.Specifications.Builders.OfExpectations;
using Stile.Prototypes.Specifications.Printable.Specifications.Should;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
using Stile.Readability;
#endregion

namespace Stile.Prototypes.Specifications.Printable.Past
{
	public interface IPastEvaluationDescriber : IEvaluationVisitor {}

	public class PastEvaluationDescriber : Describer<IEvaluationVisitor, IAcceptEvaluationVisitors>,
		IPastEvaluationDescriber
	{
		public void Visit1<TSubject>(IFaultSpecification<TSubject> target)
		{
			var describer = new ShouldSpecificationDescriber();
			describer.Visit1(target);
			Append(describer.ToString());
		}

		public void Visit1<TSubject>(IFaultEvaluation<TSubject> evaluation)
		{
			Visit1(evaluation.Xray.Specification);

			if (evaluation.Outcome == Outcome.Incomplete)
			{
				Append(Environment.NewLine);
				Append(PastTenseEvaluations.But);
				if (evaluation.TimedOut)
				{
					AppendFormat(" {0}", PastTenseEvaluations.TimedOut);
				}
			}
		}

		public void Visit2<TSubject, TResult>(IEvaluation<TSubject, TResult> evaluation)
		{
			Visit2(evaluation.Xray.Specification);

			if (evaluation.Outcome == false)
			{
				Append(Environment.NewLine);
				Append(PastTenseEvaluations.But);
				AppendFormat(" {0} {1}", PastTenseEvaluations.Was, evaluation.Value.ToDebugString());
			}
		}

		public void Visit3<TSubject, TResult, TExpectationBuilder>(
			ISpecification<TSubject, TResult, TExpectationBuilder> specification)
			where TExpectationBuilder : class, IExpectationBuilder
		{
			Visit2(specification);
		}

		public static string Describe<TSubject>(IFaultEvaluation<TSubject> evaluation)
		{
			var describer = new PastEvaluationDescriber();
			describer.Visit1(evaluation);
			return describer.ToString();
		}

		public static string Describe<TSubject, TResult>(IEvaluation<TSubject, TResult> evaluation)
		{
			var describer = new PastEvaluationDescriber();
			describer.Visit2(evaluation);
			return describer.ToString();
		}

		public void Visit2<TSubject, TResult>(ISpecification<TSubject, TResult> target)
		{
			var describer = new ShouldSpecificationDescriber();
			describer.Visit2(target);
			Append(describer.ToString());
		}
	}
}
