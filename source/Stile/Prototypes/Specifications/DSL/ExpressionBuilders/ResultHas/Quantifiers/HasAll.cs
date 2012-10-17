#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

using System.Collections.Generic;
using Stile.Prototypes.Specifications.DSL.SemanticModel;

namespace Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultHas.Quantifiers
{
	public class HasAll<TResult, TItem, TSpecifies> : QuantifiedEnumerableHas<TItem, TSpecifies>
		where TResult : class, IEnumerable<TItem>
		where TSpecifies : class, ISpecification {}
}
