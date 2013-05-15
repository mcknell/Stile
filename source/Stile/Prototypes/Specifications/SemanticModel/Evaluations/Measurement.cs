#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Evaluations
{
	public interface IMeasurement : IObservation {}

	public interface IMeasurement<TSubject, out TResult> : IMeasurement,
		IObservation<TSubject>
	{
		[CanBeNull]
		TResult Value { get; }
	}

	public class Measurement<TSubject, TResult> : Observation<TSubject>,
		IMeasurement<TSubject, TResult>
	{
		public Measurement(IObservation<TSubject> observation, TResult value)
			: this(
				observation.Sample,
				value,
				observation.TaskStatus,
				observation.TimedOut,
				observation.Deadline,
				observation.Errors.ToArray()) {}

		public Measurement(ISample<TSubject> sample,
			TResult value,
			TaskStatus taskStatus,
			bool timedOut,
			IDeadline deadline,
			params IError[] errors)
			: base(taskStatus, timedOut, sample, deadline, errors)
		{
			Value = value;
		}

		public TResult Value { get; private set; }
	}
}
