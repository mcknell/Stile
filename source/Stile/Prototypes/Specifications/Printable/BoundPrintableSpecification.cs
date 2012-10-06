#region License info...
// Propter for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.Bound;
#endregion

namespace Stile.Prototypes.Specifications.Printable
{
    public class BoundPrintableSpecification<TSubject, TResult> : IBoundSpecification<TSubject, TResult>
    {
        private readonly ISource<TSubject> _source;
        private readonly IPrintableSpecification<TSubject, TResult> _specification;

        public BoundPrintableSpecification([NotNull] ISource<TSubject> source,
            [NotNull] IPrintableSpecification<TSubject, TResult> specification)
        {
            _source = source.ValidateArgumentIsNotNull();
            _specification = specification.ValidateArgumentIsNotNull();
        }

        public IBoundEvaluation<TResult> Evaluate(TSubject subject)
        {
            IPrintableEvaluation<TResult> evaluation = _specification.Evaluate(subject);
            return new BoundEvaluation<TResult>(evaluation.Result);
        }

        public IBoundEvaluation<TResult> Evaluate()
        {
            TSubject subject = _source.Get();
            IPrintableEvaluation<TResult> evaluation = _specification.Evaluate(subject);
            return new BoundEvaluation<TResult>(evaluation.Result);
        }
    }
}
