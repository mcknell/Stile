#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using System.Linq.Expressions;
using Stile.Types.Expressions.Printing.Tokens;
#endregion

namespace Stile.Types.Expressions.Printing
{
	public interface IPrintStrategy
	{
		TokenFormat Format { get; }
		void Append(string value);
		void Append(Expression expression, bool isTopLevel = false);
		string Print();
	}
}
