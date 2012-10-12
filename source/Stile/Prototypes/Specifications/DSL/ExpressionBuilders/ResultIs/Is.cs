#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.SpecificationBuilders;
using Stile.Prototypes.Specifications.DSL.SemanticModel;
#endregion

namespace Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultIs
{
	public interface IIs {}

	public interface IIs<out TResult, out TSpecifies> : IIs
		where TSpecifies : class, ISpecification {}

	public interface IIs<out TSubject, out TResult, out TSpecifies> : IIs<TResult, TSpecifies>
		where TSpecifies : class, ISpecification<TSubject, TResult> {}

	public interface IIsState
	{
		Negated Negated { get; }
	}

	public interface IIsState<TSubject, TResult> : IIsState
	{
		Lazy<Func<TSubject, TResult>> Instrument { get; }
	}

	public interface IIsState<TSubject, TResult, out TSpecifies, in TInput> : IIsState<TSubject, TResult>
		where TSpecifies : class, ISpecification<TSubject, TResult>
		where TInput : class, ISpecificationInput<TSubject, TResult>
	{
		[NotNull]
		TSpecifies Make([NotNull] TInput input);
	}

	public abstract class Is<TSubject, TResult, TNegated, TSpecifies, TInput> :
		INegatableIs<TSubject, TResult, TNegated, TSpecifies>,
		IIsState<TSubject, TResult, TSpecifies, TInput>
		where TSpecifies : class, ISpecification<TSubject, TResult>
		where TNegated : class, IIs<TSubject, TResult, TSpecifies>
		where TInput : class, ISpecificationInput<TSubject, TResult>
	{
		protected Is(Negated negated, [NotNull] Lazy<Func<TSubject, TResult>> instrument)
		{
			Negated = negated;
			Instrument = instrument.ValidateArgumentIsNotNull();
		}

		public Lazy<Func<TSubject, TResult>> Instrument { get; private set; }
		public Negated Negated { get; private set; }
		public TNegated Not
		{
			get { return Factory(Negated.Invert(), Instrument); }
		}
		public abstract TSpecifies Make(TInput input);

		[NotNull]
		protected abstract TNegated Factory(Negated negated, Lazy<Func<TSubject, TResult>> instrument);
	}
}
