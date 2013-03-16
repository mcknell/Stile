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
#endregion

namespace Stile.Prototypes.Specifications.Printable.Past
{
	public interface IPastEvaluationDescriber : IEvaluationVisitor {}

	public class PastEvaluationDescriber : Describer<IEvaluationVisitor, IAcceptEvaluationVisitors>,
		IPastEvaluationDescriber
	{
		private readonly Func<IPastExpectationDescriber> _expectationFormaterFactory;

		public PastEvaluationDescriber(Func<IPastExpectationDescriber> expectationFormatterFactory = null)
		{
			_expectationFormaterFactory = expectationFormatterFactory ?? (() => new PastExpectationDescriber());
		}

		public void Visit1<TSubject>(IExceptionFilter<TSubject> target)
		{
			throw new NotImplementedException();
		}

		public void Visit1<TSubject>(IProcedure<TSubject> procedure)
		{
			throw new NotImplementedException();
		}

		public void Visit1<TSubject>(IProcedureBuilder<TSubject> builder)
		{
			throw new NotImplementedException();
		}

		public void Visit1<TSubject>(ISource<TSubject> source)
		{
			source.ValidateArgumentIsNotNull();
		}

		public void Visit2<TSubject, TResult>(IExceptionFilter<TSubject, TResult> target)
		{
			throw new NotImplementedException();
		}

		public void Visit2<TSubject, TResult>(IExpectation<TSubject, TResult> expectation)
		{
			IPastExpectationDescriber expectationFormater = _expectationFormaterFactory.Invoke();
			expectation.Accept(expectationFormater);
			Append(expectationFormater.ToString());
		}

		public void Visit2<TSubject, TResult>(IInstrument<TSubject, TResult> instrument)
		{
			IProcedureState<TSubject> state = instrument.ValidateArgumentIsNotNull().Xray;
			Append(state.Lambda.Body);
		}

		public void Visit2<TSubject, TResult>(ISpecification<TSubject, TResult> target)
		{
			throw new NotImplementedException();
		}

		public void Visit2<TSubject, TResult>(IEvaluation<TSubject, TResult> evaluation)
		{
			IEvaluationState<TSubject, TResult> state = evaluation.ValidateArgumentIsNotNull().Xray;
			IExpectation<TSubject, TResult> expectation = state.Specification.Xray.Expectation;
			IInstrument<TSubject, TResult> instrument = expectation.Xray.Instrument;
			if (evaluation.Outcome)
			{
				DescribeSourceAndInstrument(this, instrument);
			}
			else
			{
				AppendFormat("{0} {1} ", PastTenseEvaluations.Expected, PastTenseEvaluations.That);
				DescribeSourceAndInstrument(this, instrument);
				Visit2(expectation);
				Append(Environment.NewLine);
				Append(PastTenseEvaluations.But);
			}
			AppendFormat(" {0} {1}", PastTenseEvaluations.Was, evaluation.Value.ToDebugString());
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
			throw new NotImplementedException();
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

		public static string Describe<TSubject, TResult>(IEvaluation<TSubject, TResult> evaluation)
		{
			var describer = new PastEvaluationDescriber();
			describer.Visit2(evaluation);
			return describer.ToString();
		}

		private static void DescribeSourceAndInstrument<TSubject, TResult>(PastEvaluationDescriber describer,
			IInstrument<TSubject, TResult> instrument)
		{
			DescribeSourceAndInstrument(describer, instrument, PastTenseEvaluations.InstrumentedBy, describer.Visit2);
		}
	}
}
