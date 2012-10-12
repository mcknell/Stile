#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using Stile.Patterns.Structural.FluentInterface;
using Stile.Prototypes.Specifications.DSL.SemanticModel;
#endregion

namespace Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultIs
{
	public interface INegatableIs : IIs {}

	public interface INegatableIs<out TResult, out TNegated, out TSpecifies> : INegatableIs,
		IIs<TResult, TSpecifies>,
		INegatable<TNegated>
		where TNegated : class, IIs<TResult, TSpecifies>
		where TSpecifies : class, ISpecification {}

	public interface INegatableIs<out TSubject, out TResult, out TNegated, out TSpecifies> :
		INegatableIs<TResult, TNegated, TSpecifies>,
		IIs<TSubject, TResult, TSpecifies>
		where TNegated : class, IIs<TSubject, TResult, TSpecifies>
		where TSpecifies : class, ISpecification<TSubject, TResult> {}
}
