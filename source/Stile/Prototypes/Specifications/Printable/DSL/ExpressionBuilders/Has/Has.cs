#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.Has
{
    public interface IHas<out TSubject> {}

    public interface IHas<out TSubject, out TResult> : IHas<TResult> {}

    public class Has<TSubject> : IHas<TSubject> {}

    public class Has<TSubject, TResult> : Has<TResult>,
        IHas<TSubject, TResult> {}
}
