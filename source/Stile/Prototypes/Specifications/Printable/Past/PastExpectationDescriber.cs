#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Has;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Expectations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
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
		private readonly IPhrasebook _phrasebook;

		public PastExpectationDescriber(IPhrasebook phrasebook = null)
		{
			_phrasebook = phrasebook ?? Phrasebook.Core;
		}

		public void Visit2<TSubject, TResult>(IExpectation<TSubject, TResult> expectation)
		{
			// expectation needs to look up its clause, in the current formatter
			// (this lets it adjust to the tense, etc.)
			IExpectationFormat<TSubject, TResult> format = FindPhrase(expectation);



			// then, expectation can apply its private data to the clause, yielding a string

			// may also need the measurement that the expectation judged
			format.ToString(expectation);
		}

		public void Visit3<TSpecification, TSubject, TResult>(IHas<TSpecification, TSubject, TResult> expectation) where TSpecification : class, IChainableSpecification
		{
			throw new NotImplementedException();
		}

		public IExpectationFormat<TSubject, TResult> FindPhrase<TSubject, TResult>(
			IExpectation<TSubject, TResult> expectation)
		{
			return new ExpectationFormat<TSubject, TResult>();
		}
	}
}
