#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using Stile.Patterns.Behavioral.Visitor;
using Stile.Patterns.Structural.Hierarchy;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Visitors
{
	public interface IAcceptExpectationVisitors : IAcceptVisitors<IExpectationVisitor>,
		IHasParent<IAcceptExpectationVisitors>
	{
		TData Accept<TData>(IExpectationVisitor<TData> visitor, TData data);
	}
}
