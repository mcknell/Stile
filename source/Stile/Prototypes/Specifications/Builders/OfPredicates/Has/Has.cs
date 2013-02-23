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

namespace Stile.Prototypes.Specifications.Builders.OfPredicates.Has
{
	public interface IHas {}

	public interface IHas<out TSpecification, TSubject, out TResult> : IHas,
		IHides<IHasState<TSpecification, TSubject, TResult>>
		where TSpecification : class, ISpecification<TSubject, TResult> {}

	public interface IHasState<out TSpecification, TSubject, out TResult>
		where TSpecification : class, ISpecification<TSubject, TResult>
	{
		[NotNull]
		IInstrument<TSubject, TResult> Instrument { get; }
		[CanBeNull]
		ISource<TSubject> Source { get; }

		[NotNull]
		TSpecification Make(ICriterion<TResult> criterion);
	}

	public class Has<TSpecification, TSubject, TResult> :
		IHas<TSpecification, TSubject, TResult>,
		IHasState<TSpecification, TSubject, TResult>
		where TSpecification : class, ISpecification<TSubject, TResult>
	{
		private readonly Specification.Factory<TSpecification, TSubject, TResult> _specificationFactory;

		public Has([NotNull] IInstrument<TSubject, TResult> instrument,
			[NotNull] Specification.Factory<TSpecification, TSubject, TResult> specificationFactory,
			ISource<TSubject> source = null)
		{
			Instrument = instrument.ValidateArgumentIsNotNull();
			_specificationFactory = specificationFactory.ValidateArgumentIsNotNull();
			Source = source;
		}

		public IInstrument<TSubject, TResult> Instrument { get; private set; }
		public ISource<TSubject> Source { get; private set; }

		public IHasState<TSpecification, TSubject, TResult> Xray
		{
			get { return this; }
		}

		public TSpecification Make(ICriterion<TResult> criterion)
		{
			TSpecification specification = _specificationFactory.Invoke(Source, Instrument, criterion);
			return specification;
		}
	}
}