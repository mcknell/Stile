#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.Sources;
using Stile.Prototypes.Specifications.DSL.SemanticModel;
#endregion

namespace Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultHas
{
	public interface IHas {}

	public interface IHas<out TResult, out TSpecifies> : IHas
		where TSpecifies : class, ISpecification {}

	public interface IHas<out TSubject, out TResult, out TSource, out TSpecifies> : IHas<TResult, TSpecifies>
		where TSource : class, ISource<TSubject>
		where TSpecifies : class, ISpecification<TSubject, TResult> {}

	public interface IHasState<TSubject> {}

	public interface IHasState<TSubject, TResult, out TSource> : IHasState<TSubject>
		where TSource : class, ISource<TSubject>
	{
		[NotNull]
		Lazy<Func<TSubject, TResult>> Instrument { get; }
		[NotNull]
		TSource Source { get; }
	}

	public class Has<TSubject, TResult, TSource, TSpecifies> : IHas<TSubject, TResult, TSource, TSpecifies>,
		IHasState<TSubject, TResult, TSource>
		where TSource : class, ISource<TSubject>
		where TSpecifies : class, ISpecification<TSubject, TResult>
	{
		public Has([NotNull] TSource source, [NotNull] Lazy<Func<TSubject, TResult>> instrument)
		{
			Source = source.ValidateArgumentIsNotNull();
			Instrument = instrument.ValidateArgumentIsNotNull();
		}

		public Lazy<Func<TSubject, TResult>> Instrument { get; private set; }
		public TSource Source { get; private set; }
	}
}
