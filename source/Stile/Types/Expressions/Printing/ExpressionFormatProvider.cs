#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using System;
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

		public string Format(string format, object arg, IFormatProvider formatProvider)
		{
			var expression = arg as Expression;
			if (expression == null)
			{
				try
				{
					return HandleOtherFormats(format, arg);
				} catch (FormatException e)
				{
					throw new FormatException(string.Format("The format of '{0}' is invalid.", format), e);
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
					} catch (FormatException e)
					{
						throw new FormatException(string.Format("The format of '{0}' is invalid.", format), e);
					}
				}
			}

			string value = ExpressionPrinter.Make().Print(expression).Value;
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
