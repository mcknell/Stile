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
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel
{
	public interface IExceptionFilter : IAcceptSpecificationVisitors {}

	public interface IExceptionFilter<TSubject> : IExceptionFilter
	{
		[NotNull]
		Lazy<string> Description { get; }
		[NotNull]
		Predicate<Exception> Predicate { get; }
		[CanBeNull]
		ISource<TSubject> Source { get; }

		[NotNull]
		IObservation<TSubject> Filter(IObservation<TSubject> observation);
	}

	public interface IExceptionFilter<TSubject, TResult> : IExceptionFilter<TSubject>,
		IAcceptExpectationVisitors
	{
		[NotNull]
		IInstrument<TSubject, TResult> Instrument { get; }

		[NotNull]
		IMeasurement<TSubject, TResult> Filter(IMeasurement<TSubject, TResult> measurement);
	}

	public class ExceptionFilter<TSubject> : IExceptionFilter<TSubject>
	{
		public ExceptionFilter([NotNull] Predicate<Exception> predicate,
			[NotNull] IProcedure<TSubject> procedure,
			Lazy<string> description)
			: this(predicate, procedure, procedure.Xray.Source, description) {}

		protected ExceptionFilter([NotNull] Predicate<Exception> predicate,
			[NotNull] IAcceptSpecificationVisitors parent,
			[CanBeNull] ISource<TSubject> source,
			[NotNull] Lazy<string> description)
		{
			Description = description;
			Predicate = predicate.ValidateArgumentIsNotNull();
			Parent = parent.ValidateArgumentIsNotNull();
			Source = source;
		}

		public Lazy<string> Description { get; private set; }
		public IAcceptSpecificationVisitors Parent { get; private set; }
		public Predicate<Exception> Predicate { get; private set; }
		public ISource<TSubject> Source { get; private set; }

		public virtual void Accept(ISpecificationVisitor visitor)
		{
			visitor.Visit1(this);
		}

		public virtual TData Accept<TData>(ISpecificationVisitor<TData> visitor, TData data)
		{
			return visitor.Visit1(this, data);
		}

		public IObservation<TSubject> Filter(IObservation<TSubject> observation)
		{
			List<IError> errors = MarkHandledErrors(observation);

			var filtered = new Observation<TSubject>(observation.TaskStatus,
				observation.TimedOut,
				observation.Sample,
				observation.Deadline,
				errors.ToArray());
			return filtered;
		}

		protected List<IError> MarkHandledErrors(IObservation<TSubject> observation)
		{
			var errors = new List<IError>();
			foreach (IError error in observation.Errors)
			{
				IError copy = error;
				if (Predicate.Invoke(error.Exception))
				{
					copy = new Error(error.Exception, true);
				}
				errors.Add(copy);
			}
			return errors;
		}
	}

	public class ExceptionFilter<TSubject, TResult> : ExceptionFilter<TSubject>,
		IExceptionFilter<TSubject, TResult>
	{
		public ExceptionFilter([NotNull] Predicate<Exception> predicate,
			[NotNull] IInstrument<TSubject, TResult> instrument,
			[NotNull] Lazy<string> description)
			: base(predicate, instrument, instrument.Xray.Source, description)
		{
			Instrument = instrument;
		}

		public IInstrument<TSubject, TResult> Instrument { get; private set; }

		public virtual TData Accept<TData>(IExpectationVisitor<TData> visitor, TData data)
		{
			return visitor.Visit2(this, data);
		}

		public override void Accept(ISpecificationVisitor visitor)
		{
			visitor.Visit2(this);
		}

		public override TData Accept<TData>(ISpecificationVisitor<TData> visitor, TData data)
		{
			return visitor.Visit2(this, data);
		}

		public virtual void Accept(IExpectationVisitor visitor)
		{
			visitor.Visit2(this);
		}

		public IMeasurement<TSubject, TResult> Filter(IMeasurement<TSubject, TResult> measurement)
		{
			List<IError> errors = MarkHandledErrors(measurement);

			var filtered = new Measurement<TSubject, TResult>(measurement.Sample,
				measurement.Value,
				measurement.TaskStatus,
				measurement.TimedOut,
				measurement.Deadline,
				errors.ToArray());
			return filtered;
		}

		IAcceptExpectationVisitors IHasParent<IAcceptExpectationVisitors>.Parent
		{
			get { return null; }
		}
	}
}
