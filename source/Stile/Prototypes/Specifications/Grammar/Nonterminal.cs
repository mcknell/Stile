#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Prototypes.Compilation.Grammars;
#endregion

namespace Stile.Prototypes.Specifications.Grammar
{
	public class Nonterminal : NonterminalSymbol
	{
		public enum Enum
		{
			Start,
			Specification,
			Source,
			Inspection,
			Expectation,
			Deadline,
			Instrument,
			Action,
			Has,
			Is,
			HashCode,
			Before,
			AndLater,
			Reason
		}

		public static readonly Symbol Action = new Nonterminal(Enum.Action);
		public static readonly Symbol AndLater = new Nonterminal(Enum.AndLater);
		public static readonly Symbol Before = new Nonterminal(Enum.Before);
		public static readonly Symbol Deadline = new Nonterminal(Enum.Deadline);
		public static readonly Symbol Expectation = new Nonterminal(Enum.Expectation);
		public static readonly Symbol Has = new Nonterminal(Enum.Has);
		public static readonly Symbol HashCode = new Nonterminal(Enum.HashCode);
		public static readonly Symbol Inspection = new Nonterminal(Enum.Inspection);
		public static readonly Symbol Instrument = new Nonterminal(Enum.Instrument);
		public static readonly Symbol Reason = new Nonterminal(Enum.Reason);
		public static readonly Symbol Source = new Nonterminal(Enum.Source);
		public static readonly Symbol Specification = new Nonterminal(Enum.Specification);

		protected Nonterminal(Enum token)
			: this(token.ToString()) {}

		public Nonterminal([NotNull] string token)
			: base(token) {}

		public static Symbol Make([NotNull] string token)
		{
			return new Nonterminal(token);
		}
	}
}
