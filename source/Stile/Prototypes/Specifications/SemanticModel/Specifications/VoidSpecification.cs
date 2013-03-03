#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Specifications
{
	public interface IVoidSpecification : ISpecification {}

	public interface IVoidSpecification<TSubject> : IVoidSpecification,
		ISpecification<TSubject>,
		IHides<IVoidSpecificationState<TSubject>>,
		IChainableSpecification
	{
		[NotNull]
		IEvaluation Evaluate(TSubject subject, bool onThisThread = false);
	}

	public interface IVoidSpecificationState {}

	public interface IVoidSpecificationState<TSubject> : IVoidSpecificationState,
		ISpecificationState<TSubject>
	{
		[CanBeNull]
		IExceptionFilter ExceptionFilter { get; }
		[NotNull]
		IProcedure<TSubject> Procedure { get; }
	}

	public static class VoidSpecification
	{
		public delegate TSpecification Factory<out TSpecification, TSubject>(
			[NotNull] IProcedure<TSubject> procedure,
			[NotNull] IExceptionFilter exceptionFilter,
			ISource<TSubject> source = null) where TSpecification : class, ISpecification<TSubject>;
	}

	public class VoidSpecification<TSubject> : Specification<TSubject>,
		IVoidBoundSpecification<TSubject>,
		IVoidSpecificationState<TSubject>
	{
		private readonly ISpecificationDeadline _deadline;

		protected VoidSpecification([NotNull] IProcedure<TSubject> procedure,
			[NotNull] IExceptionFilter exceptionFilter,
			[CanBeNull] ISource<TSubject> source,
			[CanBeNull] string because,
			ISpecificationDeadline deadline = null)
			: base(source, because)
		{
			Procedure = procedure.ValidateArgumentIsNotNull();
			ExceptionFilter = exceptionFilter.ValidateArgumentIsNotNull();
			_deadline = deadline;
		}

		public IExceptionFilter ExceptionFilter { get; private set; }
		public IProcedure<TSubject> Procedure { get; private set; }

		public IVoidSpecificationState<TSubject> Xray
		{
			get { return this; }
		}

		public override ISpecification Clone(ISpecificationDeadline deadline)
		{
			return new VoidSpecification<TSubject>(Procedure, ExceptionFilter, Source, Because, deadline);
		}

		public IEvaluation Evaluate(bool onThisThread = false)
		{
			return Evaluate(Source.Get, onThisThread);
		}

		public IEvaluation Evaluate(TSubject subject, bool onThisThread = false)
		{
			return Evaluate(() => subject, onThisThread);
		}

		public static VoidSpecification<TSubject> Make([NotNull] IProcedure<TSubject> procedure,
			IExceptionFilter exceptionFilter,
			ISource<TSubject> source = null)
		{
			return new VoidSpecification<TSubject>(procedure, exceptionFilter, null, null);
		}

		public static VoidSpecification<TSubject> MakeBound([NotNull] IProcedure<TSubject> procedure,
			[NotNull] IExceptionFilter exceptionFilter,
			[NotNull] ISource<TSubject> source)
		{
			return new VoidSpecification<TSubject>(procedure, exceptionFilter, source, null);
		}

		private IEvaluation Evaluate(Func<TSubject> subjectGetter,
			bool onThisThread,
			CancellationToken? cancellationToken = null)
		{
			TimeSpan timeout = DefaultTimeout;
			if (_deadline != null)
			{
				if (_deadline.Timeout.HasValue)
				{
					timeout = _deadline.Timeout.Value;
				}
				cancellationToken = cancellationToken ?? _deadline.CancellationToken;
			}
			var millisecondsTimeout = (int) timeout.TotalMilliseconds;

			var stopwatch = new Stopwatch();
			var now = DateTime.Now;
			Console.WriteLine("Current Time: {0:HH:mm:ss.fff}", now);
			Console.WriteLine("Expected Timeout duration: {0}ms", timeout.TotalMilliseconds);
			Console.WriteLine("Expected Timeout: {0:HH:mm:ss.fff}", now.Add(timeout));

			Task task;
			bool timedOut;
			try
			{
				task = new Task(() =>
				{
					stopwatch.Start();
					TSubject subject = subjectGetter.Invoke();
					Procedure.Sample(subject);
				});
				if (onThisThread)
				{
					task.RunSynchronously();
				} else
				{
					task.Start();
				}
				if (cancellationToken.HasValue)
				{
					timedOut = !task.Wait(millisecondsTimeout, cancellationToken.Value);
				} else
				{
					timedOut = !task.Wait(millisecondsTimeout);
				}
				stopwatch.Stop();
			} catch (Exception e)
			{
				Exception thrownException = e;
				if (e is AggregateException)
				{
					thrownException = e.InnerException;
					if (thrownException == null)
					{
						foreach (DictionaryEntry dictionaryEntry in e.Data)
						{
							thrownException = (Exception) dictionaryEntry.Value;
							break;
						}
					}
				}
				IEvaluation evaluation;
				if (ExceptionFilter.TryFilterBeforeResult(thrownException, out evaluation))
				{
					Console.WriteLine("Finishing with exception at: {0:HH:mm:ss.fff}", DateTime.Now);
					Console.WriteLine("Elapsed time: {0}ms", stopwatch.ElapsedMilliseconds);
					return evaluation;
				}
				// allow unexpected exceptions to bubble out
				throw;
			}

			// exception was expected but none was thrown
			Console.WriteLine("Finishing with no exception at: {0:HH:mm:ss.fff}", DateTime.Now);
			Console.WriteLine("Elapsed time: {0}ms", stopwatch.ElapsedMilliseconds);
			return ExceptionFilter.FailBeforeResult(timedOut);
		}
	}
}
