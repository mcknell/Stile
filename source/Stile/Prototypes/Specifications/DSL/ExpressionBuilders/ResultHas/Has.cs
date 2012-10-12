#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.DSL.SemanticModel;
#endregion

namespace Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultHas
{
	public interface IHas {}

	public interface IHas<out TResult, out TSpecifies> : IHas
		where TSpecifies : class, ISpecification {}

	public interface IHas<out TSubject, out TResult, out TSpecifies> : IHas<TResult, TSpecifies>
		where TSpecifies : class, ISpecification<TSubject, TResult> {}

	public class Has<TSubject, TResult, TSpecifies> : IHas<TSubject, TResult, TSpecifies>,
		IHasState<TSubject, TResult>
		where TSpecifies : class, ISpecification<TSubject, TResult>
	{
		public Has([NotNull] Lazy<Func<TSubject, TResult>> instrument)
		{
			Instrument = instrument.ValidateArgumentIsNotNull();
		}

		public Lazy<Func<TSubject, TResult>> Instrument { get; private set; }
	}
}
