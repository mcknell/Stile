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
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.SemanticModel.Expectations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
using Stile.Readability;
#endregion

namespace Stile.Prototypes.Specifications.Printable.Past
{
	public interface IPastExpectationDescriber : IExpectationVisitor {}

	public class PastExpectationDescriber : Describer<IPastExpectationDescriber, IAcceptExpectationVisitors>,
		IPastExpectationDescriber
	{
		private readonly Outcome _outcome;

		public PastExpectationDescriber(Outcome outcome)
		{
			_outcome = outcome;
		}

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

		public void Visit3<TSpecification, TSubject, TResult>(
			IComparablyEquivalentTo<TSpecification, TSubject, TResult> target)
			where TSpecification : class, IChainableSpecification
		{
			string s = target.Prior.Negated
				? PastTenseEvaluations.ComparablyEquivalentToNegated
				: PastTenseEvaluations.ComparablyEquivalentTo;
			AppendFormat(" {0} {1}", s, target.Expected.ToDebugString());
		}

		public void Visit3<TSpecification, TSubject, TResult>(IEmpty<TSpecification, TSubject, TResult> target)
			where TSpecification : class, IChainableSpecification
		{
			throw new NotImplementedException();
		}

		public void Visit3<TSpecification, TSubject, TResult>(
			IEqualToState<TSpecification, TSubject, TResult> target)
			where TSpecification : class, IChainableSpecification
		{
			AppendFormat(" {0}", target.ValidateArgumentIsNotNull().Description.Value);
		}

		public void Visit3<TSpecification, TSubject, TResult>(IHas<TSpecification, TSubject, TResult> target)
			where TSpecification : class, IChainableSpecification
		{
			throw new NotImplementedException();
		}

		public void Visit3<TSpecification, TSubject, TResult>(
			IHashcodeState<TSpecification, TSubject, TResult> target)
			where TSpecification : class, IChainableSpecification
		{
			throw new NotImplementedException();
		}

		public void Visit3<TSpecification, TSubject, TResult>(IIs<TSpecification, TSubject, TResult> target)
			where TSpecification : class, IChainableSpecification
		{
			string verb = _outcome ? PastTenseEvaluations.Was : PastTenseEvaluations.WouldBe;
			AppendFormat(" {0}", verb);
			if (target.Xray.Negated)
			{
				AppendFormat(" {0}", PastTenseEvaluations.Not);
			}
		}

		public void Visit3<TSpecification, TSubject, TResult>(INullState<TSpecification, TSubject, TResult> target)
			where TSpecification : class, IChainableSpecification where TResult : class
		{
			AppendFormat(" {0}", PastTenseEvaluations.Null);
		}

		public void Visit3<TSpecification, TSubject, TResult>(
			INullableState<TSpecification, TSubject, TResult> target)
			where TSpecification : class, IChainableSpecification where TResult : struct
		{
			throw new NotImplementedException();
		}

		public void Visit4<TSpecification, TSubject, TResult, TItem>(
			IHasAll<TSpecification, TSubject, TResult, TItem> target)
			where TSpecification : class, ISpecification, IChainableSpecification
		{
			throw new NotImplementedException();
		}

		public void Visit4<TSpecification, TSubject, TResult, TItem>(
			IItemsSatisfying<TSpecification, TSubject, TResult, TItem> target)
			where TSpecification : class, ISpecification, IChainableSpecification
		{
			throw new NotImplementedException();
		}
	}
}
