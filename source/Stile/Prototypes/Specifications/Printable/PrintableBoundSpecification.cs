#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.Bound;
using Stile.Prototypes.Specifications.DSL.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.Sources;
using Stile.Prototypes.Specifications.Printable.Output;
#endregion

namespace Stile.Prototypes.Specifications.Printable
{
    public interface IPrintableBoundSpecification : IBoundSpecification,
        IPrintableSpecification {}

    public interface IPrintableBoundSpecification<in TSubject, out TResult, out TEvaluation> :
        IPrintableBoundSpecification,
        IBoundSpecification<TSubject, TResult, TEvaluation>,
        IPrintableSpecification<TSubject, TResult, TEvaluation, ILazyReadableText>
        where TEvaluation : class, IPrintableBoundEvaluation<TResult, ILazyReadableText> {}

    public interface IPrintableBoundSpecification<in TSubject, out TResult> :
        IPrintableBoundSpecification<TSubject, TResult, IPrintableBoundEvaluation<TResult>> {}

    public class PrintableBoundSpecification<TSubject, TResult> :
        PrintableSpecification<TSubject, TResult, IPrintableBoundEvaluation<TResult>, ILazyReadableText>,
        IPrintableBoundSpecification<TSubject, TResult>
    {
        private readonly IPrintableSource<TSubject> _source;

        public PrintableBoundSpecification([NotNull] IPrintableSource<TSubject> source,
            [NotNull] IPrintableSpecificationState<TSubject, TResult> specification)
            : base(
                specification.LazyInstrument,
                specification.Accepter,
                specification.Explainer,
                specification.SubjectDescription,
                specification.Reason,
                (result, exception) => MakeExceptionFilter(specification, result, exception))
        {
            _source = source.ValidateArgumentIsNotNull();
        }

        public IPrintableBoundEvaluation<TResult> Evaluate()
        {
            TSubject subject = _source.Get();
            return Evaluate(subject);
        }

        protected override ILazyReadableText EmittingFactory(IWrappedResult<TResult> result)
        {
            throw new NotImplementedException();
        }

        protected override IPrintableBoundEvaluation<TResult> EvaluationFactory(IWrappedResult<TResult> result,
            ILazyReadableText emitted)
        {
            throw new NotImplementedException();
        }

        protected override ILazyReadableText ExplainEvaluation(Lazy<ILazyReadableText> emitted, TSubject subject)
        {
            return new LazyReadableText(() => emitted.ExplainEvaluation(subject, SubjectDescription));
        }

        private static PrintableBoundEvaluation<TResult> MakeExceptionFilter(
            IPrintableSpecificationState<TSubject, TResult> specification, TResult result, Exception exception)
        {
            IPrintableEvaluation<TResult> evaluation = specification.ExceptionFilter.Invoke(result, exception);
            if (evaluation == null)
            {
                return null;
            }
            return new PrintableBoundEvaluation<TResult>(evaluation);
        }
    }
}
