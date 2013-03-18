#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Has;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Has.Quantifiers;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Is;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Expectations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
using Stile.Readability;
#endregion

namespace Stile.Prototypes.Specifications.Printable.Specifications.Should
{
	public interface IShouldExpectationDescriber : IExpectationVisitor {}

	public class ShouldExpectationDescriber : Describer<IShouldExpectationDescriber, IAcceptExpectationVisitors>,
		IShouldExpectationDescriber
	{
		public void Visit1<TSubject>(IExceptionFilter<TSubject> target)
		{
			throw new NotImplementedException();
		}

		public void Visit2<TSubject, TResult>(IExceptionFilter<TSubject, TResult> target)
		{
			throw new NotImplementedException();
		}

		public void Visit2<TSubject, TResult>(IExpectation<TSubject, TResult> target)
		{
			IAcceptExpectationVisitors lastTerm = target.ValidateArgumentIsNotNull().Xray.LastTerm;
			FillStackAndUnwind(lastTerm);
		}

		public void Visit3<TSpecification, TSubject, TResult>(IComparablyEquivalentTo<TSpecification, TSubject, TResult> target) where TSpecification : class, IChainableSpecification
		{
			string s = target.Prior.Negated
				? ShouldSpecifications.ComparablyEquivalentToNegated
				: ShouldSpecifications.ComparablyEquivalentTo;
			AppendFormat(" {0} {1}", s, target.Expected.ToDebugString());
		}

		public void Visit3<TSpecification, TSubject, TResult>(IEmpty<TSpecification, TSubject, TResult> target)
			where TSpecification : class, IChainableSpecification
		{
			AppendFormat(" {0}", ShouldSpecifications.Empty);
		}

		public void Visit3<TSpecification, TSubject, TResult>(
			IEqualToState<TSpecification, TSubject, TResult> target)
			where TSpecification : class, IChainableSpecification
		{
			AppendFormat(" {0}", target.Description.Value);
		}

		public void Visit3<TSpecification, TSubject, TResult>(IHas<TSpecification, TSubject, TResult> target)
			where TSpecification : class, IChainableSpecification
		{
			AppendFormat(" {0}", ShouldSpecifications.ShouldHave);
		}

		public void Visit3<TSpecification, TSubject, TResult>(
			IHashcodeState<TSpecification, TSubject, TResult> target)
			where TSpecification : class, IChainableSpecification
		{
			AppendFormat(" {0} {1}", ShouldSpecifications.Hashcode, target.Expected);
		}

		public void Visit3<TSpecification, TSubject, TResult>(IIs<TSpecification, TSubject, TResult> target)
			where TSpecification : class, IChainableSpecification
		{
			AppendFormat(" {0}", target.Xray.Negated ? ShouldSpecifications.ShouldNotBe : ShouldSpecifications.ShouldBe);
		}

		public void Visit3<TSpecification, TSubject, TResult>(INullState<TSpecification, TSubject, TResult> target) where TSpecification : class, IChainableSpecification where TResult : class
		{
			AppendFormat(" {0}", ShouldSpecifications.Null);
		}

		public void Visit3<TSpecification, TSubject, TResult>(INullableState<TSpecification, TSubject, TResult> target) where TSpecification : class, IChainableSpecification where TResult : struct
		{
			throw new NotImplementedException();
		}

		public void Visit4<TSpecification, TSubject, TResult, TItem>(
			IHasAll<TSpecification, TSubject, TResult, TItem> target)
			where TSpecification : class, ISpecification, IChainableSpecification
		{
			AppendFormat(" {0}", ShouldSpecifications.All);
		}

		public void Visit4<TSpecification, TSubject, TResult, TItem>(
			IItemsSatisfying<TSpecification, TSubject, TResult, TItem> target)
			where TSpecification : class, ISpecification, IChainableSpecification
		{
			AppendFormat(" {0}", target.Description.Value);
		}
	}
}
