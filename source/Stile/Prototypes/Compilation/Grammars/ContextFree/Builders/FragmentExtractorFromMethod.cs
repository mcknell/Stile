#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Reflection;
using Stile.Prototypes.Specifications.Grammar;
using Stile.Prototypes.Specifications.Grammar.Metadata;
using Stile.Types.Reflection;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.ContextFree.Builders
{
	public class FragmentExtractorFromMethod :
		ExtractorFromMethod<RuleFragmentAttribute, IReadOnlyList<IFragment>>
	{
		public FragmentExtractorFromMethod(MethodBase methodBase, RuleFragmentAttribute attribute)
			: base(methodBase, attribute) {}

		protected override IEnumerable<Tuple<ParameterInfo, SymbolAttribute>> GetParameters(out string firstAlias,
			out string firstToken)
		{
			firstAlias = Attribute.Alias;
			firstToken = ProductionBuilder.GetSymbol(MethodBase, Attribute.Token);
			return MethodBase.GetParametersWith<SymbolAttribute>();
		}

		protected override IReadOnlyList<IFragment> MakeOutput(Nonterminal first, List<IFragment> fragments)
		{
			Cardinality cardinality = Attribute.Optional ? Cardinality.ZeroOrOne : Cardinality.One;
			var fragment = new Fragment(Attribute.Prior, first, cardinality);
			fragments.Insert(0, fragment);
			return fragments;
		}
	}
}
