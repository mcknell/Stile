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
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
using Stile.Types.Expressions;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel
{
	public interface IInstrument : IProcedure {}

	public interface IInstrument<TSubject, out TResult> : IInstrument,
		IProcedure<TSubject>
	{
		IMeasurement<TSubject, TResult> Measure([NotNull] ISource<TSubject> source, IDeadline deadline = null);
	}

	public static class Instrument
	{
		public static Instrument<TSubject, TSubject> GetTrivialBound<TSubject>(ISource<TSubject> source)
		{
			return new Instrument<TSubject, TSubject>(x => x, source);
		}

		public static Instrument<TSubject, TSubject> GetTrivialUnbound<TSubject>()
		{
			return new Instrument<TSubject, TSubject>(x => x, null);
		}
	}

	public class Instrument<TSubject, TResult> : IInstrument<TSubject, TResult>,
		IProcedureState<TSubject>
	{
		private readonly Lazy<Func<TSubject, TResult>> _lazyFunc;

		public Instrument([NotNull] Expression<Func<TSubject, TResult>> expression,
			[CanBeNull] ISource<TSubject> source)
		{
			Expression<Func<TSubject, TResult>> validatedExpression = expression.ValidateArgumentIsNotNull();
			_lazyFunc = new Lazy<Func<TSubject, TResult>>(validatedExpression.Compile);
			Source = source;
			Lambda = new LazyDescriptionOfLambda(expression);
		}

		public ILazyDescriptionOfLambda Lambda { get; private set; }
		public IAcceptSpecificationVisitors Parent
		{
			get { return Source; }
		}
		public ISource<TSubject> Source { get; private set; }
		public IProcedureState<TSubject> Xray
		{
			get { return this; }
		}

		public void Accept(ISpecificationVisitor visitor)
		{
			visitor.Visit2(this);
		}

		public TData Accept<TData>(ISpecificationVisitor<TData> visitor, TData data)
		{
			return visitor.Visit2(this, data);
		}

		public IMeasurement<TSubject, TResult> Measure(ISource<TSubject> source, IDeadline deadline = null)
		{
// ReSharper disable ReturnValueOfPureMethodIsNotUsed
			source.ValidateArgumentIsNotNull();
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

			ISample<TSubject> sample = null;

			var task = new Task<TResult>(() =>
			{
				sample = source.Get();
				TSubject subject = sample.Value;
				return _lazyFunc.Value.Invoke(subject);
			});

			using(task)
			{
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
				catch (Exception e)
				{
					if (e is AggregateException)
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
				}
				var measurement = new Measurement<TSubject, TResult>(sample,
					result,
					task.Status,
					timedOut,
					deadline,
					errors.ToArray());
				return measurement;
			}
		}

		IObservation<TSubject> IProcedure<TSubject>.Observe(ISource<TSubject> source, IDeadline deadline)
		{
			return Measure(source, deadline);
		}
	}
}
