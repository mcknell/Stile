#region License info...
// Propter for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Propter project on GitHub
#endregion

#region using...
using System;
using System.Linq.Expressions;
using System.Threading;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Types.Expressions;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel
{
	public interface IInstrument : IProcedure {}

	public interface IInstrument<TSubject, out TResult> : IInstrument,
		IProcedure<TSubject>
	{
		new TResult Sample(TSubject subject, CancellationToken? cancellationToken = null);
	}

	public class Instrument<TSubject, TResult> : IInstrument<TSubject, TResult>,
		IProcedureState<TSubject>
	{
		private readonly Lazy<Func<TSubject, TResult>> _lazyFunc;

		public Instrument([NotNull] Expression<Func<TSubject, TResult>> expression, ISource<TSubject> source = null)
		{
			Expression<Func<TSubject, TResult>> validatedExpression = expression.ValidateArgumentIsNotNull();
			_lazyFunc = new Lazy<Func<TSubject, TResult>>(validatedExpression.Compile);
			Description = validatedExpression.ToLazyDebugString();
			Source = source;
		}

		public Lazy<string> Description { get; private set; }
		public ISource<TSubject> Source { get; private set; }
		public IProcedureState<TSubject> Xray
		{
			get { return this; }
		}

		public TResult Sample(TSubject subject, CancellationToken? cancellationToken = null)
		{
			return _lazyFunc.Value.Invoke(subject);
		}

		void IProcedure<TSubject>.Sample(TSubject subject, CancellationToken? cancellationToken)
		{
			((IInstrument<TSubject, TResult>) this).Sample(subject);
		}
	}
}
