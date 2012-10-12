#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
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

	public interface IPrintableSpecification<in TSubject, out TResult> : IPrintableSpecification,
		IEmittingSpecification<TSubject, TResult> {}

	public interface IPrintableSpecification<in TSubject, out TResult, out TEvaluation, out TEmit> :
		IPrintableSpecification<TSubject, TResult>,
		IEmittingSpecification<TSubject, TResult, TEvaluation, TEmit>
		where TEvaluation : class, IPrintableEvaluation<TResult, TEmit> {}

	public interface IFluentSpecification<in TSubject, out TResult> :
		IPrintableSpecification<TSubject, TResult, IPrintableEvaluation<TResult>, ILazyReadableText> {}

	public interface IPrintableSpecificationState<TSubject, TResult, out TEvaluation, TEmit> :
		IEmittingSpecificationState<TSubject, TResult, TEvaluation, TEmit>
		where TEvaluation : class, IPrintableEvaluation<TResult, TEmit>
	{
		IExplainer<TSubject, TResult> Explainer { get; }
		string Reason { get; }
		Lazy<string> SubjectDescription { get; }
	}

	public interface IPrintableSpecificationState<TSubject, TResult> :
		IPrintableSpecificationState<TSubject, TResult, IPrintableEvaluation<TResult>, ILazyReadableText> {}

	public static class PrintableSpecification
	{
		public static string ExplainEvaluation<TSubject>(this Lazy<ILazyReadableText> emitted,
			TSubject subject,
			Lazy<string> lazySubjectDescription)
		{
			string type = typeof(TSubject).ToDebugString();
			string evaluated = emitted.Value.Retrieved.Value;
			string subjectDescription = subject.ToDebugString();
			if (lazySubjectDescription != null && String.IsNullOrWhiteSpace(lazySubjectDescription.Value) == false)
			{
				subjectDescription += String.Format(" transformed by {0}", lazySubjectDescription.Value);
			}
			else
			{
				subjectDescription += String.Format(" (of type {0})", type);
			}
			return String.Format("expected {0} would {1}", subjectDescription, evaluated);
		}
	}

	public abstract class PrintableSpecification<TSubject, TResult, TEvaluation, TEmit> :
		EmittingSpecification<TSubject, TResult, TEvaluation, TEmit>,
		IPrintableSpecification<TSubject, TResult, TEvaluation, TEmit>,
		IPrintableSpecificationState<TSubject, TResult, TEvaluation, TEmit>
		where TEvaluation : class, IPrintableEvaluation<TResult, TEmit>
	{
		private readonly IExplainer<TSubject, TResult> _explainer;
		private readonly string _reason;
		private readonly Lazy<string> _subjectDescription;

		protected PrintableSpecification([NotNull] Lazy<Func<TSubject, TResult>> instrument,
			[NotNull] Predicate<TResult> accepter,
			[NotNull] IExplainer<TSubject, TResult> explainer,
			Lazy<string> subjectDescription = null,
			string reason = null,
			Func<TResult, Exception, TEvaluation> exceptionFilter = null)
			: base(instrument, accepter, exceptionFilter)
		{
			_subjectDescription = subjectDescription;
			_reason = reason;
			_explainer = explainer.ValidateArgumentIsNotNull();
		}

		public IExplainer<TSubject, TResult> Explainer
		{
			get { return _explainer; }
		}
		public string Reason
		{
			get { return _reason; }
		}
		public Lazy<string> SubjectDescription
		{
			get { return _subjectDescription; }
		}

		[Rule(Variable.StartSymbol, Items = new object[]
		{
			"(", Terminal.Because, Variable.Reason, Terminal.EOL, ")?", //
			Terminal.SubjectPrefix, "{0}", Terminal.DescriptionPrefix, Variable.Explainer
		})]
		public override TEvaluation Evaluate([Symbol(Variable.Subject)] TSubject subject)
		{
			TEvaluation evaluation = base.Evaluate(subject);
			TEmit explained = ExplainEvaluation(new Lazy<TEmit>(() => evaluation.Emitted), subject);
			TEvaluation output = EvaluationFactory(evaluation.Result, explained);
			return output;
		}

		public static string Explain(IWrappedResult<TResult> result, IExplainer<TSubject, TResult> explainer, string reason)
		{
			string expected = explainer.ExplainExpected(result);
			string conjunction =
				PrintableSpecification<TSubject, TResult, IPrintableEvaluation<TResult>, ILazyReadableText>.PrintConjunction(
					result.Outcome);
			string actual = explainer.ExplainActualSurprise(result);
			string because = reason == null ? null : string.Format("because {0}{1}", reason, Environment.NewLine);
			string basic = string.Join(" ", expected, Environment.NewLine, conjunction, actual);
			return because + basic;
		}

		public static string PrintConjunction(Outcome outcome)
		{
			return outcome == Outcome.Succeeded ? "and" : "but";
		}

		protected string Explain(IWrappedResult<TResult> result)
		{
			return Explain(result, Explainer, Reason);
		}

		protected abstract TEmit ExplainEvaluation(Lazy<TEmit> emitted, TSubject subject);
	}

	public class PrintableSpecification<TSubject, TResult> :
		PrintableSpecification<TSubject, TResult, IPrintableEvaluation<TResult>, ILazyReadableText>,
		IFluentSpecification<TSubject, TResult>,
		IPrintableSpecificationState<TSubject, TResult>
	{
		public PrintableSpecification([NotNull] Lazy<Func<TSubject, TResult>> instrument,
			[NotNull] Predicate<TResult> accepter,
			[NotNull] IExplainer<TSubject, TResult> explainer,
			Lazy<string> subjectDescription = null,
			string reason = null,
			Func<TResult, Exception, IPrintableEvaluation<TResult>> exceptionFilter = null)
			: base(instrument, accepter, explainer, subjectDescription, reason, exceptionFilter) {}

		protected override ILazyReadableText EmittingFactory(IWrappedResult<TResult> result)
		{
			return new LazyReadableText(() => Explain(result));
		}

		protected override IPrintableEvaluation<TResult> EvaluationFactory(IWrappedResult<TResult> result,
			ILazyReadableText emitted)
		{
			return new PrintableEvaluation<TResult>(result, emitted);
		}

		protected override ILazyReadableText ExplainEvaluation(Lazy<ILazyReadableText> emitted, TSubject subject)
		{
			return new LazyReadableText(() => emitted.ExplainEvaluation(subject, SubjectDescription));
		}
	}
}
