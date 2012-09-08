#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using System.Linq.Expressions;
using NUnit.Framework;
using Stile.Types.Expressions.Printing;
#endregion

namespace Stile.Tests.Types.Expressions
{
	[TestFixture]
	public class ExpressionPrinterFixture : ExpressionPrintingFixture<ExpressionPrinterFixture>
	{
		private ExpressionPrinter _expressionPrinter;

		protected override void AfterInit()
		{
			_expressionPrinter = ExpressionPrinter.Make();
		}

		protected override string PrintExpression(Expression expression)
		{
			string print = _expressionPrinter.Print(expression).Value;
			return print;
		}
	}
}
