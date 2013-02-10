#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.SemanticModel;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfPredicates
{
	public interface IPredicateBuilderState<TSubject, out TResult>
	{
		IInstrument<TSubject, TResult> Instrument { get; }
		ISource<TSubject> Source { get; }
	}

	public class PredicateBuilderState<TSubject, TResult> : IPredicateBuilderState<TSubject, TResult>
	{
		public PredicateBuilderState([NotNull] IInstrument<TSubject, TResult> instrument,
			[NotNull] ISource<TSubject> source)
		{
			Instrument = instrument.ValidateArgumentIsNotNull();
			Source = source.ValidateArgumentIsNotNull();
		}

		public IInstrument<TSubject, TResult> Instrument { get; private set; }
		public ISource<TSubject> Source { get; private set; }
	}
}
