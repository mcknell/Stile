#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Types.Expressions;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel
{
	public interface IInstrument : IProcedure {}

	public interface IInstrument<in TSubject, out TResult> : IInstrument,
		IProcedure<TSubject>
	{
		new TResult Sample(TSubject subject, CancellationToken? cancellationToken = null);
	}

	public class Instrument<TSubject, TResult> : IInstrument<TSubject, TResult>,
		IProcedureState
	{
		private readonly Lazy<Func<TSubject, TResult>> _lazyFunc;

		public Instrument([NotNull] Expression<Func<TSubject, TResult>> expression)
		{
			Expression<Func<TSubject, TResult>> validatedExpression = expression.ValidateArgumentIsNotNull();
			_lazyFunc = new Lazy<Func<TSubject, TResult>>(validatedExpression.Compile);
			Description = validatedExpression.ToLazyDebugString();
		}

		public Lazy<string> Description { get; private set; }
		public IProcedureState Xray
		{
			get { return this; }
		}

		public TResult Sample(TSubject subject, CancellationToken? cancellationToken = null)
		{
			return _lazyFunc.Value.Invoke(subject);
		}

		void IProcedure<TSubject>.Sample(TSubject subject, CancellationToken? cancellationToken)
		{
			((IInstrument<TSubject, TResult>)this).Sample(subject);
		}
	}
}
