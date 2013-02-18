#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfPredicates.Is
{
	public interface IIs {}

	public interface IResultIs<TSpecification, out TResult> : IIs
		where TSpecification : class, IResultSpecification<TResult> {}

	public interface IIs<TSpecification, TSubject, out TResult> :
		IResultIs<TSpecification, TResult>,
		IHides<IIsState<TSpecification, TSubject, TResult>>
		where TSpecification : class, ISpecification<TSubject, TResult> {}

	public interface INegatableIs : IIs {}

	public interface INegatableIs<TSpecification, TSubject, out TResult, out TNegated> :
		INegatableIs,
		IIs<TSpecification, TSubject, TResult>,
		INegatable<TNegated>
		where TSpecification : class, ISpecification<TSubject, TResult>
		where TNegated : class, IIs<TSpecification, TSubject, TResult> {}

	public interface IIsState<out TSpecification, TSubject, out TResult>
		where TSpecification : class, ISpecification<TSubject, TResult>
	{
		[NotNull]
		IInstrument<TSubject, TResult> Instrument { get; }
		Negated Negated { get; }
		[CanBeNull]
		ISource<TSubject> Source { get; }

		[NotNull]
		TSpecification Make(ICriterion<TResult> criterion);
	}

	public class Is<TSpecification, TSubject, TResult> :
		INegatableIs<TSpecification, TSubject, TResult, IIs<TSpecification, TSubject, TResult>>,
		IIsState<TSpecification, TSubject, TResult>
		where TSpecification : class, ISpecification<TSubject, TResult>
	{
		private readonly Specification.Factory<TSpecification, TSubject, TResult> _specificationFactory;

		public Is([NotNull] IInstrument<TSubject, TResult> instrument,
			Negated negated,
			[NotNull] Specification.Factory<TSpecification, TSubject, TResult> specificationFactory,
			ISource<TSubject> source = null)
		{
			Instrument = instrument.ValidateArgumentIsNotNull();
			Negated = negated;
			_specificationFactory = specificationFactory.ValidateArgumentIsNotNull();
			Source = source;
		}

		public IInstrument<TSubject, TResult> Instrument { get; private set; }
		public Negated Negated { get; private set; }
		public IIs<TSpecification, TSubject, TResult> Not
		{
			get
			{
				return new Is<TSpecification, TSubject, TResult>(Instrument,
					Negated.True,
					_specificationFactory,
					Source);
			}
		}
		public ISource<TSubject> Source { get; private set; }
		public IIsState<TSpecification, TSubject, TResult> Xray
		{
			get { return this; }
		}

		public TSpecification Make(ICriterion<TResult> criterion)
		{
			return _specificationFactory.Invoke(Source, Instrument, criterion);
		}
	}
}
