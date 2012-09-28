#region License info...
// Propter for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.Emitting;
using Stile.Prototypes.Specifications.Evaluations;
using Stile.Prototypes.Specifications.Printable.Output;
using Stile.Prototypes.Specifications.Printable.Output.GrammarMetadata;
#endregion

namespace Stile.Prototypes.Specifications.Printable
{
    public interface IPrintableSpecification : IEmittingSpecification {}

    public interface IPrintableSpecification<in TSubject, out TResult> : IPrintableSpecification,
        IEmittingSpecification<TSubject, TResult, IPrintableEvaluation<TResult>, ILazyReadableText> {}

    public class PrintableSpecification<TSubject, TResult> :
        EmittingSpecification<TSubject, TResult, IPrintableEvaluation<TResult>, LazyReadableText>,
        IPrintableSpecification<TSubject, TResult>
    {
        private readonly IDescription<TResult> _description;
        private readonly string _reason;

        [Rule(Variable.Specification, Inline = true)]
        public PrintableSpecification([NotNull] Func<TSubject, TResult> extractor,
            [NotNull] Predicate<TResult> accepter,
            [Symbol] [NotNull] IDescription<TResult> description,
            [Symbol(Prefix = Terminal.Because)] string reason = null,
            Func<TResult, Exception, IPrintableEvaluation<TResult>> exceptionFilter = null)
            : base(extractor, accepter, exceptionFilter)
        {
            _reason = reason;
            _description = description.ValidateArgumentIsNotNull();
        }

        protected override LazyReadableText EmittingFactory(IWrappedResult<TResult> result)
        {
            return new LazyReadableText(() => Explain(result));
        }

        protected override IPrintableEvaluation<TResult> EvaluationFactory(IWrappedResult<TResult> result,
            LazyReadableText emitted)
        {
            return new PrintableEvaluation<TResult>(result, emitted);
        }

        private string Explain(IWrappedResult<TResult> result)
        {
            string expected = _description.ExplainExpected(result);
            string actual = _description.ExplainActual(result);
            string because = _reason == null ? null : "because " + _reason;
            return string.Join(" ", expected, actual, because);
        }
    }
}
