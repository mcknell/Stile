#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Threading.Tasks;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel
{
	public interface IMeasurement : IObservation {}

	public interface IMeasurement<out TResult> : IMeasurement
	{
		[CanBeNull]
		TResult Value { get; }
	}

	public abstract class Measurement : Observation,
		IMeasurement
	{
		protected Measurement(TaskStatus taskStatus, bool timedOut, params IError[] errors)
			: base(taskStatus, timedOut, errors) {}
	}

	public class Measurement<TResult> : Measurement,
		IMeasurement<TResult>
	{
		public Measurement(TResult value, TaskStatus taskStatus, bool timedOut, params IError[] errors)
			: base(taskStatus, timedOut, errors)
		{
			Value = value;
		}

		public TResult Value { get; private set; }
	}
}
