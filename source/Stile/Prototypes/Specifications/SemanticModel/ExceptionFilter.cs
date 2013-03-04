#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel
{
	public interface IExceptionFilter
	{
		IObservation Filter(IObservation observation);
	}

	public interface IExceptionFilter<TResult> : IExceptionFilter
	{
		IMeasurement<TResult> Filter(IMeasurement<TResult> measurement);
	}

	public class ExceptionFilter : IExceptionFilter
	{
		public ExceptionFilter([NotNull] Predicate<Exception> predicate)
		{
			Predicate = predicate.ValidateArgumentIsNotNull();
		}

		public IObservation Filter(IObservation observation)
		{
			List<IError> errors = ExtractHandledErrors(observation);

			var filtered = new Observation(observation.TaskStatus, observation.TimedOut, errors.ToArray());
			return filtered;
		}

		protected Predicate<Exception> Predicate { get; private set; }

		protected List<IError> ExtractHandledErrors(IObservation observation)
		{
			var errors = new List<IError>();
			foreach (IError error in observation.Errors)
			{
				if (Predicate.Invoke(error.Exception))
				{
					errors.Add(new Error(error.Exception, true));
				}
			}
			return errors;
		}
	}

	public class ExceptionFilter<TResult> : ExceptionFilter,
		IExceptionFilter<TResult>
	{
		public ExceptionFilter([NotNull] Predicate<Exception> predicate)
			: base(predicate) {}

		public IMeasurement<TResult> Filter(IMeasurement<TResult> measurement)
		{
			List<IError> errors = ExtractHandledErrors(measurement);

			var filtered = new Measurement<TResult>(measurement.Value,
				measurement.TaskStatus,
				measurement.TimedOut,
				errors.ToArray());
			return filtered;
		}
	}
}
