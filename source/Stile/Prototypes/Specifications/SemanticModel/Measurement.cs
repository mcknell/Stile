#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Threading.Tasks;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel
{
	public interface IMeasurement : IObservation {}

	public interface IMeasurement<TResult> : IMeasurement {}

	public abstract class Measurement : Observation,
		IMeasurement
	{
		protected Measurement(TaskStatus taskStatus, params IError[] errors)
			: base(taskStatus, errors) {}
	}

	public class Measurement<TResult> : Measurement,
		IMeasurement<TResult>
	{
		public Measurement(TaskStatus taskStatus, params IError[] errors)
			: base(taskStatus, errors) {}
	}
}
