#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Prototypes.Specifications.SemanticModel;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfInstruments
{
	/// <summary>
	/// This non-generic interface exists for typewashing, such as in collections.
	/// </summary>
	public interface IBoundInstrumentBuilder : IInstrumentBuilder {}

	public interface IBoundInstrumentBuilder<out TSubject> : IBoundInstrumentBuilder,
		IInstrumentBuilder<TSubject>,
		IHides<IInstrumentBuilderState<TSubject>> {}

	public interface IBoundInstrumentBuilderState<out TSubject> : IInstrumentBuilderState<TSubject>
	{
		[NotNull]
		ISource<TSubject> Source { get; }
	}

	public class BoundInstrumentBuilder<TSubject> : InstrumentBuilder<TSubject>,
		IBoundInstrumentBuilder<TSubject>,
		IBoundInstrumentBuilderState<TSubject>
	{
		public BoundInstrumentBuilder(ISource<TSubject> source)
		{
			Source = source;
		}

		public ISource<TSubject> Source { get; private set; }
	}
}
