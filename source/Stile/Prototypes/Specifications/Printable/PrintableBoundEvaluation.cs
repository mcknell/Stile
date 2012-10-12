#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.Bound;
using Stile.Prototypes.Specifications.DSL.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.Emitting;
using Stile.Prototypes.Specifications.Printable.Output;
#endregion

namespace Stile.Prototypes.Specifications.Printable
{
    public interface IPrintableBoundEvaluation : IBoundEvaluation,
        IPrintableEvaluation {}

    public interface IPrintableBoundEvaluation<out TResult, out TEmit> : IPrintableBoundEvaluation,
        IBoundEvaluation<TResult>,
        IPrintableEvaluation<TResult, TEmit> {}

    public interface IPrintableBoundEvaluation<out TResult> : IPrintableBoundEvaluation<TResult, ILazyReadableText> {}

    public class PrintableBoundEvaluation<TResult> : EmittingEvaluation<TResult, ILazyReadableText>,
        IPrintableBoundEvaluation<TResult>
    {
        public PrintableBoundEvaluation(IPrintableEvaluation<TResult> evaluation)
            : this(evaluation.Result, evaluation.Emitted) {}

        public PrintableBoundEvaluation([NotNull] IWrappedResult<TResult> wrappedResult, ILazyReadableText emitted)
            : base(wrappedResult, emitted) {}
    }
}
