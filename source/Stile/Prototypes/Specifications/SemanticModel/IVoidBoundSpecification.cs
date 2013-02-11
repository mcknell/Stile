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
	public interface IThrowingBoundSpecification : IThrowingSpecification,
		IBoundSpecification {}

	public interface IThrowingBoundSpecification<TSubject> : IThrowingBoundSpecification,
		IThrowingSpecification<TSubject> {}

	public interface IThrowingBoundSpecification<TSubject, TException> : IThrowingBoundSpecification<TSubject>,
		IThrowingSpecification<TSubject, TException>
		where TException : Exception
	{
		[NotNull]
		IEvaluation<TException> Evaluate();
	}
}
