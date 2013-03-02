#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Prototypes.Specifications.SemanticModel;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations.Has
{
	public interface IHas {}

	public interface IHas<out TSpecification, TSubject, out TResult> : IHas,
		IHides<IHasState<TSpecification, TSubject, TResult>>
		where TSpecification : class, IChainableSpecification {}

	public interface IHasState<out TSpecification, TSubject, out TResult>
		where TSpecification : class, IChainableSpecification
	{
		[NotNull]
		IInstrument<TSubject, TResult> Instrument { get; }
		[CanBeNull]
		ISource<TSubject> Source { get; }

		[NotNull]
		TSpecification Make(ICriterion<TResult> criterion);
	}

	public class Has<TSpecification, TSubject, TResult> : IHas<TSpecification, TSubject, TResult>,
		IHasState<TSpecification, TSubject, TResult>
		where TSpecification : class, IChainableSpecification
	{
		private readonly Func<ICriterion<TResult>, IExceptionFilter<TSubject, TResult>, TSpecification> _specificationFactory;

		public Has([NotNull] IInstrument<TSubject, TResult> instrument,
			[NotNull] Func<ICriterion<TResult>, IExceptionFilter<TSubject, TResult>, TSpecification> specificationFactory,
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
			TSpecification specification = _specificationFactory.Invoke(criterion, null);
			return specification;
		}
	}
}
