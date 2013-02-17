#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.Builders.OfExceptionFilters;
using Stile.Prototypes.Specifications.Builders.OfPredicates;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfInstruments
{
	public static class InstrumentBuilderExtensions
	{
		[System.Diagnostics.Contracts.Pure]
		public static IPredicateBuilder<ISpecification<TSubject, TResult>, TSubject, TResult> That
			<TSubject, TResult>([NotNull] this IInstrumentBuilder<TSubject> builder,
				Expression<Func<TSubject, TResult>> expression)
		{
// ReSharper disable ReturnValueOfPureMethodIsNotUsed
			builder.ValidateArgumentIsNotNull();
// ReSharper restore ReturnValueOfPureMethodIsNotUsed
			var instrument = new Instrument<TSubject, TResult>(expression);
			return new PredicateBuilder<ISpecification<TSubject, TResult>, TSubject, TResult>(instrument,
				Specification<TSubject, TResult>.Make);
		}

		[System.Diagnostics.Contracts.Pure]
		public static IBoundPredicateBuilder<IBoundSpecification<TSubject, TResult>, TSubject, TResult> That
			<TSubject, TResult>([NotNull] this IBoundInstrumentBuilder<TSubject> builder,
				Expression<Func<TSubject, TResult>> expression)
		{
// ReSharper disable ReturnValueOfPureMethodIsNotUsed
			builder.ValidateArgumentIsNotNull();
// ReSharper restore ReturnValueOfPureMethodIsNotUsed
			var instrument = new Instrument<TSubject, TResult>(expression);
			return new BoundPredicateBuilder<IBoundSpecification<TSubject, TResult>, TSubject, TResult>(instrument,
				Specification<TSubject, TResult>.MakeBound,
				builder.Xray.Source);
		}

		[System.Diagnostics.Contracts.Pure]
		public static IExceptionFilterBuilder<IThrowingSpecification<TSubject>, TSubject> That<TSubject>(
			[NotNull] this IInstrumentBuilder<TSubject> builder, Expression<Action<TSubject>> expression)
		{
// ReSharper disable ReturnValueOfPureMethodIsNotUsed
			builder.ValidateArgumentIsNotNull();
// ReSharper restore ReturnValueOfPureMethodIsNotUsed
			var instrument = new ThrowingInstrument<TSubject>(expression);
			return ExceptionFilterBuilder<IThrowingSpecification<TSubject>, TSubject>.Make(instrument,
				ThrowingSpecification<TSubject>.Make);
		}

		[System.Diagnostics.Contracts.Pure]
		public static IExceptionFilterBuilder<IThrowingBoundSpecification<TSubject>, TSubject> That<TSubject>(
			[NotNull] this IBoundInstrumentBuilder<TSubject> builder, Expression<Action<TSubject>> expression)
		{
// ReSharper disable ReturnValueOfPureMethodIsNotUsed
			builder.ValidateArgumentIsNotNull();
// ReSharper restore ReturnValueOfPureMethodIsNotUsed
			var instrument = new ThrowingInstrument<TSubject>(expression);
			ISource<TSubject> source = builder.Xray.Source;
			return ExceptionFilterBuilder<IThrowingBoundSpecification<TSubject>, TSubject>.MakeBound(source, instrument,
				ThrowingSpecification<TSubject>.MakeBound);
		}
	}
}
