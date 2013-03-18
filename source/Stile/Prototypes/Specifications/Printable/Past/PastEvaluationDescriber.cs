#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.Builders.OfExpectations;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Has;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Is;
using Stile.Prototypes.Specifications.Builders.OfInstruments;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.SemanticModel.Expectations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
using Stile.Readability;
using Stile.Types.Primitives;
#endregion

namespace Stile.Prototypes.Specifications.Printable.Past
{
	public interface IPastEvaluationDescriber : IEvaluationVisitor {}

	public class PastEvaluationDescriber : Describer<IEvaluationVisitor, IAcceptSpecificationVisitors>,
		IPastEvaluationDescriber
	{
		private readonly Func<Outcome, IPastExpectationDescriber> _expectationFormaterFactory;
		private readonly Outcome _outcome;

		public PastEvaluationDescriber(Outcome outcome,
			Func<Outcome, IPastExpectationDescriber> expectationFormatterFactory = null)
		{
			_outcome = outcome;
			_expectationFormaterFactory = expectationFormatterFactory ?? (x => new PastExpectationDescriber(x));
		}

		public void Visit1<TSubject>(IExceptionFilter<TSubject> target)
		{
			Append(" ");
			AppendFormat(PastTenseEvaluations.WouldThrow, target.Description.Value);
		}

		public void Visit1<TSubject>(IFaultSpecification<TSubject> target)
		{
			Append(PrintDeadlineIfAny(target.Xray.Deadline));
		}

		public void Visit1<TSubject>(IProcedure<TSubject> procedure)
		{
			if (procedure.Xray.Source != null)
			{
				DescribeSourceAndProcedure(this, procedure, PastTenseEvaluations.InstrumentedBy);
			}
			else
			{
				AppendFormat(PastTenseEvaluations.AnyType, typeof(TSubject).ToDebugString());
			}
		}

		public void Visit1<TSubject>(IProcedureBuilder<TSubject> builder)
		{
			throw new NotImplementedException();
		}

		public void Visit1<TSubject>(ISource<TSubject> source)
		{
			source.ValidateArgumentIsNotNull();
		}

		public void Visit1<TSubject>(IFaultEvaluation<TSubject> evaluation)
		{
			if (evaluation.Outcome == false)
			{
				AppendFormat("{0} {1} ", PastTenseEvaluations.Expected, PastTenseEvaluations.That);
			}
			FillStackAndUnwind(evaluation.Xray.Specification.Xray);

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

		public void Visit2<TSubject, TResult>(IExceptionFilter<TSubject, TResult> target)
		{
			throw new NotImplementedException();
		}

		public void Visit2<TSubject, TResult>(IExpectation<TSubject, TResult> expectation)
		{
			IPastExpectationDescriber expectationFormater = _expectationFormaterFactory.Invoke(_outcome);
			expectation.Accept(expectationFormater);
			Append(expectationFormater.ToString());
		}

		public void Visit2<TSubject, TResult>(IInstrument<TSubject, TResult> instrument)
		{
			if (instrument.Xray.Source != null)
			{
				DescribeSourceAndInstrument(this, instrument, PastTenseEvaluations.InstrumentedBy, Visit2);
			}
			else
			{
				AppendFormat(PastTenseEvaluations.AnyType, typeof(TSubject).ToDebugString());
			}
		}

		public void Visit2<TSubject, TResult>(ISpecification<TSubject, TResult> target)
		{
			Append(PrintDeadlineIfAny(target.Xray.Deadline));
		}

		public void Visit2<TSubject, TResult>(IEvaluation<TSubject, TResult> evaluation)
		{
			if (evaluation.Outcome == false)
			{
				AppendFormat("{0} {1} ", PastTenseEvaluations.Expected, PastTenseEvaluations.That);
			}
			FillStackAndUnwind(evaluation.Xray.Specification.Xray);

			if (evaluation.Outcome == false)
			{
				Append(Environment.NewLine);
				Append(PastTenseEvaluations.But);
				AppendFormat(" {0} {1}", PastTenseEvaluations.Was, evaluation.Value.ToDebugString());
			}
		}

		public void Visit3<TSpecification, TSubject, TResult>(IHas<TSpecification, TSubject, TResult> has)
			where TSpecification : class, IChainableSpecification
		{
			throw new NotImplementedException();
		}

		public void Visit3<TSpecification, TSubject, TResult>(IIs<TSpecification, TSubject, TResult> @is)
			where TSpecification : class, IChainableSpecification
		{
			throw new NotImplementedException();
		}

		public void Visit3<TSubject, TResult, TExpectationBuilder>(
			ISpecification<TSubject, TResult, TExpectationBuilder> specification)
			where TExpectationBuilder : class, IExpectationBuilder
		{
			Visit2(specification);
		}

		public void Visit3<TSpecification, TSubject, TResult>(
			IExpectationBuilder<TSpecification, TSubject, TResult> builder)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
		{
			throw new NotImplementedException();
		}

		public void Visit4<TSpecification, TSubject, TResult, TItem>(
			IEnumerableExpectationBuilder<TSpecification, TSubject, TResult, TItem> builder)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
			where TResult : class, IEnumerable<TItem>
		{
			throw new NotImplementedException();
		}

		public static string Describe<TSubject>(IFaultEvaluation<TSubject> evaluation)
		{
			var describer = new PastEvaluationDescriber(evaluation.Outcome);
			describer.Visit1(evaluation);
			return describer.ToString();
		}

		public static string Describe<TSubject, TResult>(IEvaluation<TSubject, TResult> evaluation)
		{
			var describer = new PastEvaluationDescriber(evaluation.Outcome);
			describer.Visit2(evaluation);
			return describer.ToString();
		}

		private static string PrintDeadlineIfAny(IDeadline deadline)
		{
			if (deadline != null)
			{
				if (deadline.Timeout > TimeSpan.Zero)
				{
					return string.Format(", {0} {1}", PastTenseEvaluations.InTimeLessThan, deadline.Timeout.ToReadableUnits());
				}
			}
			return null;
		}
	}
}
