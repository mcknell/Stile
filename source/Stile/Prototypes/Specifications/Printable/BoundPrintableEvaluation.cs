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
    public class BoundPrintableEvaluation<TResult> : EmittingEvaluation<TResult, LazyReadableText>,
        IBoundEvaluation<TResult>
    {
        public BoundPrintableEvaluation([NotNull] IWrappedResult<TResult> wrappedResult, LazyReadableText emitted)
            : base(wrappedResult, emitted) {}
    }
}
