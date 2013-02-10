#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Readability;
using Stile.Types.Expressions;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel
{
	public interface IInstrument<in TSubject, out TResult>
	{
		Lazy<string> Description { get; }
		TResult Sample([NotNull] TSubject subject);
	}

	public class Instrument
	{
		public static class Trivial<TSubject>
		{
			public static readonly Lazy<Func<TSubject, TSubject>> Map =
				new Lazy<Func<TSubject, TSubject>>(Identity.Map<TSubject>);
		}
	}

	public class Instrument<TSubject, TResult> : Instrument,
		IInstrument<TSubject, TResult>
	{
		private readonly Lazy<Func<TSubject, TResult>> _lazyFunc;

		public Instrument([NotNull] Expression<Func<TSubject, TResult>> expression)
		{
			Expression<Func<TSubject, TResult>> validExpression = expression.ValidateArgumentIsNotNull();
			_lazyFunc = new Lazy<Func<TSubject, TResult>>(validExpression.Compile);
			Description = validExpression.ToLazyDebugString();
		}

		public Lazy<string> Description { get; private set; }

		public TResult Sample(TSubject subject)
		{
			return _lazyFunc.Value.Invoke(subject);
		}
	}
}
