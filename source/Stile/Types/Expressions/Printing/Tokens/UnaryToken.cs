#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

namespace Stile.Types.Expressions.Printing.Tokens
{
	public class UnaryToken
	{
		public UnaryToken(string prefix, string suffix)
		{
			Prefix = prefix;
			Suffix = suffix;
		}

		public string Prefix { get; private set; }
		public string Suffix { get; private set; }
	}
}
