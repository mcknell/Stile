#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
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

	public class Evaluation<TValue> : IEvaluation<TValue>
	{
		public Evaluation(Outcome outcome, TValue value, params IError[] errors)
		{
			Outcome = outcome;
			Value = value;
			Errors = errors;
		}

		public IError[] Errors { get; private set; }
		public Outcome Outcome { get; private set; }
		public TValue Value { get; private set; }
	}
}
