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
using Stile.Prototypes.Specifications.Grammar;
using Stile.Prototypes.Specifications.Grammar.Metadata;
using Stile.Types.Reflection;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.ContextFree.Builders
{
	public class ProductionExtractorFromMethod : ExtractorFromMethod<RuleAttribute, IProductionBuilder>
	{
		public ProductionExtractorFromMethod(MethodBase methodBase, RuleAttribute attribute)
			: base(methodBase, attribute) {}

		public static Cardinality GetCardinality(ParameterInfo parameterInfo)
		{
			if (parameterInfo.IsOptional || parameterInfo.GetCustomAttribute<CanBeNullAttribute>() != null)
			{
				return Cardinality.ZeroOrOne;
			}
			return Cardinality.One;
		}

		public static IEnumerable<Tuple<ParameterInfo, SymbolAttribute>> GetParameterSymbols(MethodBase methodBase)
		{
			return methodBase.GetParametersWith<SymbolAttribute>();
		}

		protected override IEnumerable<Tuple<ParameterInfo, SymbolAttribute>> GetParameters(out string firstAlias,
			out string firstToken)
		{
			List<Tuple<ParameterInfo, SymbolAttribute>> parameters = GetParameterSymbols(MethodBase).ToList();

			if (Attribute.NameIsSymbol)
			{
				firstAlias = Attribute.Alias;
				firstToken = GetSymbol(MethodBase, null);
			}
			else
			{
				Tuple<ParameterInfo, SymbolAttribute> tuple = parameters.First();
				firstAlias = tuple.Item2.Alias;
				firstToken = firstAlias ?? tuple.Item2.Token ?? tuple.Item1.Name;
				if (tuple.Item2.Terminal == false)
				{
					parameters.RemoveAt(0);
				}
			}
			return parameters;
		}

		protected override IProductionBuilder MakeOutput(Nonterminal first, List<IFragment> fragments)
		{
			var right = new Choice(new Sequence(new Item(first)));
			Nonterminal left = GetNonterminal(MethodBase, Attribute.Left, Attribute.Alias);
			var builder = new ProductionBuilder(left, right, Attribute, fragments);
			return builder;
		}
	}
}
