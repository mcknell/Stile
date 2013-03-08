#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations
{
	public interface IExpectationTerm {}

	public interface IExpectationTerm<out TPrior> : IExpectationTerm
		where TPrior : class, IExpectationTerm
	{
		[CanBeNull]
		TPrior Prior { get; }
	}
}
