#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultHas;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultIs;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.Sources;
using Stile.Prototypes.Specifications.DSL.SemanticModel;
using Stile.Prototypes.Specifications.DSL.SemanticModel.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.DSL.ExpressionBuilders.SpecificationBuilders
{
	public interface IEnumerableSpecificationBuilder : ISpecificationBuilder {}

	public interface IEnumerableSpecificationBuilder<out TResult, out TItem, out THas, out TNegatableIs, out TIs,
		out TSpecifies> : IEnumerableSpecificationBuilder,
			ISpecificationBuilder<TResult, THas, TNegatableIs, TIs, TSpecifies>
		where TResult : class, IEnumerable<TItem>
		where THas : class, IEnumerableHas<TResult, TItem, TSpecifies>
		where TNegatableIs : class, INegatableEnumerableIs<TResult, TItem, TIs, TSpecifies>
		where TIs : class, IEnumerableIs<TResult, TItem, TSpecifies>
		where TSpecifies : class, ISpecification {}

	public interface IEnumerableSpecificationBuilder<out TSubject, out TResult, out TItem, out THas, out TNegatableIs,
		out TIs, out TSource, out TSpecifies> :
			IEnumerableSpecificationBuilder<TResult, TItem, THas, TNegatableIs, TIs, TSpecifies>,
			ISpecificationBuilder<TSubject, TResult, THas, TNegatableIs, TIs, TSpecifies>
		where TResult : class, IEnumerable<TItem>
		where THas : class, IEnumerableHas<TSubject, TResult, TItem, TSource, TSpecifies>
		where TNegatableIs : class, INegatableEnumerableIs<TResult, TItem, TIs, TSpecifies>
		where TIs : class, IEnumerableIs<TResult, TItem, TSpecifies>
		where TSource : class, ISource<TSubject>
		where TSpecifies : class, ISpecification<TSubject, TResult> {}

	public abstract class EnumerableSpecificationBuilder<TSubject, TResult, TItem, THas, TNegatableIs, TIs, TSource,
		TSpecifies, TEvaluation, TEmit, TSpecificationArguments> :
			SpecificationBuilder
				<TSubject, TResult, THas, TNegatableIs, TIs, TSource, TSpecifies, TEvaluation, TEmit, TSpecificationArguments>,
			IEnumerableSpecificationBuilder<TSubject, TResult, TItem, THas, TNegatableIs, TIs, TSource, TSpecifies>
		where TResult : class, IEnumerable<TItem>
		where THas : class, IEnumerableHas<TSubject, TResult, TItem, TSource, TSpecifies>
		where TNegatableIs : class, INegatableEnumerableIs<TResult, TItem, TIs, TSpecifies>
		where TIs : class, IEnumerableIs<TResult, TItem, TSpecifies>
		where TSpecifies : class, IEmittingSpecification<TSubject, TResult, TEvaluation, TEmit>
		where TSource : class, ISource<TSubject>
		where TEvaluation : class, IEmittingEvaluation<TResult, TEmit>
		where TSpecificationArguments : class, ISpecificationArguments<TResult, TEvaluation>
	{
		protected EnumerableSpecificationBuilder([NotNull] TSource source,
			[NotNull] Lazy<Func<TSubject, TResult>> instrument)
			: base(source, instrument) {}
	}
}
