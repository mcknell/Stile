#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

namespace Stile.Types.Expressions.Printing.Tokens
{
	public class BinaryOperatorToken
	{
		public BinaryOperatorToken(string value)
		{
			Value = value;
		}

		public string Value { get; private set; }
	}
}
