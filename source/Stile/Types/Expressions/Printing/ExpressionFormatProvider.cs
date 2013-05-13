#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using Stile.Types.Primitives;
#endregion

namespace Stile.Types.Expressions.Printing
{
	public class ExpressionFormatProvider : IFormatProvider,
		ICustomFormatter
	{
		public static readonly string CSharp4 = "CS4";
		private readonly Dictionary<string, string> _parameterAliases;

		public ExpressionFormatProvider(Dictionary<string, string> parameterAliases = null)
		{
			_parameterAliases = parameterAliases;
		}

		public string Format(string format, object arg, IFormatProvider formatProvider)
		{
			var expression = arg as Expression;
			if (expression == null)
			{
				try
				{
					return HandleOtherFormats(format, arg);
				}
				catch (FormatException e)
				{
					throw new FormatException(ErrorMessages.ExpressionFormatProvider_Format_Invalid.InvariantFormat(format),
						e);
				}
			}

			if (format.IsNeitherNullNorEmpty())
			{
				string upper = format.ToUpper(CultureInfo.InvariantCulture);
				if (upper != CSharp4)
				{
					try
					{
						return HandleOtherFormats(format, arg);
					}
					catch (FormatException e)
					{
						throw new FormatException(
							ErrorMessages.ExpressionFormatProvider_Format_Invalid.InvariantFormat(format), e);
					}
				}
			}

			string value = ExpressionPrinter.Make().Print(expression, _parameterAliases).Value;
			return value;
		}

		public object GetFormat(Type formatType)
		{
			if (formatType == typeof(ICustomFormatter))
			{
				return this;
			}
			return null;
		}

		private static string HandleOtherFormats(string format, object arg)
		{
			var formattable = arg as IFormattable;
			if (formattable != null)
			{
				return formattable.ToString(format, CultureInfo.CurrentCulture);
			}
			if (arg != null)
			{
				return arg.ToString();
			}
			return string.Empty;
		}
	}
}
