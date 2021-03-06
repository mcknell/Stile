﻿#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Enumerable;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Has;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Has.Quantifiers;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Is;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Expectations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
using Stile.Readability;
using Stile.Types.Comparison;
using Stile.Types.Enums;
#endregion

namespace Stile.Prototypes.Specifications.Printable.Should
{
	public interface IShouldExpectationDescriber : IExpectationVisitor {}

	public class ShouldExpectationDescriber : Describer<IShouldExpectationDescriber, IAcceptExpectationVisitors>,
		IShouldExpectationDescriber
	{
		public ShouldExpectationDescriber([CanBeNull] ISource source)
			: base(source) {}

		public void Visit(IGetsMeasuredState target)
		{
			AppendFormat(" {0}", ShouldSpecifications.GetMeasured);
		}

		public void Visit1<TItem>(IContains<TItem> target)
		{
			Append(" ");
			AppendFormat(ShouldSpecifications.ShouldContain, target.Item.ToDebugString());
		}

		public void Visit1<TItem>(ISequenceEqual<TItem> target)
		{
			AppendFormat(" {0} {1}", ShouldSpecifications.SequenceEqual, target.Expected.ToDebugString());
		}

		public void Visit2<TSubject, TResult>(IExceptionFilter<TSubject, TResult> target)
		{
			Append(" ");
			AppendFormat(ShouldSpecifications.ShouldThrow, target.Description.Value);
		}

		public void Visit2<TSubject, TResult>(IExpectation<TSubject, TResult> target)
		{
			IAcceptExpectationVisitors lastTerm = target.ValidateArgumentIsNotNull().Xray.LastTerm;
			FillStackAndUnwind(lastTerm);
		}

		public void Visit3<TSpecification, TSubject, TResult>(
			IComparablyEquivalentTo<TSpecification, TSubject, TResult> target)
			where TSpecification : class, IChainableSpecification where TResult : IComparable<TResult>
		{
			string s;
			switch (target.Comparison)
			{
				case ComparisonRelation.Equal:
					s = target.Prior.Negated
						? ShouldSpecifications.ComparablyEquivalentToNegated
						: ShouldSpecifications.ComparablyEquivalentTo;
					break;
				case ComparisonRelation.GreaterThan:
					s = ShouldSpecifications.GreaterThan;
					break;
				case ComparisonRelation.GreaterThanOrEqual:
					s = ShouldSpecifications.GreaterThanOrEqualTo;
					break;
				case ComparisonRelation.LessThan:
					s = ShouldSpecifications.LessThan;
					break;
				case ComparisonRelation.LessThanOrEqual:
					s = ShouldSpecifications.LessThanOrEqualTo;
					break;
				default:
					throw Enumeration.FailedToRecognize(() => target.Comparison);
			}
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

		public void Visit3<TSpecification, TSubject, TResult>(INullState<TSpecification, TSubject, TResult> target)
			where TSpecification : class, IChainableSpecification where TResult : class
		{
			AppendFormat(" {0}", ShouldSpecifications.Null);
		}

		public void Visit3<TSpecification, TSubject, TResult>(
			INullableState<TSpecification, TSubject, TResult> target)
			where TSpecification : class, IChainableSpecification where TResult : struct
		{
			AppendFormat(" {0}", ShouldSpecifications.Null);
		}

		public void Visit4<TSpecification, TSubject, TResult, TItem>(
			IAll<TSpecification, TSubject, TResult, TItem> target)
			where TSpecification : class, ISpecification, IChainableSpecification
		{
			AppendFormat(" {0} {1}", ShouldSpecifications.All, ShouldSpecifications.Items);
		}

		public void Visit4<TSpecification, TSubject, TResult, TItem>(
			IAtLeast<TSpecification, TSubject, TResult, TItem> target)
			where TSpecification : class, ISpecification, IChainableSpecification
		{
			string item = PluralizeItem(target);
			AppendFormat(" {0} {1} {2}", ShouldSpecifications.AtLeast, target.Limit, item);
		}

		public void Visit4<TSpecification, TSubject, TResult, TItem>(
			IAtMost<TSpecification, TSubject, TResult, TItem> target)
			where TSpecification : class, ISpecification, IChainableSpecification
		{
			string item = PluralizeItem(target);
			AppendFormat(" {0} {1} {2}", ShouldSpecifications.AtMost, target.Limit, item);
		}

		public void Visit4<TSpecification, TSubject, TResult, TItem>(
			IExactly<TSpecification, TSubject, TResult, TItem> target)
			where TSpecification : class, ISpecification, IChainableSpecification
		{
			string item = PluralizeItem(target);
			AppendFormat(" {0} {1} {2}", ShouldSpecifications.Exactly, target.Limit, item);
		}

		public void Visit4<TSpecification, TSubject, TResult, TItem>(
			IFewerThan<TSpecification, TSubject, TResult, TItem> target)
			where TSpecification : class, ISpecification, IChainableSpecification
		{
			string item = PluralizeItem(target);
			AppendFormat(" {0} {1} {2}", ShouldSpecifications.FewerThan, target.Limit, item);
		}

		public void Visit4<TSpecification, TSubject, TResult, TItem>(
			IItemsFailing<TSpecification, TSubject, TResult, TItem> target)
			where TSpecification : class, ISpecification, IChainableSpecification
		{
			AppendFormat(" {0} '{1}'", ShouldSpecifications.Failing, target.Description.Value);
		}

		public void Visit4<TSpecification, TSubject, TResult, TItem>(
			IItemsSatisfying<TSpecification, TSubject, TResult, TItem> target)
			where TSpecification : class, ISpecification, IChainableSpecification
		{
			AppendFormat(" {0}", target.Description.Value);
		}

		public void Visit4<TSpecification, TSubject, TResult, TItem>(
			IMoreThan<TSpecification, TSubject, TResult, TItem> target)
			where TSpecification : class, ISpecification, IChainableSpecification
		{
			string item = PluralizeItem(target);
			AppendFormat(" {0} {1} {2}", ShouldSpecifications.MoreThan, target.Limit, item);
		}

		public void Visit4<TSpecification, TSubject, TResult, TItem>(
			INo<TSpecification, TSubject, TResult, TItem> target)
			where TSpecification : class, ISpecification, IChainableSpecification
		{
			AppendFormat(" {0} {1}", ShouldSpecifications.No, ShouldSpecifications.Items);
		}

		private static string PluralizeItem<TSpecification, TSubject, TResult, TItem>(
			ICountedLimit<TSpecification, TSubject, TResult, TItem> target)
			where TSpecification : class, ISpecification, IChainableSpecification
		{
			return target.Limit.Pluralize(ShouldSpecifications.Item, ShouldSpecifications.Items);
		}
	}
}
