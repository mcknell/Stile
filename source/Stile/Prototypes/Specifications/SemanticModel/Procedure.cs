#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Prototypes.Specifications.Builders.Lifecycle;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
using Stile.Types.Expressions;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel
{
	public interface IProcedure : IAcceptSpecificationVisitors {}

	public interface IProcedure<TSubject> : IProcedure,
		IHides<IProcedureState<TSubject>>
	{
		[NotNull]
		[System.Diagnostics.Contracts.Pure]
		IObservation<TSubject> Observe([NotNull] ISource<TSubject> source, IDeadline deadline = null);
	}

	public interface IProcedureState : IAcceptSpecificationVisitors
	{
		ILazyDescriptionOfLambda Lambda { get; }
	}

	public interface IProcedureState<TSubject> : IProcedureState,
		IHasSource<TSubject> {}

	public abstract class Procedure : IProcedure,
		IProcedureState
	{
		protected Procedure([NotNull] LambdaExpression lambda, [CanBeNull] IAcceptSpecificationVisitors parent)
		{
			Lambda = new LazyDescriptionOfLambda(lambda.ValidateArgumentIsNotNull());
			Parent = parent;
		}

		public ILazyDescriptionOfLambda Lambda { get; private set; }
		public IAcceptSpecificationVisitors Parent { get; private set; }

		public abstract void Accept(ISpecificationVisitor visitor);
		public abstract TData Accept<TData>(ISpecificationVisitor<TData> visitor, TData data);
	}

	public class Procedure<TSubject> : Procedure,
		IProcedure<TSubject>,
		IProcedureState<TSubject>
	{
		private readonly Lazy<Action<TSubject>> _lazyAction;

		public Procedure([NotNull] Expression<Action<TSubject>> expression, [CanBeNull] ISource<TSubject> source)
			: this(expression, source, expression.Compile) {}

		protected Procedure([NotNull] LambdaExpression lambda,
			[CanBeNull] ISource<TSubject> source,
			Func<Action<TSubject>> actionFactory)
			: base(lambda, source)
		{
			Source = source;
			if (actionFactory != null)
			{
				_lazyAction = new Lazy<Action<TSubject>>(actionFactory);
			}
		}

		public ISource<TSubject> Source { get; private set; }

		public IProcedureState<TSubject> Xray
		{
			get { return this; }
		}

		public override void Accept(ISpecificationVisitor visitor)
		{
			visitor.Visit1(this);
		}

		public override TData Accept<TData>(ISpecificationVisitor<TData> visitor, TData data)
		{
			return visitor.Visit1(this, data);
		}

		public IObservation<TSubject> Observe(ISource<TSubject> source, IDeadline deadline = null)
		{
			source = source.ValidateArgumentIsNotNull();
			ObservationResult<bool> observationResult = Observe(source, deadline, TakeAction);
			return observationResult.Observation;
		}

		protected ObservationResult<TResult> Observe<TResult>(ISource<TSubject> source,
			[CanBeNull] IDeadline deadline,
			Func<TSubject, TResult> operation)
		{
			operation = operation.ValidateArgumentIsNotNull();
			var errors = new List<IError>();
			bool onThisThread = false;
			CancellationToken cancellationToken = CancellationToken.None;
			TimeSpan timeout = Deadline.DefaultTimeout;
			if (deadline != null)
			{
				onThisThread = deadline.OnThisThread;
				cancellationToken = deadline.CancellationToken;
				timeout = deadline.Timeout;
			}
			var millisecondsTimeout = (int) timeout.TotalMilliseconds;

			ISample<TSubject> sample = null;

			var task = new Task<TResult>(() =>
			{
				sample = source.Get();
				TSubject subject = sample.Value;
				return operation.Invoke(subject);
			});

			bool timedOut = false;
			TResult result = default(TResult);
			try
			{
				if (onThisThread)
				{
					task.RunSynchronously();
				}
				else
				{
					task.Start();
				}
				timedOut = !task.Wait(millisecondsTimeout, cancellationToken);
				result = task.Result;
			}
			catch (AggregateException e)
			{
				if (e.InnerException != null)
				{
					errors.Add(new Error(e.InnerException, false));
				}
				else
				{
					foreach (DictionaryEntry dictionaryEntry in e.Data)
					{
						var additionalException = (Exception) dictionaryEntry.Value;
						errors.Add(new Error(additionalException, false));
					}
				}
			}

			if (task.IsCompleted)
			{
				task.Dispose();
			}
			var observation = new Observation<TSubject>(task.Status, timedOut, sample, deadline, errors.ToArray());
			return new ObservationResult<TResult>(observation, result);
		}

		private bool TakeAction(TSubject subject)
		{
			_lazyAction.Value.Invoke(subject);
			return false;
		}

		protected class ObservationResult<TResult>
		{
			public ObservationResult(IObservation<TSubject> observation, [CanBeNull] TResult result)
			{
				Observation = observation.ValidateArgumentIsNotNull();
				Result = result;
			}

			public IObservation<TSubject> Observation { get; private set; }
			[CanBeNull]
			public TResult Result { get; private set; }
		}
	}
}
