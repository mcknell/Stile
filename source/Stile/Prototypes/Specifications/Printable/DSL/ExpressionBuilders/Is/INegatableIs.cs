#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using Stile.Patterns.Structural.FluentInterface;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.Is
{
    public interface INegatableIs<out TSubject, out TNegated> : INegatable<TNegated>,
        IIs<TSubject>
        where TNegated : class, IIs<TSubject> {}

    public interface INegatableIs<out TSubject, out TResult, out TNegated> : INegatableIs<TResult, TNegated>,
        IIs<TSubject, TResult>
        where TNegated : class, IIs<TSubject, TResult> {}
}
