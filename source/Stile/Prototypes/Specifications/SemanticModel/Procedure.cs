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
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Readability;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel
{
	public interface IProcedure {}

	public interface IProcedureState
	{
		Lazy<string> Description { get; }
	}

	public interface IProcedureState<out TSubject> : IProcedure
	{
		[CanBeNull]
		ISource<TSubject> Source { get; }
	}

	public interface IProcedure<TSubject> : IProcedure,
		IHides<IProcedureState<TSubject>>
	{
		IObservation Sample(TSubject subject, IDeadline deadline = null);
		IObservation Sample(Func<TSubject> subjectGetter, IDeadline deadline = null);
	}

	public abstract class Procedure : IProcedure
	{
		protected Procedure(Lazy<string> description)
		{
			Description = description;
		}

		public Lazy<string> Description { get; private set; }

		public static class Trivial<TSubject>
		{
			public static readonly Lazy<Func<TSubject, TSubject>> Map =
				new Lazy<Func<TSubject, TSubject>>(Identity.Map<TSubject>);
		}
	}

	public class Procedure<TSubject> : Procedure,
		IProcedure<TSubject>,
		IProcedureState<TSubject>
	{
		private readonly Lazy<Action<TSubject>> _lazyAction;

		public Procedure([NotNull] Expression<Action<TSubject>> expression, ISource<TSubject> source = null)
			: base(expression.ToLazyDebugString())
		{
			_lazyAction = new Lazy<Action<TSubject>>(expression.Compile);
			Source = source;
		}

		public ISource<TSubject> Source { get; private set; }

		public IProcedureState<TSubject> Xray
		{
			get { return this; }
		}

		public IObservation Sample(TSubject subject, IDeadline deadline = null)
		{
			return Sample(() => subject, deadline);
		}

		public IObservation Sample(Func<TSubject> subjectGetter, IDeadline deadline = null)
		{
// ReSharper disable ReturnValueOfPureMethodIsNotUsed
			subjectGetter.ValidateArgumentIsNotNull();
// ReSharper restore ReturnValueOfPureMethodIsNotUsed
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

			var task = new Task(() =>
			{
				TSubject subject = subjectGetter.Invoke();
				_lazyAction.Value.Invoke(subject);
			});

			bool timedOut = false;
			try
			{
				if (onThisThread)
				{
					task.RunSynchronously();
				} else
				{
					task.Start();
				}
				timedOut = !task.Wait(millisecondsTimeout, cancellationToken);
			} catch (Exception e)
			{
				if (e is AggregateException)
				{
					if (e.InnerException != null)
					{
						errors.Add(new Error(e.InnerException, false));
					} else
					{
						foreach (DictionaryEntry dictionaryEntry in e.Data)
						{
							var additionalException = (Exception) dictionaryEntry.Value;
							errors.Add(new Error(additionalException, false));
						}
					}
				}
			}
			var observation = new Observation(task.Status, timedOut, errors.ToArray());
			return observation;
		}
	}
}
