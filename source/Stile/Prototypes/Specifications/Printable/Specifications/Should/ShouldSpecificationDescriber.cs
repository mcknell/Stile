#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.Builders.OfExpectations;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Has;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Is;
using Stile.Prototypes.Specifications.Builders.OfProcedures;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Expectations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
using Stile.Types.Expressions;
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
		public ShouldSpecificationDescriber([CanBeNull] ISource source)
			: base(source) {}

		public void Visit1<TSubject>(IExceptionFilter<TSubject> target)
		{
			Append(" ");
			AppendFormat(ShouldSpecifications.ShouldThrow, target.Description.Value);
		}

		public void Visit1<TSubject>(IFaultSpecification<TSubject> target)
		{
			IAcceptSpecificationVisitors lastTerm = target.ValidateArgumentIsNotNull().Xray.LastTerm;
			FillStackAndUnwind(lastTerm);
			Append(PrintDeadlineIfAny(target.Xray.Deadline));
		}

		public void Visit1<TSubject>(IProcedure<TSubject> procedure)
		{
			ISource<TSubject> source = procedure.Xray.Source ?? _source as ISource<TSubject>;
			if (source != null)
			{
				DescribeSourceAndProcedure(this, procedure, source, ShouldSpecifications.InstrumentedBy);
			}
			else
			{
				string type = typeof(TSubject).ToDebugString();
				if (IsSingleToken(type))
				{
					ILazyDescriptionOfLambda lambda = procedure.Xray.Lambda;
					AppendFormat("{0} {1}", ShouldSpecifications.AnyCaps, lambda.AliasParametersIntoBody(type));
				}
				else
				{
					AppendFormat(ShouldSpecifications.AnyType, type);
					AppendFormat(" {0}", ShouldSpecifications.InstrumentedBy);
				}
			}
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
			var expectationDescriber = new ShouldExpectationDescriber(_source);
			expectationDescriber.Visit2(expectation.ValidateArgumentIsNotNull());
			Append(expectationDescriber.ToString());
		}

		public void Visit2<TSubject, TResult>(IInstrument<TSubject, TResult> instrument)
		{
			Visit1(instrument);
		}

		public void Visit2<TSubject, TResult>(ISpecification<TSubject, TResult> target)
		{
			IAcceptSpecificationVisitors lastTerm = target.ValidateArgumentIsNotNull().Xray.LastTerm;
			FillStackAndUnwind(lastTerm);
			Append(PrintDeadlineIfAny(target.Xray.Deadline));
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

		public static string Describe<TSubject>(IFaultSpecification<TSubject> specification)
		{
			ISource<TSubject> source = specification.Xray.Procedure.Xray.Source;
			var describer = new ShouldSpecificationDescriber(source);
			var stack = new Stack<IFaultSpecification<TSubject>>();
			IFaultSpecification<TSubject> prior = specification;
			while (prior != null)
			{
				stack.Push(prior);
				prior = prior.Xray.Prior;
			}
			IFaultSpecification<TSubject> first = stack.Pop();
			describer.Visit1(first);
			if (stack.Count > 0)
			{
				describer.AppendFormat(" {0}",ShouldSpecifications.Initially);
			}
			while (stack.Count > 0)
			{
				IFaultSpecification<TSubject> popped = stack.Pop();
				describer.Append(string.Format("{0}{1}", Environment.NewLine, ShouldSpecifications.Then));
				describer.Visit1(popped.Xray.ExceptionFilter);
				describer.Append(PrintDeadlineIfAny(popped.Xray.Deadline));
				describer.AppendFormat(" {0}", ShouldSpecifications.WhenMeasuredAgain);
			}
			return describer.ToString();
		}

		public static string Describe<TSubject, TResult>(ISpecification<TSubject, TResult> specification)
		{
			ISource<TSubject> source = specification.Xray.Expectation.Xray.Instrument.Xray.Source;
			var describer = new ShouldSpecificationDescriber(source);
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
				describer.AppendFormat(" {0}", ShouldSpecifications.Initially);
			}
			while (stack.Count > 0)
			{
				ISpecification<TSubject, TResult> popped = stack.Pop();
				describer.Append(string.Format("{0}{1}", Environment.NewLine, ShouldSpecifications.Then));
				describer.Visit2(popped.Xray.Expectation);
				describer.Append(PrintDeadlineIfAny(popped.Xray.Deadline));
				describer.AppendFormat(" {0}", ShouldSpecifications.WhenMeasuredAgain);
			}
			return describer.ToString();
		}

		private static string PrintDeadlineIfAny(IDeadline deadline)
		{
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
