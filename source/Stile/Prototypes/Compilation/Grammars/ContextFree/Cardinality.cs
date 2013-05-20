#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using Stile.Types.Enums;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.ContextFree
{
	public enum Cardinality
	{
		One,
		OneOrMore,
		ZeroOrOne,
		ZeroOrMore
	}

	public static class CardinalityExtensions
	{
		public static string ToEbnfString(this Cardinality cardinality)
		{
			switch (cardinality)
			{
				case Cardinality.One:
					return string.Empty;
				case Cardinality.OneOrMore:
					return "+";
				case Cardinality.ZeroOrMore:
					return "*";
				case Cardinality.ZeroOrOne:
					return "?";
				default:
					throw Enumeration.FailedToRecognize(() => cardinality);
			}
		}
	}
}
