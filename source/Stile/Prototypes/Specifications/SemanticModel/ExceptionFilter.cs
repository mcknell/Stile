#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Patterns.Structural.Hierarchy;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel
{
	public interface IExceptionFilter : IAcceptSpecificationVisitors,
		IAcceptExpectationVisitors {}

	public interface IExceptionFilter<TSubject> : IExceptionFilter
	{
		ISource<TSubject> Source { get; }
		IObservation<TSubject> Filter(IObservation<TSubject> observation);
	}

	public interface IExceptionFilter<TSubject, TResult> : IExceptionFilter<TSubject>
	{
		[NotNull]
		IInstrument<TSubject, TResult> Instrument { get; }
		IMeasurement<TSubject, TResult> Filter(IMeasurement<TSubject, TResult> measurement);
	}

	public class ExceptionFilter<TSubject> : IExceptionFilter<TSubject>
	{
		public ExceptionFilter([NotNull] Predicate<Exception> predicate, [NotNull] IProcedure<TSubject> procedure)
			: this(predicate, procedure, procedure.Xray.Source) {}

		protected ExceptionFilter([NotNull] Predicate<Exception> predicate,
			[NotNull] IAcceptSpecificationVisitors parent,
			[CanBeNull] ISource<TSubject> source)
		{
			Predicate = predicate.ValidateArgumentIsNotNull();
			Parent = parent.ValidateArgumentIsNotNull();
			Source = source;
		}

		public IAcceptSpecificationVisitors Parent { get; private set; }
		public ISource<TSubject> Source { get; private set; }

		public TData Accept<TData>(IExpectationVisitor<TData> visitor, TData data)
		{
			return visitor.Visit1(this, data);
		}

		public void Accept(ISpecificationVisitor visitor)
		{
			visitor.Visit1(this);
		}

		public TData Accept<TData>(ISpecificationVisitor<TData> visitor, TData data)
		{
			return visitor.Visit1(this, data);
		}

		public void Accept(IExpectationVisitor visitor)
		{
			visitor.Visit1(this);
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

		IAcceptExpectationVisitors IHasParent<IAcceptExpectationVisitors>.Parent
		{
			get { return null; }
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
		public ExceptionFilter([NotNull] Predicate<Exception> predicate,
			[NotNull] IInstrument<TSubject, TResult> instrument)
			: base(predicate, instrument, instrument.Xray.Source)
		{
			Instrument = instrument;
		}

		public IInstrument<TSubject, TResult> Instrument { get; private set; }

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
