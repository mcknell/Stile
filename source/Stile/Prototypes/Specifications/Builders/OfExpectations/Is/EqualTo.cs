#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.Grammar;
using Stile.Prototypes.Specifications.Grammar.Metadata;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
using Stile.Readability;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations.Is
{
	public interface IEqualToState<out TSpecification, TSubject, TResult> :
		IExpectationTerm<IIsState<TSpecification, TSubject, TResult>>
		where TSpecification : class, IChainableSpecification
	{
		[NotNull]
		Lazy<string> Description { get; }
		TResult Expected { get; }
		[NotNull]
		Expression<Predicate<TResult>> Expression { get; set; }

		[NotNull]
		TSpecification Build();
	}

	public static class EqualTo
	{
		public static TSpecification Make<TSpecification, TSubject, TResult>(
			[NotNull] Expression<Predicate<TResult>> expression,
			TResult expected,
			[NotNull] IIsState<TSpecification, TSubject, TResult> prior)
			where TSpecification : class, IChainableSpecification
		{
			var equalTo = new EqualTo<TSpecification, TSubject, TResult>(expression, prior, expected);
			return equalTo.Build();
		}
	}

	public class EqualTo<TSpecification, TSubject, TResult> :
		ExpectationTerm<IIsState<TSpecification, TSubject, TResult>>,
		IEqualToState<TSpecification, TSubject, TResult>
		where TSpecification : class, IChainableSpecification
	{
		public EqualTo([NotNull] Expression<Predicate<TResult>> expression,
			[NotNull] IIsState<TSpecification, TSubject, TResult> prior,
			TResult expected)
			: base(prior)
		{
			Expression = expression.ValidateArgumentIsNotNull();
			Expected = expected;
			Description = Expected.ToLazyDebugString();
		}

		public Lazy<string> Description { get; private set; }
		public TResult Expected { get; private set; }
		public Expression<Predicate<TResult>> Expression { get; set; }

		public override void Accept(IExpectationVisitor visitor)
		{
			visitor.Visit3(this);
		}

		public override TData Accept<TData>(IExpectationVisitor<TData> visitor, TData data)
		{
			return visitor.Visit3(this, data);
		}

		public TSpecification Build()
		{
			return Prior.BuilderState.Make(Expression, this, Prior.Negated);
		}
	}
}
