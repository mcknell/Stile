#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel
{
	public interface IExceptionFilter<TResult>
	{
		/// <summary>
		/// If exception was expected but none was thrown.
		/// </summary>
		/// <param name="result"></param>
		/// <returns></returns>
		IEvaluation<TResult> Fail(TResult result);
		bool TryFilter(TResult result, [NotNull] Exception e, out IEvaluation<TResult> evaluation);
	}

	public class ExceptionFilter<TResult> : IExceptionFilter<TResult>
	{
		public IEvaluation<TResult> Fail(TResult result)
		{
			throw new NotImplementedException();
		}

		public bool TryFilter(TResult result, Exception e, out IEvaluation<TResult> evaluation)
		{
			throw new NotImplementedException();
		}
	}
}
