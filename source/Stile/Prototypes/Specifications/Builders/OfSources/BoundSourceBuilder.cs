#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using Stile.Prototypes.Specifications.SemanticModel;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfSources
{
	public interface IBoundSourceBuilder<TSubject> : ISourceBuilderBase<TSubject, IBoundSourceBuilderState<TSubject>> {}

	public interface IBoundSourceBuilderState<TSubject> : ISourceBuilderState<TSubject>
	{
		ISource<TSubject> Source { get; }
	}

	public class BoundSourceBuilder<TSubject> : SourceBuilder<TSubject, IBoundSourceBuilderState<TSubject>>,
		IBoundSourceBuilder<TSubject>
	{
		public BoundSourceBuilder(TSubject subject)
			: this(new Source<TSubject>(subject)) {}

		public BoundSourceBuilder(ISource<TSubject> source) {}
	}
}
