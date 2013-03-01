#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Types.Expressions;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel
{
	public interface IThrowingInstrument : IInstrument {}

	public interface IThrowingInstrument<in TSubject> : IThrowingInstrument
	{
		void Sample([NotNull] TSubject subject);
	}

	public class ThrowingInstrument 
	{
		public static class Trivial<TSubject>
		{
			public static readonly Lazy<Action<TSubject>> Map = new Lazy<Action<TSubject>>(() => x => {});
		}
	}

	public class ThrowingInstrument<TSubject> : ThrowingInstrument,
		IThrowingInstrument<TSubject>
	{
		private readonly Lazy<Action<TSubject>> _lazyAction;

		public ThrowingInstrument([NotNull] Expression<Action<TSubject>> expression)
		{
			Expression<Action<TSubject>> validExpression = expression.ValidateArgumentIsNotNull();
			_lazyAction = new Lazy<Action<TSubject>>(validExpression.Compile);
			Description = validExpression.ToLazyDebugString();
		}

		public Lazy<string> Description { get; private set; }

		public void Sample(TSubject subject)
		{
			_lazyAction.Value.Invoke(subject);
		}
	}
}
