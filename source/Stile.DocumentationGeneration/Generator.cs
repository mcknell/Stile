#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using Stile.Prototypes.Compilation.Grammars;
using Stile.Prototypes.Compilation.Grammars.ContextFree;
using Stile.Prototypes.Specifications.Grammar.Metadata;
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
			IEnumerable<IProductionRule> rules = _reflector.FindRules();

			var grammarBuilder = new ContextFreeGrammarBuilder(rules);

			foreach (Follower symbolLink in _reflector.FindRuleExpansions())
			{
				grammarBuilder.Add(symbolLink);
			}

			return grammarBuilder.ToEBNF();
		}
	}
}
