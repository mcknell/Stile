#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Expectations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations.Enumerable
{
	public interface IFluentEnumerableExpectationBuilder<TSubject, TResult, TItem> :
		IEnumerableExpectationBuilder
			<ISpecification<TSubject, TResult, IFluentEnumerableExpectationBuilder<TSubject, TResult, TItem>>, TSubject
				, TResult, TItem>
		where TResult : class, IEnumerable<TItem> {}

	public class FluentEnumerableExpectationBuilder<TSubject, TResult, TItem> :
		EnumerableExpectationBuilder
			<ISpecification<TSubject, TResult, IFluentEnumerableExpectationBuilder<TSubject, TResult, TItem>>, TSubject
				, TResult, TItem, IFluentEnumerableExpectationBuilder<TSubject, TResult, TItem>>,
		IFluentEnumerableExpectationBuilder<TSubject, TResult, TItem>
		where TResult : class, IEnumerable<TItem>
	{
		private readonly IExpectationBuilderState<IChainableSpecification, TSubject, TResult> _state;

		public FluentEnumerableExpectationBuilder(
			[NotNull] IExpectationBuilderState<IChainableSpecification, TSubject, TResult> state,
			[CanBeNull] ISpecification<TSubject, TResult, IFluentEnumerableExpectationBuilder<TSubject, TResult, TItem>> prior)
			: base(state, prior)
		{
			_state = state;
		}

		public override object ChainFrom(object specification)
		{
			return new FluentEnumerableExpectationBuilder<TSubject, TResult, TItem>(_state,
				specification as
					ISpecification<TSubject, TResult, IFluentEnumerableExpectationBuilder<TSubject, TResult, TItem>>);
		}

		protected override
			Func
				<IExpectation<TSubject, TResult>, IExceptionFilter<TSubject, TResult>,
					ISpecification<TSubject, TResult, IFluentEnumerableExpectationBuilder<TSubject, TResult, TItem>>>
			SpecFactory
		{
			get { return MakeUnboundSpecification; }
		}
	}
}
