#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Stile.Prototypes.Collections;
using Stile.Prototypes.Compilation.Grammars;
using Stile.Prototypes.Compilation.Grammars.ContextFree;
#endregion

namespace Stile.DocumentationGeneration
{
	public class Generator
	{
		private readonly Reflector _reflector;

		public Generator(params Assembly[] others)
			: this(typeof(VersionedLanguage).Assembly, others) {}

		public Generator([NotNull] Assembly stile, params Assembly[] others)
		{
			_reflector = new Reflector(stile, others);
		}

		[NotNull]
		public string Generate()
		{
			HashBucket<Symbol, IProductionRule> rules = _reflector.FindRules();

			var grammarBuilder = new ContextFreeGrammarBuilder(rules.SelectMany(x => x.Value));

			foreach (SymbolLink symbolLink in _reflector.FindRuleExpansions())
			{
				grammarBuilder.AddLink(symbolLink);
			}

			IList<IProductionRule> consolidated = rules.SelectMany(x => x.Value).ToList();

			IOrderedEnumerable<IProductionRule> sorted = consolidated.OrderBy(x => x.SortOrder);

			string generated = string.Join(Environment.NewLine, sorted.Select(x => x.ToEBNF()));
			return generated;
		}
	}
}
