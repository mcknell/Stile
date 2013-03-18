#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.Builders.OfExpectations;
using Stile.Prototypes.Specifications.Printable.Specifications.Should;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
using Stile.Readability;
using Stile.Types.Enumerables;
#endregion

namespace Stile.Prototypes.Specifications.Printable.Past
{
	public interface IPastEvaluationDescriber : IEvaluationVisitor {}

	public class PastEvaluationDescriber : Describer<IEvaluationVisitor, IAcceptEvaluationVisitors>,
		IPastEvaluationDescriber
	{
		public PastEvaluationDescriber([CanBeNull] ISource source)
			: base(source) {}

		public void Visit1<TSubject>(IFaultSpecification<TSubject> target)
		{
			var describer = new ShouldSpecificationDescriber(_source);
			describer.Visit1(target);
			Append(describer.ToString());
		}

		public void Visit1<TSubject>(IFaultEvaluation<TSubject> evaluation)
		{
			Visit1(evaluation.Xray.Specification);

			if (evaluation.Outcome != Outcome.Succeeded)
			{
				AppendFormat("{0}{1}", Environment.NewLine, PastTenseEvaluations.But);
			}
			if (evaluation.TimedOut)
			{
				AppendFormat(" {0}", PastTenseEvaluations.TimedOut);
			}
			else if (evaluation.Outcome == Outcome.Failed)
			{
				if (evaluation.Errors.None())
				{
					AppendFormat(" {0}", PastTenseEvaluations.NoExceptionThrown);
				}
			}
		}

		public void Visit2<TSubject, TResult>(IEvaluation<TSubject, TResult> evaluation)
		{
			Visit(evaluation.Xray.Specification);

			if (evaluation.Outcome == false)
			{
				Append(Environment.NewLine);
				Append(PastTenseEvaluations.But);
				AppendFormat(" {0} {1}", PastTenseEvaluations.Was, evaluation.Value.ToDebugString());
				if (evaluation.Xray.Specification.Xray.ExceptionFilter != null && evaluation.Errors.None())
				{
					AppendFormat(" {0} {1}", PastTenseEvaluations.And, PastTenseEvaluations.NoExceptionThrown);
				}
			}
		}

		public void Visit3<TSubject, TResult, TExpectationBuilder>(
			ISpecification<TSubject, TResult, TExpectationBuilder> specification)
			where TExpectationBuilder : class, IExpectationBuilder
		{
			Visit(specification);
		}

		public static string Describe<TSubject>(IFaultEvaluation<TSubject> evaluation)
		{
			var describer = new PastEvaluationDescriber(evaluation.Sample.Source);
			describer.Visit1(evaluation);
			return describer.ToString();
		}

		public static string Describe<TSubject, TResult>(IEvaluation<TSubject, TResult> evaluation)
		{
			var describer = new PastEvaluationDescriber(evaluation.Sample.Source);
			describer.Visit2(evaluation);
			return describer.ToString();
		}

		private void Visit<TSubject, TResult>(ISpecification<TSubject, TResult> target)
		{
			var describer = new ShouldSpecificationDescriber(_source);
			describer.Visit2(target);
			Append(describer.ToString());
		}
	}
}
