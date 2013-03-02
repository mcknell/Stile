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

namespace Stile.Prototypes.Specifications.Builders.OfInstruments
{
	/// <summary>
	/// This non-generic interface exists for typewashing, such as in collections.
	/// </summary>
	public interface IBoundProcedureBuilder : IProcedureBuilder {}

	public interface IBoundProcedureBuilder<out TSubject> : IBoundProcedureBuilder,
		IProcedureBuilder<TSubject>,
		IHides<IBoundProcedureBuilderState<TSubject>> {}

	public interface IBoundProcedureBuilderState<out TSubject> : IProcedureBuilderState<TSubject>
	{
		[NotNull]
		ISource<TSubject> Source { get; }
	}

	public class BoundProcedureBuilder<TSubject> : ProcedureBuilder<TSubject>,
		IBoundProcedureBuilder<TSubject>,
		IBoundProcedureBuilderState<TSubject>
	{
		public BoundProcedureBuilder([NotNull] ISource<TSubject> source)
			: base(source.ValidateArgumentIsNotNull()) {}

		public IBoundProcedureBuilderState<TSubject> Xray
		{
			get { return this; }
		}
	}
}
