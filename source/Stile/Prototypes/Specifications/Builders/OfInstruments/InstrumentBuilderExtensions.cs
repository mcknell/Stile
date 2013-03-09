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
using Stile.Prototypes.Specifications.Builders.OfExpectations;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfInstruments
{
	public static class InstrumentBuilderExtensions
	{
		[System.Diagnostics.Contracts.Pure]
		public static IFluentExpectationBuilder<TSubject, TResult> That<TSubject, TResult>(
			[NotNull] this IProcedureBuilder<TSubject> builder, Expression<Func<TSubject, TResult>> expression)
		{
// ReSharper disable ReturnValueOfPureMethodIsNotUsed
			builder.ValidateArgumentIsNotNull();
// ReSharper restore ReturnValueOfPureMethodIsNotUsed
			var instrument = new Instrument<TSubject, TResult>(expression, builder.Xray.Source);
			return new FluentExpectationBuilder<TSubject, TResult>(instrument);
		}

		[System.Diagnostics.Contracts.Pure]
		public static IExceptionFilterBuilder<IVoidSpecification<TSubject>, TSubject> That<TSubject>(
			[NotNull] this IProcedureBuilder<TSubject> builder, Expression<Action<TSubject>> expression)
		{
// ReSharper disable ReturnValueOfPureMethodIsNotUsed
			builder.ValidateArgumentIsNotNull();
// ReSharper restore ReturnValueOfPureMethodIsNotUsed
			var procedure = new Procedure<TSubject>(expression);
			return ExceptionFilterBuilder<IVoidSpecification<TSubject>, TSubject>.Make(procedure,
				VoidSpecification<TSubject>.Make);
		}

		[System.Diagnostics.Contracts.Pure]
		public static IExceptionFilterBuilder<IVoidBoundSpecification<TSubject>, TSubject> That<TSubject>(
			[NotNull] this IBoundProcedureBuilder<TSubject> builder, Expression<Action<TSubject>> expression)
		{
// ReSharper disable ReturnValueOfPureMethodIsNotUsed
			builder.ValidateArgumentIsNotNull();
// ReSharper restore ReturnValueOfPureMethodIsNotUsed
			ISource<TSubject> source = builder.Xray.Source.ValidateArgumentIsNotNull();
			IProcedure<TSubject> procedure = new Procedure<TSubject>(expression, source);
			return ExceptionFilterBuilder<IVoidBoundSpecification<TSubject>, TSubject>.MakeBound(source,
				procedure,
				VoidSpecification<TSubject>.MakeBound);
		}

		[System.Diagnostics.Contracts.Pure]
		public static IFluentBoundExpectationBuilder<TSubject, TResult> That<TSubject, TResult>(
			[NotNull] this IBoundProcedureBuilder<TSubject> builder, Expression<Func<TSubject, TResult>> expression)
		{
// ReSharper disable ReturnValueOfPureMethodIsNotUsed
			builder.ValidateArgumentIsNotNull();
// ReSharper restore ReturnValueOfPureMethodIsNotUsed
			var instrument = new Instrument<TSubject, TResult>(expression, builder.Xray.Source);
			return new FluentBoundExpectationBuilder<TSubject, TResult>(instrument, builder.Xray);
		}
	}
}
