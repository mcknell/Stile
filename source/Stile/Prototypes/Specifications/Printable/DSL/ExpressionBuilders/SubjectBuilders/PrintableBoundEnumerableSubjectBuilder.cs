#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.SubjectBuilders;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.Sources;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SubjectBuilders
{
    public interface IPrintableBoundEnumerableSubjectBuilder : IBoundEnumerableSubjectBuilder {}

    public interface IPrintableBoundEnumerableSubjectBuilder<TSubject, TItem> :
        IPrintableBoundEnumerableSubjectBuilder,
        IBoundEnumerableSubjectBuilder<TSubject, TItem>
        where TSubject : class, IEnumerable<TItem> {}

    public class PrintableBoundEnumerableSubjectBuilder<TSubject, TItem> :
        BoundEnumerableSubjectBuilder<TSubject, TItem, IPrintableEnumerableSource<TSubject, TItem>>,
        IPrintableBoundEnumerableSubjectBuilder<TSubject, TItem>,
        IPrintableBoundEnumerableSubjectBuilderState<TSubject, TItem>
        where TSubject : class, IEnumerable<TItem>
    {
        public PrintableBoundEnumerableSubjectBuilder([NotNull] IPrintableSource<TSubject> source)
            : base(new PrintableEnumerableSource<TSubject, TItem>(source)) {}
    }

    public interface IPrintableBoundEnumerableSubjectBuilderState : IBoundEnumerableSubjectBuilderState {}

    public interface IPrintableBoundEnumerableSubjectBuilderState<out TSubject, out TItem> :
        IPrintableBoundEnumerableSubjectBuilderState,
        IBoundEnumerableSubjectBuilderState<TSubject, TItem, IPrintableEnumerableSource<TSubject, TItem>>
        where TSubject : class, IEnumerable<TItem> {}
}
