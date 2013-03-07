#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Visitors
{
	public interface IAcceptSpecificationVisitors
	{
		void Accept(ISpecificationVisitor visitor);
		TData Accept<TData>(ISpecificationVisitor<TData> visitor, TData data);
	}
}
