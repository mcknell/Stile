#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using NUnit.Framework;
using Stile.Prototypes.Compilation.Grammars.ContextFree;
using Stile.Prototypes.Compilation.Grammars.ContextFree.Builders;
using Stile.Prototypes.Specifications.Grammar;
#endregion

namespace Stile.Tests.Prototypes.Compilation.Grammars.ContextFree.Builders
{
	[TestFixture]
	public class ProductionConsolidatorFixture : ContextFreeFixtureBase
	{
		[Test]
		public void Consolidates()
		{
			var source = new Item(Nonterminal.Source, Cardinality.ZeroOrOne);
			var instrument = new Item(Nonterminal.Instrument);
			var exceptionSequence = new Sequence(Exception(), Deadline(), Reason());
			var expectation = new Item(Nonterminal.Expectation);
			var deadlineReasonSequence = new Sequence(Deadline(), Reason());
			var optionalExceptionSequence = new Sequence(Exception(Cardinality.ZeroOrOne), Deadline(), Reason());
			var expectationSequence = new Sequence(expectation,
				new Item(new Choice(deadlineReasonSequence, optionalExceptionSequence)));
			var instrumentSequence = new Sequence(instrument,
				new Item(new Choice(exceptionSequence, expectationSequence)));
			var procedure = new Item(Nonterminal.Procedure);
			var procedureSequence = new Sequence(procedure, Exception(), Deadline(), Reason());
			var choice = new Choice(new Sequence(source, new Item(new Choice(instrumentSequence, procedureSequence))));

			AssertPrints(choice,
				"Source? ("
					+ "(Instrument ((Exception Deadline? Reason?) | (Expectation ((Deadline? Reason?) | (Exception? Deadline? Reason?))))) "
					+ "| (Procedure Exception Deadline? Reason?))");

			IChoice consolidated = new ProductionConsolidator(choice).Consolidate();

			AssertPrints(consolidated,
				"Source? ((Instrument (Exception | (Expectation Exception?))) | (Procedure Exception)) Deadline? Reason?");
		}

		private static Item Deadline()
		{
			return new Item(Nonterminal.Deadline, Cardinality.ZeroOrOne);
		}

		private static Item Exception(Cardinality cardinality = Cardinality.One)
		{
			return new Item(Nonterminal.Exception, cardinality);
		}

		private static Item Reason()
		{
			return new Item(Nonterminal.Reason, Cardinality.ZeroOrOne);
		}
	}
}
