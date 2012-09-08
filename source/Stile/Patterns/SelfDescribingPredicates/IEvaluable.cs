#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using System;
#endregion

namespace Stile.Patterns.SelfDescribingPredicates
{
	public interface IEvaluable<out TEvaluation> : IDisposable
		where TEvaluation : IEvaluation
	{
		TEvaluation Evaluate();
	}

	public interface IEvaluable : IEvaluable<IEvaluation> {}
}
