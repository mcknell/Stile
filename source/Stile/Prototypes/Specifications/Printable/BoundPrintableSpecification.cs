#region License info...
// Propter for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.Bound;
using Stile.Prototypes.Specifications.Emitting;
using Stile.Prototypes.Specifications.Evaluations;
using Stile.Prototypes.Specifications.Printable.Output;
using Stile.Prototypes.Specifications.Printable.Output.GrammarMetadata;
#endregion

namespace Stile.Prototypes.Specifications.Printable
{
    public class BoundPrintableEvaluation<TSubject, TResult> : EmittingEvaluation<TSubject, TResult, LazyReadableText>,
        IBoundEvaluation<TSubject, TResult>
    {
        public BoundPrintableEvaluation([NotNull] IWrappedResult<TSubject, TResult> wrappedResult, LazyReadableText emitted)
            : base(wrappedResult, emitted) {}
    }

    public class BoundPrintableSpecification<TSubject, TResult> : IBoundSpecification<TSubject, TResult>
    {
        private readonly ISource<TSubject> _source;
        private readonly IPrintableSpecification<TSubject, TResult> _specification;

        [Rule(Variable.StartSymbol)]
        public BoundPrintableSpecification(
            //
            [Symbol(Variable.SubjectClause, Prefix = Terminal.SubjectPrefix)] [NotNull] ISource<TSubject> source,
            [Symbol(Variable.Specification, Prefix = Terminal.DescriptionPrefix)] [NotNull] IPrintableSpecification<TSubject, TResult> specification)
        {
            _source = source.ValidateArgumentIsNotNull();
            _specification = specification.ValidateArgumentIsNotNull();
        }

        public IBoundEvaluation<TSubject, TResult> Evaluate(TSubject subject)
        {
            IPrintableEvaluation<TSubject, TResult> evaluation = _specification.Evaluate(subject);
            return new BoundEvaluation<TSubject, TResult>(evaluation.Result);
        }

        public IBoundEvaluation<TSubject, TResult> Evaluate()
        {
            TSubject subject = _source.Get();
            IPrintableEvaluation<TSubject, TResult> evaluation = _specification.Evaluate(subject);
            return new BoundEvaluation<TSubject, TResult>(evaluation.Result);
        }
    }
}
