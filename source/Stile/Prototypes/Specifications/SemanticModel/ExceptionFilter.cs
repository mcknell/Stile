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
	public interface IExceptionFilter<TSubject>
	{
		IObservation<TSubject> Filter(IObservation<TSubject> observation);
	}

	public interface IExceptionFilter<TSubject, TResult> : IExceptionFilter<TSubject>
	{
		IMeasurement<TSubject, TResult> Filter(IMeasurement<TSubject, TResult> measurement);
	}

	public class ExceptionFilter<TSubject> : IExceptionFilter<TSubject>
	{
		public ExceptionFilter([NotNull] Predicate<Exception> predicate)
		{
			Predicate = predicate.ValidateArgumentIsNotNull();
		}

		public IObservation<TSubject> Filter(IObservation<TSubject> observation)
		{
			List<IError> errors = ExtractHandledErrors(observation);

			var filtered = new Observation<TSubject>(observation.TaskStatus,
				observation.TimedOut,
				observation.Sample,
				errors.ToArray());
			return filtered;
		}

		protected Predicate<Exception> Predicate { get; private set; }

		protected List<IError> ExtractHandledErrors(IObservation<TSubject> observation)
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

	public class ExceptionFilter<TSubject, TResult> : ExceptionFilter<TSubject>,
		IExceptionFilter<TSubject, TResult>
	{
		public ExceptionFilter([NotNull] Predicate<Exception> predicate)
			: base(predicate) {}

		public IMeasurement<TSubject, TResult> Filter(IMeasurement<TSubject, TResult> measurement)
		{
			List<IError> errors = ExtractHandledErrors(measurement);

			var filtered = new Measurement<TSubject, TResult>(measurement.Sample,
				measurement.Value,
				measurement.TaskStatus,
				measurement.TimedOut,
				errors.ToArray());
			return filtered;
		}
	}
}
