﻿#region License info...
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
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Types.Expressions;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel
{
	public interface IInstrument : IProcedure {}

	public interface IInstrument<TSubject, out TResult> : IInstrument,
		IProcedure<TSubject>
	{
		IMeasurement<TResult> Sample(TSubject subject, IDeadline deadline = null);
		IMeasurement<TResult> Sample([NotNull] Func<TSubject> subjectGetter, IDeadline deadline = null);
	}

	public class Instrument<TSubject, TResult> : IInstrument<TSubject, TResult>,
		IProcedureState<TSubject>
	{
		private readonly Lazy<Func<TSubject, TResult>> _lazyFunc;

		public Instrument([NotNull] Expression<Func<TSubject, TResult>> expression, ISource<TSubject> source = null)
		{
			Expression<Func<TSubject, TResult>> validatedExpression = expression.ValidateArgumentIsNotNull();
			_lazyFunc = new Lazy<Func<TSubject, TResult>>(validatedExpression.Compile);
			Description = validatedExpression.ToLazyDebugString();
			Source = source;
		}

		public Lazy<string> Description { get; private set; }
		public ISource<TSubject> Source { get; private set; }
		public IProcedureState<TSubject> Xray
		{
			get { return this; }
		}

		public IMeasurement<TResult> Sample(TSubject subject, IDeadline deadline = null)
		{
			return Sample(() => subject, deadline);
		}

		public IMeasurement<TResult> Sample(Func<TSubject> subjectGetter, IDeadline deadline = null)
		{
			subjectGetter.ValidateArgumentIsNotNull();
			var errors = new List<IError>();
			bool onThisThread = false;
			CancellationToken cancellationToken = CancellationToken.None;
			TimeSpan timeout = Deadline.DefaultTimeout;
			if (deadline != null)
			{
				onThisThread = deadline.OnThisThread;
				cancellationToken = deadline.CancellationToken ?? cancellationToken;
				timeout = deadline.Timeout ?? timeout;
			}
			var millisecondsTimeout = (int) timeout.TotalMilliseconds;

			var task = new Task<TResult>(() =>
			{
				TSubject subject = subjectGetter.Invoke();
				return _lazyFunc.Value.Invoke(subject);
			});

			bool timedOut = false;
			TResult result = default(TResult);
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
				result = task.Result;
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
			var measurement = new Measurement<TResult>(result, task.Status, timedOut, errors.ToArray());
			return measurement;
		}

		void IProcedure<TSubject>.Sample(TSubject subject, CancellationToken? cancellationToken)
		{
			((IInstrument<TSubject, TResult>) this).Sample(subject);
		}
	}
}
