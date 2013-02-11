#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Stile.Prototypes.Specifications.Builders.OfExceptionFilters;
using Stile.Prototypes.Specifications.Builders.OfPredicates;
using Stile.Prototypes.Specifications.SemanticModel;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfInstruments
{
	public static class InstrumentBuilderExtensions
	{
		[Pure]
		public static IPredicateBuilder<ISpecification<TSubject, TResult>, TSubject, TResult> That
			<TSubject, TResult>(this IInstrumentBuilder<TSubject> builder,
				Expression<Func<TSubject, TResult>> expression)
		{
			var instrument = new Instrument<TSubject, TResult>(expression);
			Specification.Factory<ISpecification<TSubject, TResult>, TSubject, TResult> specificationFactory =
				Specification.MakeUnboundFactory<TSubject, TResult>();
			return new PredicateBuilder<ISpecification<TSubject, TResult>, TSubject, TResult>(instrument,
				specificationFactory);
		}

		[Pure]
		public static IPredicateBuilder<IBoundSpecification<TSubject, TResult>, TSubject, TResult> That
			<TSubject, TResult>(this IBoundInstrumentBuilder<TSubject> builder,
				Expression<Func<TSubject, TResult>> expression)
		{
			var instrument = new Instrument<TSubject, TResult>(expression);
			Specification.Factory<IBoundSpecification<TSubject, TResult>, TSubject, TResult> specificationFactory =
				Specification.MakeBoundFactory<TSubject, TResult>();
			return new PredicateBuilder<IBoundSpecification<TSubject, TResult>, TSubject, TResult>(instrument,
				specificationFactory,
				builder.Xray.Source);
		}

		[Pure]
		public static IExceptionFilterBuilder<IThrowingBoundSpecification<TSubject>, TSubject> That<TSubject>(
			this IBoundInstrumentBuilder<TSubject> builder, Expression<Action<TSubject>> expression)
		{
			var instrument = new VoidInstrument<TSubject>(expression);
			ISource<TSubject> source = builder.Xray.Source;
			return new ExceptionFilterBuilder<IThrowingBoundSpecification<TSubject>, TSubject>(instrument, source);
		}
	}
}
