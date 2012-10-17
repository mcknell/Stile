#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.DSL.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.Printable.Output;
#endregion

namespace Stile.Prototypes.Specifications.Printable
{
    public interface IPrintableEvaluation : IEmittingEvaluation {}

    public interface IPrintableEvaluation<out TResult, out TEmit> : IPrintableEvaluation,
        IEmittingEvaluation<TResult, TEmit> {}

    public interface IPrintableEvaluation<out TResult> : IPrintableEvaluation<TResult, ILazyReadableText> {}

    public class PrintableEvaluation<TSubject> : EmittingEvaluation<TSubject, ILazyReadableText>,
        IPrintableEvaluation<TSubject>
    {
        public PrintableEvaluation([NotNull] IWrappedResult<TSubject> wrappedResult, ILazyReadableText emitted)
            : base(wrappedResult, emitted) {}
    }
}
