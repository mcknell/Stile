#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
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

    public interface IPrintableEvaluation<out TSubject> : IPrintableEvaluation,
        IEmittingEvaluation<TSubject, LazyReadableText> {}

    public interface IPrintableEvaluation<out TSubject, out TResult> : IPrintableEvaluation,
        IEmittingEvaluation<TSubject, TResult, LazyReadableText> {}

    public class PrintableEvaluation<TSubject, TResult> : EmittingEvaluation<TSubject, TResult, LazyReadableText>,
        IPrintableEvaluation<TSubject, TResult>
    {
        public PrintableEvaluation([NotNull] IWrappedResult<TSubject, TResult> wrappedResult, LazyReadableText emitted)
            : base(wrappedResult, emitted) {}
    }

    public class PrintableEvaluation<TSubject> : EmittingEvaluation<TSubject, LazyReadableText>,
        IPrintableEvaluation<TSubject>
    {
        public PrintableEvaluation([NotNull] IWrappedResult<TSubject> wrappedResult, LazyReadableText emitted)
            : base(wrappedResult, emitted) {}
    }
}