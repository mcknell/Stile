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
	public class FragmentExtractorFromCategory :
		ExtractorFromMethod<RuleCategoryAttribute, IReadOnlyList<IFragment>>
	{

		public FragmentExtractorFromCategory(MethodBase methodBase, RuleCategoryAttribute attribute)
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
			var left = Attribute.Prior ?? ProductionBuilder.GetName(MethodBase.ReflectedType);
			var fragment = new Fragment(left, first);
			fragments.Insert(0, fragment);
			return fragments;
		}
	}
}
