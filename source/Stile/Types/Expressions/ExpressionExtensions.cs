#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using System;
using System.Linq.Expressions;
using System.Reflection;
using Stile.Readability;
using Stile.Types.Expressions.Printing;
using System.Linq;
using Stile.Types.Enumerables;
#endregion

namespace Stile.Types.Expressions
{
	public static class ExpressionExtensions
	{
		public static string GetBodySubstring(LambdaExpression expression)
		{
			if (expression.Parameters.Count != 1)
			{
				throw new ArgumentOutOfRangeException("expression",
					"Must have exactly one expression parameter but had " + expression.Parameters.Count + ".");
			}
			string bodyDescription = expression.Body.ToDebugString();
			string firstParameterDescription = expression.Parameters[0].ToDebugString();
			if (!bodyDescription.StartsWith(firstParameterDescription))
			{
				throw new ArgumentOutOfRangeException("expression",
					"Body must be of the form 'x => x.<...>' but was '" + bodyDescription + "'.");
			}
			int startIndex = Math.Min(bodyDescription.Length, 1 + firstParameterDescription.Length);
			string bodySubstring = bodyDescription.Substring(startIndex);
			return bodySubstring;
		}

		public static PropertyInfo GetPropertyInfo<THost, TProperty>(this Expression<Func<THost, TProperty>> expression)
		{
			PropertyInfo propertyInfo;
			if (!expression.TryGetPropertyInfo(out propertyInfo))
			{
                //throw new ArgumentOutOfRangeException("expression", expression, "Expression did not describe a property.");
				throw new ArgumentOutOfRangeException("expression", "Expression did not describe a property.");
			}
			return propertyInfo;
		}

		public static string ToDebugString(this Expression expression)
		{
			return ToLazyDebugString(expression).Value;
		}

		public static Lazy<string> ToLazyDebugString(this Expression expression)
		{
			return expression.ToLazyDebugString(Identity.Format);
		}

		public static Lazy<string> ToLazyDebugString(this Expression expression, string format, params object[] args)
		{
			object[] objects = args.Unshift(expression).ToArray();
			return new Lazy<string>(() => string.Format(new ExpressionFormatProvider(), format, objects));
		}

		public static bool TryGetPropertyInfo<THost, TProperty>(this Expression<Func<THost, TProperty>> expression,
			out PropertyInfo propertyInfo)
		{
			var memberExpression = expression.Body as MemberExpression;
			if (memberExpression != null)
			{
				propertyInfo = memberExpression.Member as PropertyInfo;
			}
			else
			{
				propertyInfo = null;
			}
			return propertyInfo != null;
		}
	}
}
