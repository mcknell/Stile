#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Linq.Expressions;
#endregion

namespace Stile.Types.Expressions
{
	public static class ExpressionTypeExtensions
	{
		public static bool IsBinaryExpressionAndNotAnAssignment(this ExpressionType expressionType,
			VersionedLanguage versionedLanguage = VersionedLanguage.CSharp4)
		{
			if (versionedLanguage == VersionedLanguage.CSharp4)
			{
				switch (expressionType)
				{
					case ExpressionType.Add:
					case ExpressionType.AddChecked:
					case ExpressionType.And:
					case ExpressionType.AndAlso:
					case ExpressionType.ArrayIndex:
					case ExpressionType.Assign:
					case ExpressionType.Coalesce:
					case ExpressionType.Divide:
					case ExpressionType.Equal:
					case ExpressionType.ExclusiveOr:
					case ExpressionType.GreaterThan:
					case ExpressionType.GreaterThanOrEqual:
					case ExpressionType.LeftShift:
					case ExpressionType.LessThan:
					case ExpressionType.LessThanOrEqual:
					case ExpressionType.Modulo:
					case ExpressionType.Multiply:
					case ExpressionType.MultiplyChecked:
					case ExpressionType.NotEqual:
					case ExpressionType.Or:
					case ExpressionType.OrElse:
					case ExpressionType.Power:
					case ExpressionType.RightShift:
					case ExpressionType.Subtract:
					case ExpressionType.SubtractChecked:
						return true;
				}
			}
			return false;
		}
	}
}
