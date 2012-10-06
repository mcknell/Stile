#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders;
using Stile.Prototypes.Specifications.DSL.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.Emitting;
using Stile.Prototypes.Specifications.Printable.Output;
using Stile.Prototypes.Specifications.Printable.Output.Explainers;
using Stile.Prototypes.Specifications.Printable.Output.GrammarMetadata;
using Stile.Readability;
using Stile.Types.Reflection;
#endregion

namespace Stile.Prototypes.Specifications.Printable
{
    public interface IPrintableSpecification : IEmittingSpecification {}

    public interface IPrintableResultSpecification<out TResult> : IPrintableSpecification,
        IEmittingSpecification<TResult, IPrintableEvaluation<TResult>> {}

    public interface IPrintableSpecification<in TSubject> : IPrintableSpecification,
        IEmittingSpecification<TSubject> {}

    public interface IPrintableSpecification<in TSubject, out TResult> : IPrintableSpecification<TSubject>,
        IPrintableResultSpecification<TResult>,
        IEmittingSpecification<TSubject, TResult, IPrintableEvaluation<TResult>, ILazyReadableText> {}

    public class PrintableSpecification<TSubject, TResult> :
        EmittingSpecification<TSubject, TResult, IPrintableEvaluation<TResult>, LazyReadableText>,
        IPrintableSpecification<TSubject, TResult>
    {
        private readonly IExplainer<TSubject, TResult> _explainer;
        private readonly string _reason;
        private readonly Lazy<string> _subjectDescription;

        public PrintableSpecification([NotNull] Lazy<Func<TSubject, TResult>> extractor,
            [NotNull] Predicate<TResult> accepter,
            [NotNull] IExplainer<TSubject, TResult> explainer,
            Lazy<string> subjectDescription = null,
            string reason = null,
            Func<TResult, Exception, IPrintableEvaluation<TResult>> exceptionFilter = null)
            : base(extractor, accepter, exceptionFilter)
        {
            _subjectDescription = subjectDescription;
            _reason = reason;
            _explainer = explainer.ValidateArgumentIsNotNull();
        }

        [Rule(Variable.StartSymbol,
            Items = new object[]
            {
                "(", Terminal.Because, Variable.Reason, Terminal.EOL, ")?", //
                Terminal.SubjectPrefix, "{0}", Terminal.DescriptionPrefix, Variable.Explainer
            })]
        public override IPrintableEvaluation<TResult> Evaluate([Symbol(Variable.Subject)] TSubject subject)
        {
            IPrintableEvaluation<TResult> evaluation = base.Evaluate(subject);
            var text = new LazyReadableText(() => ExplainEvaluation(evaluation.Emitted.Retrieved, subject));
            var printableEvaluation = new PrintableEvaluation<TResult>(evaluation.Result, text);
            return printableEvaluation;
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
            string expected = _explainer.ExplainExpected(result);
            string conjunction = PrintConjunction(result.Outcome);
            string actual = _explainer.ExplainActualSurprise(result);
            string because = _reason == null ? null : string.Format("because {0}{1}", _reason, Environment.NewLine);
            string basic = string.Join(" ", expected, Environment.NewLine, conjunction, actual);
            return because + basic;
        }

        protected string ExplainEvaluation(Lazy<string> evaluatedExplanation, TSubject subject)
        {
            string type = typeof(TSubject).ToDebugString();
            string evaluated = evaluatedExplanation.Value;
            string subjectDescription = subject.ToDebugString();
            if (_subjectDescription != null && string.IsNullOrWhiteSpace(_subjectDescription.Value) == false)
            {
                subjectDescription += string.Format(" transformed by {0}", _subjectDescription.Value);
            }
            else
            {
                subjectDescription += string.Format(" (of type {0})", type);
            }
            return string.Format("expected {0} would {1}", subjectDescription, evaluated);
        }

        internal static string PrintConjunction(Outcome outcome)
        {
            return outcome == Outcome.Succeeded ? "and" : "but";
        }
    }

    public class PrintableSpecification<TSubject> : PrintableSpecification<TSubject, TSubject>,
        IPrintableSpecification<TSubject>
    {
        public PrintableSpecification([NotNull] Predicate<TSubject> accepter,
            [NotNull] IExplainer<TSubject, TSubject> explainer,
            Lazy<string> subjectDescription = null,
            string reason = null,
            Func<TSubject, Exception, IPrintableEvaluation<TSubject>> exceptionFilter = null)
            : base(Instrument.Trivial<TSubject>.Map, accepter, explainer, subjectDescription, reason, exceptionFilter
                ) {}
    }
}
