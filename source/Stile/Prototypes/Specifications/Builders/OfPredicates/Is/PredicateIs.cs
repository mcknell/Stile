#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Prototypes.Specifications.SemanticModel;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfPredicates.Is
{
	public interface IPredicateIs {}

	public interface IResultPredicateIs<TSpecification, out TResult> : IPredicateIs
		where TSpecification : class, IResultSpecification<TResult> {}

	public interface IPredicateIs<TSpecification, TSubject, out TResult> : IResultPredicateIs<TSpecification, TResult>,
		IHides<IPredicateIsState<TSpecification, TSubject, TResult>>
		where TSpecification : class, ISpecification<TSubject, TResult> {}

	public interface INegatablePredicateIs : IPredicateIs {}

	public interface INegatablePredicateIs<TSpecification, TSubject, out TResult, out TNegated> : INegatablePredicateIs,
		IPredicateIs<TSpecification, TSubject, TResult>,
		INegatable<TNegated>
		where TSpecification : class, ISpecification<TSubject, TResult>
		where TNegated : class, IPredicateIs<TSpecification, TSubject, TResult> { }

	public interface IPredicateIsState<out TSpecification, TSubject, out TResult>
		where TSpecification : class, ISpecification<TSubject, TResult>
	{
		[NotNull]
		IInstrument<TSubject, TResult> Instrument { get; }
		Negated Negated { get; }
		[NotNull]
		ISource<TSubject> Source { get; }

		[NotNull]
		TSpecification Make(ICriterion<TResult> criterion);
	}

	public class PredicateIs<TSpecification, TSubject, TResult> :
		INegatablePredicateIs<TSpecification, TSubject, TResult, IPredicateIs<TSpecification, TSubject, TResult>>
		where TSpecification : class, ISpecification<TSubject, TResult>
	{
		public IPredicateIs<TSpecification, TSubject, TResult> Not { get; private set; }
		public IPredicateIsState<TSpecification, TSubject, TResult> Xray { get; private set; }
	}
}
