#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SubjectBuilders
{
    public interface IEnumerableSubjectBuilder<TSubject, TItem> : ISubjectBuilder<TSubject>
        where TSubject : class, IEnumerable<TItem> {}

    public class EnumerableSubjectBuilder<TSubject, TItem> : SubjectBuilder<TSubject>,
        IEnumerableSubjectBuilder<TSubject, TItem>
        where TSubject : class, IEnumerable<TItem> {}
}
