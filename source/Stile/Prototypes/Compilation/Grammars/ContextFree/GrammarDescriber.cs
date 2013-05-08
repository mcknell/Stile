#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Text;
using Stile.Types.Enumerables;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.ContextFree
{
	public class GrammarDescriber : IGrammarVisitor
	{
		private readonly StringBuilder _stringBuilder;

		public GrammarDescriber()
		{
			_stringBuilder = new StringBuilder();
		}

		public void Visit(IGrammar target)
		{
			foreach (IProductionRule rule in target.ProductionRules.SkipWith(Visit))
			{
				_stringBuilder.Append(Environment.NewLine);
				Visit(rule);
			}
		}

		public void Visit(IProductionRule target)
		{
			_stringBuilder.AppendFormat("{0} {1}", target.Left, TerminalSymbol.EBNFAssignment);
			Visit(target.Right);
		}

		public void Visit(IClause target)
		{
			_stringBuilder.Append(" ");
			if (target.Members.Count > 1)
			{
				_stringBuilder.Append("(");
			}
			foreach (IClauseMember member in target.Members.SkipWith(x => x.Accept(this)))
			{
				_stringBuilder.Append(" ");
				member.Accept(this);
			}
			if (target.Members.Count > 1)
			{
				_stringBuilder.Append(")");
			}
			switch (target.Cardinality)
			{
				case Cardinality.OneOrMore:
					_stringBuilder.Append("+");
					break;
				case Cardinality.ZeroOrMore:
					_stringBuilder.Append("*");
					break;
				case Cardinality.ZeroOrOne:
					_stringBuilder.Append("?");
					break;
			}
		}

		public void Visit(Symbol target)
		{
			_stringBuilder.Append(target.Alias ?? target.Token);
		}

		public static string Describe(IGrammar grammar)
		{
			var describer = new GrammarDescriber();
			describer.Visit(grammar);
			return describer.ToString();
		}

		public override string ToString()
		{
			return _stringBuilder.Replace("  ", " ").Replace("( ", "(").ToString();
		}
	}
}
