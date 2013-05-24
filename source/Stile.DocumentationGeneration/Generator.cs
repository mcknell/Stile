#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Reflection;
using JetBrains.Annotations;
using Stile.Prototypes.Compilation.Grammars.ContextFree;
using Stile.Prototypes.Compilation.Grammars.ContextFree.Builders;
using Stile.Prototypes.Specifications.Grammar.Metadata;
#endregion

namespace Stile.DocumentationGeneration
{
	public class Generator
	{
		private readonly Assembly[] _others;
		private readonly Assembly _stile;

		public Generator(params Assembly[] others)
			: this(typeof(VersionedLanguage).Assembly, others) {}

		public Generator([NotNull] Assembly stile, params Assembly[] others)
		{
			_stile = stile;
			_others = others;
		}

		[NotNull]
		public string Generate()
		{
			var grammarBuilder = new GrammarBuilder();
			var reflector = new Reflector(grammarBuilder, _stile, _others);
			//reflector.FindRules();
			//reflector.FindRuleExpansions();
			reflector.Find();

			IGrammar grammar = grammarBuilder.Build();
			return GrammarDescriber.Describe(grammar);
		}
	}
}
