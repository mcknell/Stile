#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Linq.Expressions;
using JetBrains.Annotations;
using Stile.Types.Expressions.Printing.Tokens;
#endregion

namespace Stile.Types.Expressions.Printing
{
	public interface IPrintStrategy
	{
		TokenFormat Format { get; }
		void Append(string value);
		void Append(Expression expression, bool isTopLevel = false);

		[CanBeNull]
		string ParameterAlias(ParameterExpression expression);

		string Print();
	}
}
