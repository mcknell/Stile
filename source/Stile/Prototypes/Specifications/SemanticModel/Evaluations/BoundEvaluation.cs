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
			bool timedOut,
			params IError[] errors)
			: base(specification, outcome, value, timedOut, errors)
		{
			_specification = specification;
		}

		public IBoundEvaluation<TSubject, TResult> Evaluate()
		{
			return _specification.Evaluate();
		}
	}
}
