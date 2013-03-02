#region License info...
// Propter for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Propter project on GitHub
#endregion

#region using...
using System;
using System.Linq.Expressions;
using System.Threading;
using JetBrains.Annotations;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Readability;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel
{
	public interface IProcedure {}

	public interface IProcedureState
	{
		Lazy<string> Description { get; }
	}

	public interface IProcedureState<out TSubject> : IProcedure
	{
		[CanBeNull]
		ISource<TSubject> Source { get; }
	}

	public interface IProcedure<TSubject> : IProcedure,
		IHides<IProcedureState<TSubject>>
	{
		void Sample(TSubject subject, CancellationToken? cancellationToken = null);
	}

	public abstract class Procedure : IProcedure
	{
		protected Procedure(Lazy<string> description)
		{
			Description = description;
		}

		public Lazy<string> Description { get; private set; }

		public static class Trivial<TSubject>
		{
			public static readonly Lazy<Func<TSubject, TSubject>> Map =
				new Lazy<Func<TSubject, TSubject>>(Identity.Map<TSubject>);
		}
	}

	public class Procedure<TSubject> : Procedure,
		IProcedure<TSubject>,
		IProcedureState<TSubject>
	{
		private readonly Lazy<Action<TSubject>> _lazyAction;

		public Procedure([NotNull] Expression<Action<TSubject>> expression, ISource<TSubject> source = null)
			: base(expression.ToLazyDebugString())
		{
			_lazyAction = new Lazy<Action<TSubject>>(expression.Compile);
			Source = source;
		}

		public ISource<TSubject> Source { get; private set; }

		public IProcedureState<TSubject> Xray
		{
			get { return this; }
		}

		public void Sample(TSubject subject, CancellationToken? cancellationToken = null)
		{
			_lazyAction.Value.Invoke(subject);
		}
	}
}
