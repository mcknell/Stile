#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using System;
using Stile.Readability;
using Stile.Types;
#endregion

namespace Stile.Patterns.SelfDescribingPredicates
{
	public class Evaluation : IEvaluation
	{
		protected static readonly Lazy<string> NullExplanation = Null.String.ToLazy();

		protected Evaluation(bool outcome, Lazy<string> lazyReason)
		{
			LazyReason = lazyReason;
			Success = outcome;
		}

		public Lazy<string> LazyReason { get; private set; }
		public string Reason
		{
			get
			{
				string value = LazyReason.Value;
				return value;
			}
		}

		public bool Success { get; private set; }
	}

	public class Evaluation<TSubject> : Evaluation,
		IEvaluation<TSubject>
	{
		public Evaluation(TSubject result)
			: this(true, result) {}

		public Evaluation(string failureReason, TSubject result)
			: this(false, result, failureReason.ToLazy()) {}

		public Evaluation(Lazy<string> lazyFailureReason, TSubject result)
			: this(false, result, lazyFailureReason) {}

		protected Evaluation(bool outcome, TSubject result, Lazy<string> lazyFailureReason = null)
			: base(outcome, lazyFailureReason ?? NullExplanation)
		{
			Result = result;
		}

		public TSubject Result { get; private set; }
	}
}
