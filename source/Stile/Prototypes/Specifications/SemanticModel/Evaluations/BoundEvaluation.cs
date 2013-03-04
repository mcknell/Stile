#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Evaluations
{
	public interface IBoundEvaluation : IEvaluation {}

	public interface IBoundEvaluation<in TSubject, out TResult> : IBoundEvaluation,
		IEvaluation<TSubject, TResult>
	{
		IBoundEvaluation<TSubject, TResult> Evaluate(IDeadline deadline = null);
	}

	public class BoundEvaluation<TSubject, TResult> : Evaluation<TSubject, TResult>,
		IBoundEvaluation<TSubject, TResult>
	{
		private readonly IBoundSpecification<TSubject, TResult> _specification;

		public BoundEvaluation([NotNull] IBoundSpecification<TSubject, TResult> specification,
			Outcome outcome,
			TResult value,
			bool timedOut,
			params IError[] errors)
			: base(specification, outcome, value, timedOut, errors)
		{
			_specification = specification;
		}

		public IBoundEvaluation<TSubject, TResult> Evaluate(IDeadline deadline = null)
		{
			return _specification.Evaluate(deadline);
		}
	}
}
