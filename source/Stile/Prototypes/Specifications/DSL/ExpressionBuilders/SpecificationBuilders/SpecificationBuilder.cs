#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultHas;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultIs;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.Sources;
using Stile.Prototypes.Specifications.DSL.SemanticModel;
using Stile.Prototypes.Specifications.DSL.SemanticModel.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.DSL.ExpressionBuilders.SpecificationBuilders
{
	public interface ISpecificationBuilder {}

	public interface ISpecificationBuilder<out TResult, out THas, out TNegatableIs, out TIs, out TSpecifies> :
		ISpecificationBuilder
		where THas : class, IHas<TResult, TSpecifies>
		where TNegatableIs : class, INegatableIs<TResult, TIs, TSpecifies>
		where TIs : class, IIs<TResult, TSpecifies>
		where TSpecifies : class, ISpecification
	{
		THas Has { get; }
		TNegatableIs Is { get; }
	}

	public interface ISpecificationBuilder<out TSubject, out TResult, out THas, out TNegatableIs, out TIs,
		out TSpecifies> : ISpecificationBuilder<TResult, THas, TNegatableIs, TIs, TSpecifies>
		where THas : class, IHas<TResult, TSpecifies>
		where TNegatableIs : class, INegatableIs<TResult, TIs, TSpecifies>
		where TIs : class, IIs<TResult, TSpecifies>
		where TSpecifies : class, ISpecification<TSubject, TResult> {}

	public interface IBoundSpecificationBuilder : ISpecificationBuilder {}

	public interface IBoundSpecificationBuilder<out TSubject, out TResult, out THas, out TNegatableIs, out TIs,
		out TSpecifies, out TEvaluation> : IBoundSpecificationBuilder,
			ISpecificationBuilder<TSubject, TResult, THas, TNegatableIs, TIs, TSpecifies>
		where THas : class, IHas<TResult, TSpecifies>
		where TNegatableIs : class, INegatableIs<TResult, TIs, TSpecifies>
		where TIs : class, IIs<TResult, TSpecifies>
		where TSpecifies : class, ISpecification<TSubject, TResult, TEvaluation>
		where TEvaluation : class, IEvaluation<TResult> {}

	public interface IEmittingSpecificationBuilder : ISpecificationBuilder {}

	public interface IEmittingSpecificationBuilder<out TResult, out THas, out TNegatableIs, out TIs, out TSpecifies> :
		IEmittingSpecificationBuilder,
		ISpecificationBuilder<TResult, THas, TNegatableIs, TIs, TSpecifies>
		where THas : class, IHas<TResult, TSpecifies>
		where TNegatableIs : class, INegatableIs<TResult, TIs, TSpecifies>
		where TIs : class, IIs<TResult, TSpecifies>
		where TSpecifies : class, IEmittingSpecification {}

	public interface IEmittingSpecificationBuilder<out TSubject, out TResult, out THas, out TNegatableIs, out TIs,
		out TSpecifies, out TEvaluation, out TEmit> :
			IEmittingSpecificationBuilder<TResult, THas, TNegatableIs, TIs, TSpecifies>,
			ISpecificationBuilder<TSubject, TResult, THas, TNegatableIs, TIs, TSpecifies>
		where THas : class, IHas<TResult, TSpecifies>
		where TNegatableIs : class, INegatableIs<TSubject, TResult, TIs, TSpecifies>
		where TIs : class, IIs<TSubject, TResult, TSpecifies>
		where TSpecifies : class, IEmittingSpecification<TSubject, TResult>
		where TEvaluation : class, IEmittingEvaluation<TResult, TEmit> {}

	public interface ISpecificationBuilderState {}

	public interface ISpecificationBuilderState<TSubject, TResult, out TSource, out TSpecifies, TEvaluation,
		in TSpecificationArguments> : ISpecificationBuilderState
		where TSource : class, ISource<TSubject>
		where TSpecificationArguments : class, ISpecificationArguments<TResult, TEvaluation>
	{
		Lazy<Func<TSubject, TResult>> Instrument { get; }
		TSource Source { get; }

		TSpecifies Build(TSpecificationArguments arguments);
	}

	public interface ISpecificationArguments {}

	public interface ISpecificationArguments<in TResult, out TEvaluation> : ISpecificationArguments
	{
		[NotNull]
		Predicate<TResult> Accepter { get; }
		[CanBeNull]
		Func<TResult, Exception, TEvaluation> ExceptionFilter { get; }
	}

	public abstract class SpecificationBuilder<TSubject, TResult, THas, TNegatableIs, TIs, TSource, TSpecifies,
		TEvaluation, TEmit, TSpecificationArguments> :
			ISpecificationBuilder<TSubject, TResult, THas, TNegatableIs, TIs, TSpecifies>,
			ISpecificationBuilderState<TSubject, TResult, TSource, TSpecifies, TEvaluation, TSpecificationArguments>
		where TSource : class, ISource<TSubject>
		where THas : class, IHas<TResult, TSpecifies>
		where TNegatableIs : class, INegatableIs<TResult, TIs, TSpecifies>
		where TIs : class, IIs<TResult, TSpecifies>
		where TSpecifies : class, IEmittingSpecification<TSubject, TResult, TEvaluation, TEmit>
		where TEvaluation : class, IEmittingEvaluation<TResult, TEmit>
		where TSpecificationArguments : class, ISpecificationArguments<TResult, TEvaluation>
	{
		private readonly Lazy<Func<TSubject, TResult>> _instrument;
		private readonly Lazy<THas> _lazyHas;
		private readonly Lazy<TNegatableIs> _lazyIs;
		private readonly TSource _source;

		protected SpecificationBuilder([NotNull] TSource source, [NotNull] Lazy<Func<TSubject, TResult>> instrument)
		{
			_source = source.ValidateArgumentIsNotNull();
			_instrument = instrument.ValidateArgumentIsNotNull();
			_lazyHas = new Lazy<THas>(MakeHas);
			_lazyIs = new Lazy<TNegatableIs>(MakeIs);
		}

		public THas Has
		{
			get
			{
				THas value = _lazyHas.Value;
				return value;
			}
		}
		public Lazy<Func<TSubject, TResult>> Instrument
		{
			get { return _instrument; }
		}
		public TNegatableIs Is
		{
			get
			{
				TNegatableIs value = _lazyIs.Value;
				return value;
			}
		}
		public TSource Source
		{
			get { return _source; }
		}
		public abstract TSpecifies Build(TSpecificationArguments arguments);

		protected abstract THas MakeHas();
		protected abstract TNegatableIs MakeIs();
	}
}
