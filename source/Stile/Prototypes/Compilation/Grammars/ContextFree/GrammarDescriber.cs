#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
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

		public void Visit(IChoice target)
		{
			Iterate(target.Sequences, " | ");
		}

		public void Visit(IGrammar target)
		{
			Iterate(target.Productions, Environment.NewLine, allowParens : false);
		}

		public void Visit(IItem target)
		{
			target.Primary.Accept(this);
			while (_stringBuilder.Length > 0 && _stringBuilder[_stringBuilder.Length - 1] == ' ')
			{
				_stringBuilder.Length--;
			}
			_stringBuilder.AppendFormat("{0} ", target.Cardinality.ToEbnfString());
		}

		public void Visit(IProduction target)
		{
			target.Left.Accept(this);
			_stringBuilder.AppendFormat("{0} ", TerminalSymbol.EBNFAssignment);
			target.Right.Accept(this);
		}

		public void Visit(IProductionRule target)
		{
			target.Left.Accept(this);
			_stringBuilder.AppendFormat("{0} ", TerminalSymbol.EBNFAssignment);
			target.Right.Accept(this);
		}

		public void Visit(ISequence target)
		{
			Iterate(target.Items);
		}

		public void Visit(IClause target)
		{
			Iterate(target.Members, continuation : () => _stringBuilder.Append(target.Cardinality.ToEbnfString()));
		}

		public void Visit(Symbol target)
		{
			_stringBuilder.AppendFormat("{0} ", target.Alias ?? target.Token);
		}

		public static string Describe(IGrammar grammar)
		{
			var describer = new GrammarDescriber();
			describer.Visit(grammar);
			return describer.ToString();
		}

		public override string ToString()
		{
			return _stringBuilder.Replace("  ", " ").Replace("( ", "(").Replace(" )", ")").ToString().Trim();
		}

		private void Iterate<TAccepter>(IReadOnlyList<TAccepter> list,
			string separator = " ",
			Action continuation = null,
			bool allowParens = true) where TAccepter : IAcceptGrammarVisitors
		{
			if (allowParens && list.Count > 1)
			{
				_stringBuilder.Append("(");
			}
			foreach (TAccepter accepter in list.SkipWith(x => x.Accept(this)))
			{
				_stringBuilder.Append(separator);
				accepter.Accept(this);
			}
			if (allowParens && list.Count > 1)
			{
				_stringBuilder.Append(")");
			}
			if (continuation != null)
			{
				continuation.Invoke();
			}
			_stringBuilder.Append(" ");
		}
	}
}
