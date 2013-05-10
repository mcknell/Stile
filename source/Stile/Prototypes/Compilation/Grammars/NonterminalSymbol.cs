#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Globalization;
using JetBrains.Annotations;
#endregion

namespace Stile.Prototypes.Compilation.Grammars
{
	public class NonterminalSymbol : Symbol
	{
		public const string IfEnumerable = "\"(if enumerable)\"";

		protected NonterminalSymbol([NotNull] string token, string alias = null)
			: base(ToTitleCase(token), alias) {}

		protected static string ToTitleCase(string parameterName)
		{
			if (string.IsNullOrWhiteSpace(parameterName))
			{
				throw new ArgumentOutOfRangeException("parameterName");
			}
			string trimmed = parameterName.Trim();
			return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(trimmed.Substring(0, 1)) + trimmed.Substring(1);
		}
	}
}
