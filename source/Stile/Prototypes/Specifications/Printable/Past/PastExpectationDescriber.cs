#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Text;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Has;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Has.Quantifiers;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Is;
using Stile.Prototypes.Specifications.SemanticModel.Expectations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
using Stile.Resources;
#endregion

namespace Stile.Prototypes.Specifications.Printable.Past
{
	public interface IExpectationFormat<TSubject, TResult>
	{
		string ToString(IExpectation<TSubject, TResult> expectation);
	}

	public class ExpectationFormat<TSubject, TResult> : IExpectationFormat<TSubject, TResult>
	{
		public string ToString(IExpectation<TSubject, TResult> expectation)
		{
			throw new NotImplementedException();
		}
	}

	public interface IPastExpectationDescriber : IExpectationVisitor {}

	public class PastExpectationDescriber : IPastExpectationDescriber
	{
		private readonly StringBuilder _stringBuilder;
		private readonly Stack<IAcceptExpectationVisitors> _terms;

		public PastExpectationDescriber()
		{
			_stringBuilder = new StringBuilder();
			_terms = new Stack<IAcceptExpectationVisitors>();
		}

		public void Visit2<TSubject, TResult>(IExpectation<TSubject, TResult> target)
		{
			IAcceptExpectationVisitors lastTerm = target.ValidateArgumentIsNotNull().Xray.LastTerm;
			_terms.Push(lastTerm);
			IAcceptExpectationVisitors parent = lastTerm.Parent;
			while (parent != null)
			{
				_terms.Push(parent);
				parent = parent.Parent;
			}
			Unwind();
		}

		public void Visit3<TSpecification, TSubject, TResult>(IEqualToState<TSpecification, TSubject, TResult> target) where TSpecification : class, IChainableSpecification
		{
			_stringBuilder.AppendFormat(" {0}", target.ValidateArgumentIsNotNull().Description.Value);
		}

		public void Visit3<TSpecification, TSubject, TResult>(IHas<TSpecification, TSubject, TResult> target)
			where TSpecification : class, IChainableSpecification
		{
			throw new NotImplementedException();
		}

		public void Visit3<TSpecification, TSubject, TResult>(IIs<TSpecification, TSubject, TResult> target)
			where TSpecification : class, IChainableSpecification
		{
			_stringBuilder.AppendFormat(" {0}", PastTenseEvaluations.WouldBe);
			Unwind();
		}

		public void Visit4<TSpecification, TSubject, TResult, TItem>(
			IHasAll<TSpecification, TSubject, TResult, TItem> target)
			where TSpecification : class, ISpecification, IChainableSpecification
		{
			throw new NotImplementedException();
		}

		public override string ToString()
		{
			return _stringBuilder.ToString();
		}

		private void Unwind()
		{
			if (_terms.Count != 0)
			{
				IAcceptExpectationVisitors next = _terms.Pop();
				next.Accept(this);
			}
		}
	}
}
