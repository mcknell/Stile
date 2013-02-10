#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using Stile.Patterns.Structural.FluentInterface;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfSources
{
	public interface ISourceBuilderBase<TSubject, out TState> : IHides<TState>
		where TState : class, ISourceBuilderState<TSubject> {}

	public interface ISourceBuilder<TSubject> : ISourceBuilderBase<TSubject, ISourceBuilderState<TSubject>> {}

	public interface ISourceBuilderState<TSubject> {}

	public abstract class SourceBuilder<TSubject, TState> : ISourceBuilderBase<TSubject, TState>
		where TState : class, ISourceBuilderState<TSubject>
	{
		public TState Xray { get; private set; }
	}

	public class SourceBuilder<TSubject> : SourceBuilder<TSubject, ISourceBuilderState<TSubject>> {}
}
