#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Prototypes.Specifications.Builders.OfExpectations;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Expectations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
using Stile.Types.Expressions;
using Stile.Types.Primitives;
using Stile.Types.Reflection;
#endregion

namespace Stile.Prototypes.Specifications.Printable.Should
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
			AppendSpecificationAfterthoughts(target.Xray);
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

		public void Visit1<TSubject>(ISource<TSubject> source)
		{
			// do nothin'
		}

		public void Visit2<TSubject, TResult>(IExceptionFilter<TSubject, TResult> target)
		{
			Append(" ");
			AppendFormat(ShouldSpecifications.ShouldThrow, target.Description.Value);
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
			ISpecificationState<TSubject, TResult> state = target.Xray;
			AppendSpecificationAfterthoughts(state);
		}

		public void Visit3<TSubject, TResult, TExpectationBuilder>(
			ISpecification<TSubject, TResult, TExpectationBuilder> specification)
			where TExpectationBuilder : class, IExpectationBuilder
		{
			Visit2(specification);
		}

		public static string Describe<TSubject>(IFaultSpecification<TSubject> specification)
		{
			ISource<TSubject> source = specification.Xray.Procedure.Xray.Source;
			var describer = new ShouldSpecificationDescriber(source);

			return DescribeCommon<IFaultSpecification<TSubject>, TSubject>(specification,
				describer,
				x => x.Xray.Prior,
				describer.Visit1,
				x => describer.Visit1(x.Xray.ExceptionFilter));
		}

		public static string Describe<TSubject, TResult>(ISpecification<TSubject, TResult> specification)
		{
			ISource<TSubject> source = specification.Xray.Expectation.Xray.Instrument.Xray.Source;
			var describer = new ShouldSpecificationDescriber(source);

			return DescribeCommon<ISpecification<TSubject, TResult>, TSubject>(specification,
				describer,
				x => x.Xray.Prior,
				describer.Visit2,
				x => describer.Visit2(x.Xray.Expectation));
		}

		private void AppendSpecificationAfterthoughts<TSubject>(ISpecificationState<TSubject> state)
		{
			Append(DeadlineIfAny(state.Deadline));
			Append(ReasonIfAny(state.Reason));
		}

		private static string DeadlineIfAny(IDeadline deadline)
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

		private static string DescribeCommon<TSpecification, TSubject>(TSpecification specification,
			ShouldSpecificationDescriber describer,
			Func<TSpecification, TSpecification> priorExtractor,
			Action<TSpecification> visitor,
			Action<TSpecification> repeatVisitor)
			where TSpecification : class, ISpecification<TSubject>, IHides<ISpecificationState<TSubject>>
		{
			var stack = new Stack<TSpecification>();
			TSpecification prior = specification;
			while (prior != null)
			{
				stack.Push(prior);
				prior = priorExtractor.Invoke(prior);
			}
			TSpecification first = stack.Pop();
			visitor.Invoke(first);
			if (stack.Count > 0)
			{
				describer.AppendFormat(" {0}", ShouldSpecifications.Initially);
			}
			while (stack.Count > 0)
			{
				TSpecification popped = stack.Pop();
				describer.Append(string.Format("{0}{1} {2},",
					Environment.NewLine,
					ShouldSpecifications.Then,
					ShouldSpecifications.WhenMeasuredAgain));
				repeatVisitor(popped);
				describer.AppendSpecificationAfterthoughts(popped.Xray);
			}
			return describer.ToString();
		}

		private static string ReasonIfAny(string because)
		{
			if (because != null)
			{
				string trim = because.Trim();
				if (trim.Length > 0)
				{
					return string.Format(", {0} {1}", ShouldSpecifications.Because, trim);
				}
			}
			return null;
		}
	}
}
