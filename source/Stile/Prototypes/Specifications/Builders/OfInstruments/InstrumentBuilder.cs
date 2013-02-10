#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...

#endregion

namespace Stile.Prototypes.Specifications.Builders.OfInstruments
{
	/// <summary>
	/// This non-generic interface exists for typewashing, such as in collections.
	/// </summary>
	public interface IInstrumentBuilder {}

	public interface IInstrumentBuilder<out TSubject> : IInstrumentBuilder {}

	public interface IInstrumentBuilderState<out TSubject> {}

	public class InstrumentBuilder<TSubject> : IInstrumentBuilder<TSubject>,
		IInstrumentBuilderState<TSubject> {}
}
