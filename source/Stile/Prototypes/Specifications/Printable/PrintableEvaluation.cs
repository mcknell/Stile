#region License info...
// Propter for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.Emitting;
using Stile.Prototypes.Specifications.Evaluations;
using Stile.Prototypes.Specifications.Printable.Output;
#endregion

namespace Stile.Prototypes.Specifications.Printable
{
    public interface IPrintableEvaluation : IEmittingEvaluation {}

    public interface IPrintableEvaluation<out TResult> : IPrintableEvaluation,
        IEmittingEvaluation<TResult, LazyReadableText> {}

    public class PrintableEvaluation<TResult> : EmittingEvaluation<TResult, LazyReadableText>,
        IPrintableEvaluation<TResult>
    {
        public PrintableEvaluation([NotNull] IWrappedResult<TResult> wrappedResult, LazyReadableText emitted)
            : base(wrappedResult, emitted) {}
    }
}
