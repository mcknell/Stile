#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
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
			EnumerableHas,
			EnumerableResult,
			Is,
			HashCode,
			Before,
			AndLater,
			Reason,
			All,
			Procedure,
			Exception,
			Contains,
			ComparableExpectationTerm,
			EnumerableIs
		}

		public static readonly Nonterminal Action = new Nonterminal(Enum.Action);
		public static readonly Nonterminal AndLater = new Nonterminal(Enum.AndLater);
		public static readonly Nonterminal Before = new Nonterminal(Enum.Before);
		public static readonly Nonterminal Deadline = new Nonterminal(Enum.Deadline);
		public static readonly Nonterminal EnumerableHas = new Nonterminal(Enum.EnumerableHas.ToString(),
			IfEnumerable);
		public static readonly Nonterminal Expectation = new Nonterminal(Enum.Expectation);
		public static readonly Nonterminal Has = new Nonterminal(Enum.Has);
		public static readonly Nonterminal HashCode = new Nonterminal(Enum.HashCode);
		public static readonly Nonterminal Inspection = new Nonterminal(Enum.Inspection);
		public static readonly Nonterminal Instrument = new Nonterminal(Enum.Instrument);
		public static readonly Nonterminal Procedure = new Nonterminal(Enum.Procedure);
		public static readonly Nonterminal Reason = new Nonterminal(Enum.Reason);
		public static readonly Nonterminal Source = new Nonterminal(Enum.Source);
		public static readonly Nonterminal Specification = new Nonterminal(Enum.Specification);
		public static readonly IReadOnlyList<Nonterminal> All = new[] {Specification, Source, Inspection};

		protected Nonterminal(Enum token)
			: this(token.ToString()) {}

		public Nonterminal([NotNull] string token, string alias = null)
			: base(token, alias) {}

		public static Symbol Make([NotNull] string token)
		{
			return new Nonterminal(token);
		}
	}
}
