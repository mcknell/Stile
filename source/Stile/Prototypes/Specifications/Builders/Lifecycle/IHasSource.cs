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
	public interface ILifecycleStage {}

	public interface IHasSource<TSubject> : ILifecycleStage
	{
		[CanBeNull]
		ISource<TSubject> Source { get; }
	}

	public interface IHasProcedure<TSubject> : ILifecycleStage
	{
		[NotNull]
		IProcedure<TSubject> Procedure { get; }
	}

	public interface IHasInstrument<TSubject, out TResult> : ILifecycleStage
	{
		[NotNull]
		IInstrument<TSubject, TResult> Instrument { get; }
	}

	public interface IHasExpectation<TSubject, TResult> : ILifecycleStage
	{
		[NotNull]
		IExpectation<TSubject, TResult> Expectation { get; }
	}

	public interface IHasFaultSpecification<TSubject> : ILifecycleStage
	{
		[NotNull]
		IFaultSpecification<TSubject> Specification { get; }
	}

	public interface IHasSpecification<TSubject, TResult> : ILifecycleStage
	{
		[NotNull]
		ISpecification<TSubject, TResult> Specification { get; }
	}
}
