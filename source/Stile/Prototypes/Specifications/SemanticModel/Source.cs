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
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
using Stile.Prototypes.Time;
using Stile.Types.Expressions;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel
{
	public interface ISource : IAcceptSpecificationVisitors {}

	public interface ISource<TSubject> : ISource,
		IHides<ISourceState<TSubject>>
	{
		[NotNull]
		ISample<TSubject> Get();
	}

	public interface ISourceState<TSubject> : IAcceptSpecificationVisitors
	{
		[NotNull]
		Lazy<string> Description { get; }
		[NotNull]
		Expression<Func<TSubject>> Expression { get; }
	}

	public class Source<TSubject> : ISource<TSubject>,
		ISourceState<TSubject>
	{
		private readonly IClock _clock;
		private readonly Lazy<Func<TSubject>> _subjectGetter;

		public Source([NotNull] Expression<Func<TSubject>> expression, IClock clock = null)
			: this(expression.Compile, expression.Body.ToLazyDebugString())
		{
			Expression = expression.ValidateArgumentIsNotNull();
			_clock = clock ?? Clock.SystemClock;
		}

		private Source(Func<Func<TSubject>> doubleFunc, Lazy<string> description)
		{
			_subjectGetter = new Lazy<Func<TSubject>>(doubleFunc);
			Description = description.ValidateArgumentIsNotNull();
		}

		public Lazy<string> Description { get; private set; }
		public Expression<Func<TSubject>> Expression { get; private set; }
		public IAcceptSpecificationVisitors Parent
		{
			get { return null; }
		}
		public ISourceState<TSubject> Xray
		{
			get { return this; }
		}

		public void Accept(ISpecificationVisitor visitor)
		{
			visitor.Visit1(this);
		}

		public TData Accept<TData>(ISpecificationVisitor<TData> visitor, TData data)
		{
			return visitor.Visit1(this, data);
		}

		public ISample<TSubject> Get()
		{
			TSubject subject = _subjectGetter.Value.Invoke();
			return new Sample<TSubject>(subject, this, _clock.UtcNow);
		}
	}
}
