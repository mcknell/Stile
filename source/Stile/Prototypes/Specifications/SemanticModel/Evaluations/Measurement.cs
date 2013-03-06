#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Threading.Tasks;
using JetBrains.Annotations;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Evaluations
{
	public interface IMeasurement : IObservation {}

	public interface IMeasurement<out TSubject, out TResult> : IMeasurement,
		IObservation<TSubject>
	{
		[CanBeNull]
		TResult Value { get; }
	}

	public class Measurement<TSubject, TResult> : Observation<TSubject>,
		IMeasurement<TSubject, TResult>
	{
		public Measurement(ISample<TSubject> sample,
			TResult value,
			TaskStatus taskStatus,
			bool timedOut,
			params IError[] errors)
			: base(taskStatus, timedOut, sample, errors)
		{
			Value = value;
		}

		public TResult Value { get; private set; }
	}
}
