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

namespace Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultIs
{
	public interface IIs {}

	public interface IIs<out TResult, out TSpecifies> : IIs
		where TSpecifies : class, ISpecification {}

	public interface IIs<out TSubject, out TResult, out TSpecifies> : IIs<TResult, TSpecifies>
		where TSpecifies : class, ISpecification {}

	public interface IIsState
	{
		Negated Negated { get; }
	}

	public interface IIsState<TSubject, TResult> : IIsState
	{
		Lazy<Func<TSubject, TResult>> Instrument { get; }
	}

	public abstract class Is<TSubject, TResult, TSource, TNegated, TSpecifies> :
		INegatableIs<TSubject, TResult, TNegated, TSpecifies>,
		IIsState<TSubject, TResult>
		where TSource : class, ISource<TSubject>
		where TSpecifies : class, ISpecification<TSubject, TResult>
		where TNegated : class, IIs<TSubject, TResult, TSpecifies>
	{
		protected Is([NotNull] TSource source, Negated negated, [NotNull] Lazy<Func<TSubject, TResult>> instrument)
		{
			Source = source.ValidateArgumentIsNotNull();
			Negated = negated;
			Instrument = instrument.ValidateArgumentIsNotNull();
		}

		public Lazy<Func<TSubject, TResult>> Instrument { get; private set; }
		public Negated Negated { get; private set; }
		public TNegated Not
		{
			get { return Factory(Negated.Invert(), Instrument, Source); }
		}
		public TSource Source { get; private set; }

		[NotNull]
		protected abstract TNegated Factory(Negated negated, Lazy<Func<TSubject, TResult>> instrument, TSource source);
	}
}
