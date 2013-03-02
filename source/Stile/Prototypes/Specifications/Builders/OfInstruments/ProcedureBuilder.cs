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
	public interface IProcedureBuilder {}

	public interface IProcedureBuilder<out TSubject> : IProcedureBuilder,
		IHides<IProcedureBuilderState<TSubject>> {}

	public interface IProcedureBuilderState<out TSubject>
	{
		[CanBeNull]
		ISource<TSubject> Source { get; }
	}

	public class ProcedureBuilder<TSubject> : IProcedureBuilder<TSubject>,
		IProcedureBuilderState<TSubject>
	{
		public ProcedureBuilder(ISource<TSubject> source = null)
		{
			Source = source;
		}

		public ISource<TSubject> Source { get; private set; }
		public IProcedureBuilderState<TSubject> Xray
		{
			get { return this; }
		}
	}
}
