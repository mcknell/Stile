#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Prototypes.Specifications.SemanticModel;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations.Has
{
	public interface IHas {}

	public interface IHas<out TSpecification, TSubject, TResult> : IHas,
		IHides<IHasState<TSpecification, TSubject, TResult>>
		where TSpecification : class, IChainableSpecification {}

	public interface IHasState<out TSpecification, TSubject, TResult> :
		IExpectationBuilderState<TSpecification, TSubject, TResult>
		where TSpecification : class, IChainableSpecification {}

	public class Has<TSpecification, TSubject, TResult> : IHas<TSpecification, TSubject, TResult>,
		IHasState<TSpecification, TSubject, TResult>
		where TSpecification : class, IChainableSpecification
	{
		private readonly ExpectationBuilder.SpecificationFactory<TSubject, TResult, TSpecification>
			_specificationFactory;

		public Has([NotNull] IExpectationBuilderState<TSpecification, TSubject, TResult> builderState)
		{
			Instrument = builderState.Instrument.ValidateArgumentIsNotNull();
			_specificationFactory = builderState.Make;
			Source = builderState.Source;
		}

		public IInstrument<TSubject, TResult> Instrument { get; private set; }
		public ISource<TSubject> Source { get; private set; }

		public IHasState<TSpecification, TSubject, TResult> Xray
		{
			get { return this; }
		}

		public TSpecification Make(ICriterion<TResult> criterion, IExceptionFilter<TSubject, TResult> filter = null)
		{
			TSpecification specification = _specificationFactory.Invoke(criterion, filter);
			return specification;
		}
	}
}
