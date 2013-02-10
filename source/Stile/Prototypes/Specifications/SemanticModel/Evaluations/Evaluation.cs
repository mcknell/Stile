#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Evaluations
{
	public interface IEvaluation
	{
		IError[] Errors { get; }
		Outcome Outcome { get; }
		/// <summary>
		/// Note: This typewashed property is susceptible to boxing.
		/// </summary>
		[CanBeNull]
		object Value { get; }
	}

	public interface IEvaluation<out TResult> : IEvaluation
	{
		new TResult Value { get; }
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
		object IEvaluation.Value
		{
			get { return Value; }
		}
	}
}
