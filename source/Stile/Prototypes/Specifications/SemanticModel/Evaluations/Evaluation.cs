#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Evaluations
{
	public interface IEvaluation
	{
		IError[] Errors { get; }
		Outcome Outcome { get; }
	}

	public interface IEvaluation<out TResult> : IEvaluation
	{
		TResult Value { get; }
	}

	public interface IEvaluation<in TSubject, out TResult> : IEvaluation<TResult>
	{
		IEvaluation<TResult> Evaluate(TSubject subject);
	}

	public interface IBoundEvaluation<in TSubject, out TResult> : IEvaluation<TSubject, TResult>
	{
		IBoundEvaluation<TSubject, TResult> Evaluate();
	}

	public class BoundEvaluation<TSubject, TResult> : Evaluation<TSubject, TResult>,
		IBoundEvaluation<TSubject, TResult>
	{
		private readonly IBoundSpecification<TSubject, TResult> _specification;

		public BoundEvaluation([NotNull] IBoundSpecification<TSubject, TResult> specification,
			Outcome outcome,
			TResult value,
			params IError[] errors)
			: base(specification, outcome, value, errors)
		{
			_specification = specification;
		}

		public IBoundEvaluation<TSubject, TResult> Evaluate()
		{
			return _specification.Evaluate();
		}
	}

	public class Evaluation : IEvaluation
	{
		public Evaluation(Outcome outcome, Exception handledExpectedException)
			: this(outcome, new Error(handledExpectedException)) {}

		public Evaluation(Outcome outcome, params IError[] errors)
		{
			Errors = errors;
			Outcome = outcome;
		}

		public IError[] Errors { get; private set; }
		public Outcome Outcome { get; private set; }
	}

	public class Evaluation<TResult> : Evaluation,
		IEvaluation<TResult>
	{
		public Evaluation(Outcome outcome, TResult value, params IError[] errors)
			: base(outcome, errors)
		{
			Value = value;
		}

		public TResult Value { get; private set; }
	}

	public class Evaluation<TSubject, TResult> : Evaluation<TResult>,
		IEvaluation<TSubject, TResult>
	{
		private readonly ISpecification<TSubject, TResult> _specification;

		public Evaluation([NotNull] ISpecification<TSubject, TResult> specification,
			Outcome outcome,
			TResult value,
			params IError[] errors)
			: base(outcome, value, errors)
		{
			_specification = specification.ValidateArgumentIsNotNull();
		}

		public IEvaluation<TResult> Evaluate(TSubject subject)
		{
			return _specification.Evaluate(subject);
		}
	}
}
