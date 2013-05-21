#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using Stile.Prototypes.Specifications.Grammar;
using Stile.Prototypes.Specifications.Grammar.Metadata;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations.Is
{
	public interface IGetsMeasuredState : IAcceptExpectationVisitors {}

	public class GetsMeasured : IGetsMeasuredState
	{
		private static readonly Lazy<GetsMeasured> LazyInstance = new Lazy<GetsMeasured>(Make);

		[Rule(Nonterminal.Enum.Expectation, NameIsSymbol = true)]
		protected GetsMeasured() {}

		public static GetsMeasured Instance
		{
			get { return LazyInstance.Value; }
		}

		public IAcceptExpectationVisitors Parent
		{
			get { return null; }
		}

		public void Accept(IExpectationVisitor visitor)
		{
			visitor.Visit(this);
		}

		public TData Accept<TData>(IExpectationVisitor<TData> visitor, TData data)
		{
			return visitor.Visit(this, data);
		}

		private static GetsMeasured Make()
		{
			return new GetsMeasured();
		}
	}
}
