#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Stile.Prototypes.Specifications.DSL.SemanticModel;
#endregion

namespace Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultHas
{
	public interface IQuantifiedEnumerableHas {}

	public interface IQuantifiedEnumerableHas<TItem, out TSpecifies> : IQuantifiedEnumerableHas
		where TSpecifies : class, ISpecification
	{
		[Pure]
		TSpecifies ItemsSatisfying(Expression<Func<TItem, bool>> expression);
	}

	public interface IQuantifiedEnumerableHasState {}

	public interface IQuantifiedEnumerableHasState<out TSpecifies> : IQuantifiedEnumerableHasState
		where TSpecifies : class, ISpecification
	{
		TSpecifies Make();
	}

	public abstract class QuantifiedEnumerableHas<TItem, TSpecifies> :
		IQuantifiedEnumerableHas<TItem, TSpecifies>,
		IQuantifiedEnumerableHasState<TSpecifies>
		where TSpecifies : class, ISpecification
	{
		protected QuantifiedEnumerableHas() {}

		public TSpecifies ItemsSatisfying(Expression<Func<TItem, bool>> expression)
		{
			return Make();
		}

		public TSpecifies Make()
		{
			throw new NotImplementedException();
		}
	}
}
