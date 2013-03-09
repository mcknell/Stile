#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Expectations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.Lifecycle
{
	public interface IHasSource<out TSubject>
	{
		[CanBeNull]
		ISource<TSubject> Source { get; }
	}

	public interface IHasProcedure<TSubject>
	{
		[NotNull]
		IProcedure<TSubject> Procedure { get; }
	}

	public interface IHasInstrument<TSubject, out TResult>
	{
		[NotNull]
		IInstrument<TSubject, TResult> Instrument { get; }
	}

	public interface IHasExpectation<TSubject, TResult>
	{
		[NotNull]
		IExpectation<TSubject, TResult> Expectation { get; }
	}

	public interface IHasSpecification<TSubject, TResult>
	{
		[NotNull]
		ISpecification<TSubject, TResult> Specification { get; }
	}
}
