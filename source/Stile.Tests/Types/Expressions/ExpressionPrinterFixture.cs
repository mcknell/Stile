#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
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
