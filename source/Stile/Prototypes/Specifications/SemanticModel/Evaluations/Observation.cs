#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Evaluations
{
	public interface IObservation
	{
		[NotNull]
		IDeadline Deadline { get; }
		[NotNull]
		IReadOnlyList<IError> Errors { get; }
		TaskStatus TaskStatus { get; }
		bool TimedOut { get; }
	}

	public interface IObservation<TSubject> : IObservation
	{
		[CanBeNull]
		ISample<TSubject> Sample { get; }
	}

	public class Observation : IObservation
	{
		public Observation(TaskStatus taskStatus,
			bool timedOut,
			[NotNull] IDeadline deadline,
			params IError[] errors)
		{
			Errors = errors;
			TaskStatus = taskStatus;
			TimedOut = timedOut;
			Deadline = deadline;
		}

		public IDeadline Deadline { get; private set; }

		public IReadOnlyList<IError> Errors { get; private set; }
		public TaskStatus TaskStatus { get; private set; }
		public bool TimedOut { get; private set; }
	}

	public class Observation<TSubject> : Observation,
		IObservation<TSubject>
	{
		public Observation(TaskStatus taskStatus,
			bool timedOut,
			[CanBeNull] ISample<TSubject> sample,
			IDeadline deadline,
			params IError[] errors)
			: base(taskStatus, timedOut, deadline, errors)
		{
			Sample = sample;
		}

		public ISample<TSubject> Sample { get; private set; }
	}
}
