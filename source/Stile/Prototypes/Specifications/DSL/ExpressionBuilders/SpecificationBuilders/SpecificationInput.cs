#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
#endregion

namespace Stile.Prototypes.Specifications.DSL.ExpressionBuilders.SpecificationBuilders
{
	public interface ISpecificationInput {}

	public interface ISpecificationInput<TSubject, TResult> : ISpecificationInput
	{
		[NotNull]
		Predicate<TResult> Accepter { get; }
		[NotNull]
		Lazy<Func<TSubject, TResult>> LazyInstrument { get; }
	}

	public class SpecificationInput<TSubject, TResult> : ISpecificationInput<TSubject, TResult>
	{
		public SpecificationInput(Predicate<TResult> accepter, [NotNull] Lazy<Func<TSubject, TResult>> lazyInstrument)
		{
			Accepter = accepter.ValidateArgumentIsNotNull();
			LazyInstrument = lazyInstrument.ValidateArgumentIsNotNull();
		}

		public Predicate<TResult> Accepter { get; private set; }
		public Lazy<Func<TSubject, TResult>> LazyInstrument { get; private set; }
	}
}
