#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
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

	public class Instrument<TSubject, TResult> : Procedure<TSubject>,
		IInstrument<TSubject, TResult>
	{
		private readonly Lazy<Func<TSubject, TResult>> _lazyFunc;

		public Instrument([NotNull] Expression<Func<TSubject, TResult>> expression,
			[CanBeNull] ISource<TSubject> source)
			: base(expression, source, null)
		{
			expression = expression.ValidateArgumentIsNotNull();
			_lazyFunc = new Lazy<Func<TSubject, TResult>>(expression.Compile);
		}

		public override void Accept(ISpecificationVisitor visitor)
		{
			visitor.Visit2(this);
		}

		public override TData Accept<TData>(ISpecificationVisitor<TData> visitor, TData data)
		{
			return visitor.Visit2(this, data);
		}

		public IMeasurement<TSubject, TResult> Measure(ISource<TSubject> source, IDeadline deadline = null)
		{
			source = source.ValidateArgumentIsNotNull();
			ObservationResult<TResult> observationResult = Observe(source, deadline, TakeAction);
			IObservation<TSubject> observation = observationResult.Observation;
			var measurement = new Measurement<TSubject, TResult>(observation, observationResult.Result);
			return measurement;
		}

		IObservation<TSubject> IProcedure<TSubject>.Observe(ISource<TSubject> source, IDeadline deadline)
		{
			return Measure(source, deadline);
		}

		private TResult TakeAction(TSubject subject)
		{
			return _lazyFunc.Value.Invoke(subject);
		}
	}
}
