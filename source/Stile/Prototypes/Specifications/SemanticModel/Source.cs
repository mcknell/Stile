#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
using Stile.Prototypes.Time;
using Stile.Types.Expressions;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel
{
	public interface ISource {}

	public interface ISource<out TSubject> : ISource,
		IHides<ISourceState>
	{
		[NotNull]
		ISample<TSubject> Get();
	}

	public interface ISourceState : IAcceptSpecificationVisitors
	{
		Lazy<string> Description { get; }
	}

	public class Source<TSubject> : ISource<TSubject>,
		ISourceState
	{
		private readonly IClock _clock;
		private readonly Lazy<Func<TSubject>> _subjectGetter;

		public Source([NotNull] Expression<Func<TSubject>> expression, IClock clock = null)
			: this(expression.Compile, expression.Body.ToLazyDebugString())
		{
			_clock = clock ?? Clock.SystemClock;
		}

		protected Source(Func<Func<TSubject>> doubleFunc, Lazy<string> description)
		{
			_subjectGetter = new Lazy<Func<TSubject>>(doubleFunc);
			Description = description.ValidateArgumentIsNotNull();
		}

		public Lazy<string> Description { get; private set; }
		public ISourceState Xray
		{
			get { return this; }
		}

		public ISample<TSubject> Get()
		{
			TSubject subject = _subjectGetter.Value.Invoke();
			return new Sample<TSubject>(subject, this, _clock.UtcNow);
		}

		public void Accept(ISpecificationVisitor visitor)
		{
			visitor.Visit1(this);
		}

		public TData Accept<TData>(ISpecificationVisitor<TData> visitor, TData data)
		{
			return visitor.Visit1(this, data);
		}
	}
}
