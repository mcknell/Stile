#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.SemanticModel.Expectations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
using Stile.Readability;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations.Is
{
	public static class IsExtensions
	{
		[System.Diagnostics.Contracts.Pure]
		public static TSpecification EqualTo<TSpecification, TSubject, TResult>(
			this IIs<TSpecification, TSubject, TResult> builder, TResult result)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
		{
			return Make(x => x.Equals(result), builder.Xray, result);
		}

		private static TSpecification Make<TSpecification, TSubject, TResult>(
			Expression<Predicate<TResult>> expression,
			IIsState<TSpecification, TSubject, TResult> state,
			TResult expected) where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
		{
			var equalTo = new EqualTo<TSpecification, TSubject, TResult>(state, expected);
			Expectation<TSubject, TResult> expectation = Expectation<TSubject>.From(expression,
				state.Negated,
				state.BuilderState.Instrument,
				equalTo);
			return state.BuilderState.Make(expectation);
		}
	}

	public interface IEqualToState<out TSpecification, TSubject, TResult> :
		IExpectationTerm<IIsState<TSpecification, TSubject, TResult>>
		where TSpecification : class, IChainableSpecification
	{
		Lazy<string> Description { get; }
		TResult Expected { get; }
	}

	public class EqualTo<TSpecification, TSubject, TResult> :
		ExpectationTerm<IIsState<TSpecification, TSubject, TResult>>,
		IEqualToState<TSpecification, TSubject, TResult>
		where TSpecification : class, IChainableSpecification
	{
		public EqualTo([NotNull] IIsState<TSpecification, TSubject, TResult> prior, TResult expected)
			: base(prior)
		{
			Expected = expected;
			Description = Expected.ToLazyDebugString();
		}

		public Lazy<string> Description { get; private set; }
		public TResult Expected { get; private set; }

		public override void Accept(IExpectationVisitor visitor)
		{
			visitor.Visit3(this);
		}

		public override TData Accept<TData>(IExpectationVisitor<TData> visitor, TData data)
		{
			return visitor.Visit3(this, data);
		}
	}
}
