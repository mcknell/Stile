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
	public interface IObservation
	{
		IError[] Errors { get; }
		TaskStatus TaskStatus { get; }
	}

	public class Observation : IObservation
	{
		public Observation(TaskStatus taskStatus, params IError[] errors)
		{
			Errors = errors;
			TaskStatus = taskStatus;
		}

		public IError[] Errors { get; private set; }
		public TaskStatus TaskStatus { get; private set; }
	}
}
