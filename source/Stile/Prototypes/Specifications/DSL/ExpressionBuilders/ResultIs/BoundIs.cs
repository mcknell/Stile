#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.Bound;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.Sources;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.SpecificationBuilders;
using Stile.Prototypes.Specifications.DSL.SemanticModel;
#endregion

namespace Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultIs
{
	public interface IBoundIsState : IIsState {}

	public interface IBoundIsState<TSubject, TResult, out TSource> : IBoundIsState,
		IIsState<TSubject, TResult>
		where TSource : class, ISource<TSubject>
	{
		TSource Source { get; }
	}

	public abstract class BoundIs<TSubject, TResult, TNegated, TSpecifies, TSource, TInput> :
		Is<TSubject, TResult, TNegated, TSpecifies, TInput>,
		IBoundIsState<TSubject, TResult, TSource>
		where TNegated : class, IIs<TSubject, TResult, TSpecifies>
		where TSpecifies : class, IBoundSpecification<TSubject, TResult>
		where TSource : class, ISource<TSubject>
		where TInput : class, ISpecificationInput<TSubject, TResult>
	{
		private readonly TSource _source;

		protected BoundIs(Negated negated, [NotNull] Lazy<Func<TSubject, TResult>> instrument, [NotNull] TSource source)
			: base(negated, instrument)
		{
			_source = source.ValidateArgumentIsNotNull();
		}

		public TSource Source
		{
			get { return _source; }
		}

		protected abstract TNegated BoundFactory(Negated negated, Lazy<Func<TSubject, TResult>> instrument, TSource source);

		protected override TNegated Factory(Negated negated, Lazy<Func<TSubject, TResult>> instrument)
		{
			return BoundFactory(negated, instrument, Source);
		}
	}
}
