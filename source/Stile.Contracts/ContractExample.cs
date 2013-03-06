#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Diagnostics.Contracts;
using Stile.Prototypes.Specifications;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Is;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Contracts
{
	public class ContractExample
	{
		public bool ModIsOdd(int a, int b)
		{
			Contract.Requires(Specify.That(() => b).Is.GreaterThan(0).IsTrue());

			return (a % b) / 2 == 1;
		}
	}
}
