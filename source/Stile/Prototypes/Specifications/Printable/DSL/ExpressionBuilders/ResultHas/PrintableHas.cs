#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultHas;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SpecificationBuilders;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.ResultHas
{
    public interface IPrintableHas : IHas {}

    public interface IPrintableHas<out TResult> : IPrintableHas, IHas<TResult> {}

    public interface IPrintableHas<out TResult, TSubject> : IPrintableHas<TResult>,
        IHas<TResult, IPrintableSpecification<TSubject, TResult>, IPrintableEvaluation<TResult>, TSubject> {}

    public class PrintableHas<TResult, TSubject> :
        Has<TResult, TSubject, IPrintableSpecification<TSubject, TResult>, IPrintableEvaluation<TResult>>,
        IPrintableHas<TResult, TSubject>,
        IPrintableHasState<TSubject, TResult>
    {
        public PrintableHas([NotNull] Lazy<Func<TSubject, TResult>> instrument,
            [NotNull] IPrintableSpecificationBuilderState state)
            : base(instrument)
        {
            SubjectDescription = state.SubjectDescription;
        }

        public Lazy<string> SubjectDescription { get; private set; }
    }

    public class PrintableHas<TSubject> : PrintableHas<TSubject, TSubject>
    {
        public PrintableHas([NotNull] IPrintableSpecificationBuilderState state)
            : base(Specifications.DSL.ExpressionBuilders.Instrument.Trivial<TSubject>.Map, state) {}
    }
}
