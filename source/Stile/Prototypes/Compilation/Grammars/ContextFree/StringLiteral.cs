#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.ContextFree
{
	public class StringLiteral : TerminalSymbol,
		IPrimary
	{
		public const string DoubleQuote = "\"";

		public StringLiteral([NotNull] string token, string alias = null)
			: base(token, QuoteIfNeeded(alias ?? token)) {}

		public static string QuoteIfNeeded(string s)
		{
			s = s.Trim();
			if (s.StartsWith(DoubleQuote) == false)
			{
				s = DoubleQuote + s;
			}
			if (s.EndsWith(DoubleQuote) == false)
			{
				s = s + DoubleQuote;
			}
			return s;
		}
	}
}
