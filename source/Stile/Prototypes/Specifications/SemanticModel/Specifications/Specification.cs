#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Prototypes.Specifications.Builders.OfPredicates;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Specifications
{
	public interface ISpecification {}

// ReSharper disable UnusedTypeParameter
	/// <summary>
	/// Generalization so that Is and Has can have generic type constraints that don't specify a Subject.
	/// </summary>
	/// <typeparam name="TResult"></typeparam>
	public interface IResultSpecification<out TResult> : ISpecification {}

// ReSharper restore UnusedTypeParameter

	public interface ISpecification<in TSubject> : ISpecification {}

	public interface ISpecification<in TSubject, out TResult> : ISpecification<TSubject>,
		IResultSpecification<TResult>
	{
		[NotNull]
		IEvaluation<TSubject, TResult> Evaluate(TSubject subject);
	}

	public interface ISpecification<TSubject, TResult, out TExpectationBuilder> :
		ISpecification<TSubject, TResult>,
		IChainableSpecification<TExpectationBuilder>,
		IHides<ISpecificationState<TSubject, TResult>>
		where TExpectationBuilder : class, IExpectationBuilder {}

	public interface ISpecificationState {}

	public interface ISpecificationState<out TSubject> : ISpecificationState
	{
		[CanBeNull]
		string Because { get; }
		[CanBeNull]
		ISource<TSubject> Source { get; }
	}

	public interface ISpecificationState<TSubject, TResult> : ISpecificationState<TSubject>
	{
		[NotNull]
		ICriterion<TResult> Criterion { get; }
		[NotNull]
		IInstrument<TSubject, TResult> Instrument { get; }
	}

	public abstract class Specification
	{
		public delegate TSpecification Factory<out TSpecification, TSubject, TResult, in TExpectationBuilder>(
			ISource<TSubject> source,
			IInstrument<TSubject, TResult> instrument,
			ICriterion<TResult> criterion,
			TExpectationBuilder expectationBuilder,
			IExceptionFilter<TSubject, TResult> exceptionFilter = null)
			where TSpecification : class, IChainableSpecification
			where TExpectationBuilder : class, IExpectationBuilder;

		protected static readonly IError[] NoErrors = new IError[0];
	}

	public abstract class Specification<TSubject> : Specification,
		ISpecification<TSubject>
	{
		protected Specification([CanBeNull] ISource<TSubject> source, [CanBeNull] string because)
		{
			Source = source;
			Because = because;
		}

		public string Because { get; private set; }
		public ISource<TSubject> Source { get; private set; }
	}

	public class Specification<TSubject, TResult, TExpectationBuilder> : Specification<TSubject>,
		IBoundSpecification<TSubject, TResult, TExpectationBuilder>,
		ISpecificationState<TSubject, TResult>
		where TExpectationBuilder : class, IExpectationBuilder
	{
		private readonly IExceptionFilter<TSubject, TResult> _exceptionFilter;
		private readonly TExpectationBuilder _expectationBuilder;

	    public Specification([NotNull] IInstrument<TSubject, TResult> instrument,
			[NotNull] ICriterion<TResult> criterion,
			[NotNull] TExpectationBuilder expectationBuilder,
			ISource<TSubject> source = null,
			string because = null,
			IExceptionFilter<TSubject, TResult> exceptionFilter = null)
			: base(source, because)
		{
			Instrument = instrument.ValidateArgumentIsNotNull();
			Criterion = criterion.ValidateArgumentIsNotNull();
			_expectationBuilder = expectationBuilder.ValidateArgumentIsNotNull();
			_exceptionFilter = exceptionFilter;
		}

		public TExpectationBuilder AndThen
		{
			get { return _expectationBuilder; }
		}

		public ICriterion<TResult> Criterion { get; private set; }
		public IInstrument<TSubject, TResult> Instrument { get; private set; }
		public ISpecificationState<TSubject, TResult> Xray
		{
			get { return this; }
		}

		public IEvaluation<TSubject, TResult> Evaluate(TSubject subject)
		{
			return Evaluate(() => subject, UnboundFactory);
		}

		public IBoundEvaluation<TSubject, TResult> Evaluate()
		{
			return Evaluate(Source.Get, BoundFactory);
		}

		private bool ExpectsException
		{
			get { return _exceptionFilter != null; }
		}

		private IBoundEvaluation<TSubject, TResult> BoundFactory(Outcome outcome,
			TResult result,
			params IError[] error)
		{
			return new BoundEvaluation<TSubject, TResult>(this, outcome, result, error);
		}

		private TEvaluation Evaluate<TEvaluation>(Func<TSubject> subjectGetter,
			SuccessFactory<TEvaluation> successFactory) where TEvaluation : class, IEvaluation<TSubject, TResult>
		{
			TResult result = default(TResult);
			IError[] errors = NoErrors;
			try
			{
				// only trap exceptions while getting the subject or instrumenting it, not while accepting it
				TSubject subject = subjectGetter.Invoke();
				result = Instrument.Sample(subject);
			} catch (Exception e)
			{
				TEvaluation evaluation;
				if (ExpectsException
				    && _exceptionFilter.TryFilter(result, e, (o, r, ex) => successFactory(o, r, ex), out evaluation))
				{
					return evaluation;
				}
				// allow unexpected exceptions to bubble out
				throw;
			}

			if (ExpectsException)
			{
				// exception was expected but none was thrown
				return _exceptionFilter.Fail<TEvaluation>(result);
			}

			Outcome outcome = Criterion.Accept(result);
			return successFactory.Invoke(outcome, result, errors);
		}

		private IEvaluation<TSubject, TResult> UnboundFactory(Outcome outcome, TResult result, params IError[] error)
		{
			return new Evaluation<TSubject, TResult>(this, outcome, result, error);
		}

		private delegate TEvaluation SuccessFactory<TEvaluation>(
			Outcome outcome, TResult result, params IError[] error);
	}
}
