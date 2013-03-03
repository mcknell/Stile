#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Types.Expressions;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel
{
	public interface IExpectation<in TResult>
	{
		Lazy<string> Description { get; }
		Outcome Accept(TResult result);
	}

	public class Expectation<TResult> : IExpectation<TResult>
	{
		private readonly Lazy<Func<TResult, Outcome>> _lazyPredicate;

		public Expectation([NotNull] Expression<Func<TResult, Outcome>> expression)
			: this(expression.Compile, expression.ToLazyDebugString()) {}

		private Expectation([NotNull] Func<Func<TResult, Outcome>> predicateSource, [NotNull] Lazy<string> description)
		{
			Func<Func<TResult, Outcome>> source = predicateSource.ValidateArgumentIsNotNull();
			_lazyPredicate = new Lazy<Func<TResult, Outcome>>(source);
			Description = description.ValidateArgumentIsNotNull();
		}

		public Lazy<string> Description { get; private set; }

		public static Expectation<TResult> UnconditionalAcceptance
		{
			get { return new Expectation<TResult>(result => Outcome.Succeeded); }
		}

		public Outcome Accept(TResult result)
		{
			return _lazyPredicate.Value.Invoke(result);
		}
	}
}
