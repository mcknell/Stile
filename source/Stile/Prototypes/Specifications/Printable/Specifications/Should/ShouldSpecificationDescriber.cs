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
using Stile.Prototypes.Specifications.SemanticModel.Expectations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
using Stile.Types.Primitives;
using Stile.Types.Reflection;
#endregion

namespace Stile.Prototypes.Specifications.Printable.Specifications.Should
{
	public interface IShouldSpecificationDescriber : ISpecificationVisitor {}

	public class ShouldSpecificationDescriber :
		Describer<IShouldSpecificationDescriber, IAcceptSpecificationVisitors>,
		IShouldSpecificationDescriber
	{
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
			// do nothin'
		}

		public void Visit2<TSubject, TResult>(IExceptionFilter<TSubject, TResult> target)
		{
			throw new NotImplementedException();
		}

		public void Visit2<TSubject, TResult>(IExpectation<TSubject, TResult> expectation)
		{
			var expectationDescriber = new ShouldExpectationDescriber();
			expectationDescriber.Visit2(expectation.ValidateArgumentIsNotNull());
			Append(expectationDescriber.ToString());
		}

		public void Visit2<TSubject, TResult>(IInstrument<TSubject, TResult> instrument)
		{
			if (instrument.Xray.Source != null)
			{
				DescribeSourceAndInstrument(this, instrument, ShouldSpecifications.InstrumentedBy);
			}
			else
			{
				AppendFormat(ShouldSpecifications.AnyType, typeof(TSubject).ToDebugString());
			}
		}

		public void Visit2<TSubject, TResult>(ISpecification<TSubject, TResult> target)
		{
			IAcceptSpecificationVisitors lastTerm = target.ValidateArgumentIsNotNull().Xray.LastTerm;
			FillStackAndUnwind(lastTerm);
			Append(PrintDeadlineIfAny(target));
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

		public void Visit3<TSpecification, TSubject, TResult>(
			IExpectationBuilder<TSpecification, TSubject, TResult> builder)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
		{
			throw new NotImplementedException();
		}

		public void Visit3<TSubject, TResult, TExpectationBuilder>(
			ISpecification<TSubject, TResult, TExpectationBuilder> specification)
			where TExpectationBuilder : class, IExpectationBuilder
		{
			Visit2(specification);
		}

		public void Visit4<TSpecification, TSubject, TResult, TItem>(
			IEnumerableExpectationBuilder<TSpecification, TSubject, TResult, TItem> builder)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
			where TResult : class, IEnumerable<TItem>
		{
			throw new NotImplementedException();
		}

		public static string Describe<TSubject, TResult>(ISpecification<TSubject, TResult> specification)
		{
			var describer = new ShouldSpecificationDescriber();
			var stack = new Stack<ISpecification<TSubject, TResult>>();
			ISpecification<TSubject, TResult> prior = specification;
			while (prior != null)
			{
				stack.Push(prior);
				prior = prior.Xray.Prior;
			}
			ISpecification<TSubject, TResult> first = stack.Pop();
			describer.Visit2(first);
			if (stack.Count > 0)
			{
				describer.Append(" ");
				describer.AppendFormat(ShouldSpecifications.InitallyThen, Environment.NewLine);
			}
			while (stack.Count > 0)
			{
				ISpecification<TSubject, TResult> popped = stack.Pop();
				describer.Visit2(popped.Xray.Expectation);
				describer.Append(PrintDeadlineIfAny(popped));
				describer.AppendFormat(" {0}", ShouldSpecifications.WhenSampledAgain);
			}
			return describer.ToString();
		}

		private static string PrintDeadlineIfAny<TSubject, TResult>(ISpecification<TSubject, TResult> target)
		{
			IDeadline deadline = target.Xray.Deadline;
			if (deadline != null)
			{
				if (deadline.Timeout > TimeSpan.Zero)
				{
					return string.Format(", {0} {1}",
						ShouldSpecifications.MeasurableInLessThan,
						deadline.Timeout.ToReadableUnits());
				}
			}
			return null;
		}
	}
}
