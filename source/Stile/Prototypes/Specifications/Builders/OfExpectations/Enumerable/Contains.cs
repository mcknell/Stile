#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using Stile.Prototypes.Specifications.Grammar;
using Stile.Prototypes.Specifications.Grammar.Metadata;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations.Enumerable
{
	public interface IContains<out TItem> : IAcceptExpectationVisitors
	{
		TItem Item { get; }
	}

	public class Contains<TItem> : IContains<TItem>
	{
		public Contains(TItem item)
		{
			Item = item;
		}

		public TItem Item { get; private set; }

		public IAcceptExpectationVisitors Parent
		{
			get { return null; }
		}

		public void Accept(IExpectationVisitor visitor)
		{
			visitor.Visit1(this);
		}

		public TData Accept<TData>(IExpectationVisitor<TData> visitor, TData data)
		{
			return visitor.Visit1(this, data);
		}
	}
}
