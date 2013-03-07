#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

namespace Stile.Patterns.Behavioral.Visitor
{
	public interface IAcceptVisitors<in TVisitor>
		where TVisitor : class
	{
		void Accept(TVisitor visitor);
	}

	public interface IAcceptVisitors<in TVisitor1, in TVisitor2> : IAcceptVisitors<TVisitor1>
		where TVisitor1 : class
		where TVisitor2 : class, TVisitor1
	{
		TData Accept<TData>(TVisitor2 visitor, TData data);
	}
}
