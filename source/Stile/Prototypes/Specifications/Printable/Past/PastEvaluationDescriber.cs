#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Linq;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.Builders.OfExpectations;
using Stile.Prototypes.Specifications.Printable.Should;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
using Stile.Readability;
using Stile.Types.Enumerables;
using Stile.Types.Primitives;
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
			var describer = new ShouldSpecificationDescriber(Source);
			describer.Visit1(target);
			Append(describer.ToString());
		}

		public void Visit1<TSubject>(IFaultEvaluation<TSubject> target)
		{
			Visit1(target.Xray.Specification);

			if (target.Outcome != Outcome.Succeeded)
			{
				AppendFormat("{0}{1}", Environment.NewLine, PastTenseEvaluations.But);
			}
			if (target.TimedOut)
			{
				AppendFormat(" {0}", PastTenseEvaluations.TimedOut);
			}
			else if (target.Outcome == Outcome.Failed)
			{
				if (target.Errors.None())
				{
					AppendFormat(" {0}", PastTenseEvaluations.NoExceptionThrown);
				}
				else
				{
					string separator = ", {0} ".InvariantFormat(PastTenseEvaluations.Then);
					string errorTypes = string.Join(separator, target.Errors.Select(x => x.Exception.GetType().Name));
					AppendFormat(" {0} {1}", PastTenseEvaluations.Threw, errorTypes);
				}
			}
		}

		public void Visit2<TSubject, TResult>(IEvaluation<TSubject, TResult> target)
		{
			Visit(target.Xray.Specification);

			if (target.Outcome == false)
			{
				Append(Environment.NewLine);
				Append(PastTenseEvaluations.But);
				AppendFormat(" {0} {1}", PastTenseEvaluations.Was, target.Value.ToDebugString());
				if (target.Xray.Specification.Xray.ExceptionFilter != null && target.Errors.None())
				{
					AppendFormat(" {0} {1}", PastTenseEvaluations.And, PastTenseEvaluations.NoExceptionThrown);
				}
			}
		}

		public void Visit3<TSubject, TResult, TExpectationBuilder>(
			ISpecification<TSubject, TResult, TExpectationBuilder> target)
			where TExpectationBuilder : class, IExpectationBuilder
		{
			Visit(target);
		}

		public static string Describe<TSubject>(IFaultEvaluation<TSubject> evaluation)
		{
			ISource<TSubject> source = GetSource(evaluation.ValidateArgumentIsNotNull());
			var describer = new PastEvaluationDescriber(source);
			describer.Visit1(evaluation);
			return describer.ToString();
		}

		public static string Describe<TSubject, TResult>(IEvaluation<TSubject, TResult> evaluation)
		{
			ISource<TSubject> source = GetSource(evaluation.ValidateArgumentIsNotNull());
			var describer = new PastEvaluationDescriber(source);
			describer.Visit2(evaluation);
			return describer.ToString();
		}

		private static ISource<TSubject> GetSource<TSubject>(IEvaluation<TSubject> evaluation)
		{
			ISample<TSubject> sample = evaluation.Sample;
			ISource<TSubject> source;
			if (sample == null && evaluation.TimedOut)
			{
				source = null;
			}
			else
			{
				source = sample.Source;
			}
			return source;
		}

		private void Visit<TSubject, TResult>(ISpecification<TSubject, TResult> target)
		{
			var describer = new ShouldSpecificationDescriber(Source);
			describer.Visit2(target);
			Append(describer.ToString());
		}
	}
}
