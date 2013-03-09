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
using Stile.Readability;
using Stile.Types.Expressions;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel
{
	public interface IProcedure : IAcceptSpecificationVisitors {}

	public interface IProcedure<TSubject> : IProcedure,
		IHides<IProcedureState<TSubject>>
	{
		IObservation<TSubject> Observe([NotNull] ISource<TSubject> source, IDeadline deadline = null);
	}

	public interface IProcedureState : IAcceptSpecificationVisitors
	{
		ILazyDescriptionOfLambda Lambda { get; }
	}

	public interface IProcedureState<out TSubject> : IProcedureState,
		IHasSource<TSubject> {}

	public abstract class Procedure : IProcedure,
		IProcedureState
	{
		protected Procedure(LambdaExpression lambda)
		{
			Lambda = new LazyDescriptionOfLambda(lambda);
		}

		public ILazyDescriptionOfLambda Lambda { get; private set; }
		public abstract IAcceptSpecificationVisitors Parent { get; }

		public abstract void Accept(ISpecificationVisitor visitor);
		public abstract TData Accept<TData>(ISpecificationVisitor<TData> visitor, TData data);

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
			: base(expression)
		{
			_lazyAction = new Lazy<Action<TSubject>>(expression.Compile);
			Source = source;
		}

		public override IAcceptSpecificationVisitors Parent
		{
			get { return Source; }
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

			var task = new Task(() =>
			{
				sample = source.Get();
				TSubject subject = sample.Value;
				_lazyAction.Value.Invoke(subject);
			});

			bool timedOut = false;
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
			var observation = new Observation<TSubject>(task.Status, timedOut, sample, errors.ToArray());
			return observation;
		}
	}
}
