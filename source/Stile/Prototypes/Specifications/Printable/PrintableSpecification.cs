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
using Stile.Prototypes.Specifications.Printable.Output.Explainers;
using Stile.Prototypes.Specifications.Printable.Output.GrammarMetadata;
#endregion

namespace Stile.Prototypes.Specifications.Printable
{
    public interface IPrintableSpecification : IEmittingSpecification {}

    public interface IPrintableSpecification<TSubject, out TResult> : IPrintableSpecification,
        IEmittingSpecification<TSubject, TResult, IPrintableEvaluation<TSubject, TResult>, ILazyReadableText> { }

    public class PrintableSpecification<TSubject, TResult> :
        EmittingSpecification<TSubject, TResult, IPrintableEvaluation<TSubject, TResult>, LazyReadableText>,
        IPrintableSpecification<TSubject, TResult>
    {
        private readonly IExplainer<TSubject, TResult> _explainer;
        private readonly string _reason;

        [Rule(Variable.Specification, Inline = true)]
        public PrintableSpecification([NotNull] Func<TSubject, TResult> extractor,
            [NotNull] Predicate<TResult> accepter,
            [Symbol(Variable.Explainer)] [NotNull] IExplainer<TSubject, TResult> explainer,
            [Symbol(Prefix = Terminal.Because)] string reason = null,
            Func<TResult, Exception, IPrintableEvaluation<TSubject, TResult>> exceptionFilter = null)
            : base(extractor, accepter, exceptionFilter)
        {
            _reason = reason;
            _explainer = explainer.ValidateArgumentIsNotNull();
        }

        protected override LazyReadableText EmittingFactory(IWrappedResult<TSubject, TResult> result)
        {
            return new LazyReadableText(() => Explain(result));
        }

        protected override IPrintableEvaluation<TSubject, TResult> EvaluationFactory(IWrappedResult<TSubject, TResult> result,
            LazyReadableText emitted)
        {
            return new PrintableEvaluation<TSubject, TResult>(result, emitted);
        }

        private string Explain(IWrappedResult<TSubject, TResult> result)
        {
            string expected = _explainer.ExplainExpected(result);
            string conjunction = PrintConjunction(result.Outcome);
            string actual = _explainer.ExplainActualSurprise(result);
            string because = _reason == null ? null : string.Format("{0}because {1}", Environment.NewLine, _reason);
            string basic = string.Join(" ", expected, Environment.NewLine, conjunction, actual);
            return basic + because;
        }

        internal static string PrintConjunction(Outcome outcome)
        {
            return outcome == Outcome.Succeeded ? "and" : "but";
        }
    }
}
