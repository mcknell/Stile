#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Is;
using Stile.Prototypes.Specifications.SemanticModel.Expectations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations.Has
{
	public static class HasExtensions
	{
		[System.Diagnostics.Contracts.Pure]
		public static TSpecification HashCode<TSpecification, TSubject, TResult>(
			this IHas<TSpecification, TSubject, TResult> has, int hashCode)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
		{
			var hashcode = new Hashcode<TSpecification, TSubject, TResult>(has.Xray, hashCode);
			var expectation = new Expectation<TSubject, TResult>(x => x.GetHashCode() == hashCode,
				hashcode,
				has.Xray.ExpectationBuilder.Instrument);
			TSpecification specification = has.Xray.ExpectationBuilder.Make(expectation);
			return specification;
		}
	}

	public interface IHashcodeState<out TSpecification, TSubject, TResult> :
		IExpectationTerm<IHasState<TSpecification, TSubject, TResult>>
		where TSpecification : class, IChainableSpecification
	{
		int Expected { get; }
	}
	public static class Hashcode
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

	public class Hashcode<TSpecification, TSubject, TResult> :
		ExpectationTerm<IHasState<TSpecification, TSubject, TResult>>,
		IHashcodeState<TSpecification, TSubject, TResult>
		where TSpecification : class, IChainableSpecification
	{
		public Hashcode([NotNull] IHasState<TSpecification, TSubject, TResult> prior, int expected)
			: base(prior)
		{
			Expected = expected;
		}

		public override void Accept(IExpectationVisitor visitor)
		{
			visitor.Visit3(this);
		}

		public override TData Accept<TData>(IExpectationVisitor<TData> visitor, TData data)
		{
			return visitor.Visit3(this, data);
		}

		public int Expected { get; private set; }
	}
}
