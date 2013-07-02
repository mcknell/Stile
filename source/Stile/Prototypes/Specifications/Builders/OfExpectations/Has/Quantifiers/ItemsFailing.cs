#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
using Stile.Types.Expressions;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations.Has.Quantifiers
{
	public interface IItemsFailing<out TSpecification, TSubject, TResult, TItem> :
		IExpectationTerm<IQuantifierState<TSpecification, TSubject, TResult>>
		where TSpecification : class, ISpecification, IChainableSpecification
	{
		[NotNull]
		Lazy<string> Description { get; }
		[NotNull]
		Expression<Func<TItem, bool>> Expression { get; }
	}

	public class ItemsFailing<TSpecification, TSubject, TResult, TItem> :
		ExpectationTerm<IQuantifierState<TSpecification, TSubject, TResult>>,
		IItemsFailing<TSpecification, TSubject, TResult, TItem>
		where TSpecification : class, ISpecification, IChainableSpecification
	{
		public ItemsFailing([NotNull] Expression<Func<TItem, bool>> expression,
			[NotNull] IQuantifierState<TSpecification, TSubject, TResult> prior)
			: base(prior)
		{
			Expression = expression.ValidateArgumentIsNotNull();
			var binaryBody = expression.Body as BinaryExpression;
			if (binaryBody != null && binaryBody.Left == expression.Parameters[0])
			{
				Description = new Lazy<string>(() => TrimParameters(binaryBody));
			}
			else
			{
				Description = expression.ToLazyDebugString();
			}
		}

		public Lazy<string> Description { get; private set; }
		public Expression<Func<TItem, bool>> Expression { get; private set; }

		public override void Accept(IExpectationVisitor visitor)
		{
			visitor.Visit4(this);
		}

		public override TData Accept<TData>(IExpectationVisitor<TData> visitor, TData data)
		{
			return visitor.Visit4(this, data);
		}

		private string TrimParameters(BinaryExpression binaryExpression)
		{
			string debugString = binaryExpression.ToDebugString();
			string withoutParameter = debugString.Substring(binaryExpression.Left.ToDebugString().Length).Trim();
			return withoutParameter;
		}
	}
}
