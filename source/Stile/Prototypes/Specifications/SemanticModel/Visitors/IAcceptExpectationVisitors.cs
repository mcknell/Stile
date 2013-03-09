#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Visitors
{
	public interface IAcceptExpectationVisitors
	{
		[CanBeNull]
		IAcceptExpectationVisitors Parent { get; }
		void Accept(IExpectationVisitor visitor);
		TData Accept<TData>(IExpectationVisitor<TData> visitor, TData data);
	}
}
