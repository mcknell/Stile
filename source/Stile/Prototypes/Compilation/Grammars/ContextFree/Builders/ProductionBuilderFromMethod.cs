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
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.Grammar;
using Stile.Prototypes.Specifications.Grammar.Metadata;
using Stile.Types.Reflection;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.ContextFree.Builders
{
	public class ProductionBuilderFromMethod
	{
		private readonly List<string> _aliases = new List<string>();
		private readonly RuleAttribute _attribute;
		private readonly List<ParameterMetadata> _metadatas;
		private readonly MethodBase _methodBase;
		private string _alias;
		private Nonterminal _first;
		private Tuple<ParameterInfo, SymbolAttribute> _priorTuple;

		public ProductionBuilderFromMethod(MethodBase methodBase, RuleAttribute attribute)
		{
			_methodBase = methodBase.ValidateArgumentIsNotNull();
			_attribute = attribute.ValidateArgumentIsNotNull();
			_metadatas = new List<ParameterMetadata>();
		}

		public ProductionBuilder Build()
		{
			string firstAlias;
			string firstToken;
			IEnumerable<Tuple<ParameterInfo, SymbolAttribute>> parameterSymbols = GetParameters(out firstAlias,
				out firstToken);
			_aliases.Add(firstAlias ?? firstToken);

			foreach (Tuple<ParameterInfo, SymbolAttribute> tuple in parameterSymbols)
			{
				SymbolAttribute symbolAttribute = tuple.Item2;
				if (symbolAttribute.Terminal)
				{
					_aliases.Add(MakeAlias(tuple));
				}
				else
				{
					_alias = _aliases.Any() ? String.Join(" ", _aliases) : null;
					Process(firstToken);
					// setup future loops
					_aliases.Clear();
					_aliases.Add(symbolAttribute.Alias ?? symbolAttribute.Token);
					_priorTuple = tuple;
				}
			}

			if (_aliases.Any()) // if any terminals occurred after the last nonterminal, process once more
			{
				_alias = String.Join(" ", _aliases);
				Process(firstToken);
			}
			return Build(_methodBase, _attribute, _first, _metadatas);
		}

		public static ProductionBuilder Build(MethodBase methodBase,
			RuleAttribute attribute,
			Nonterminal first,
			IEnumerable<ParameterMetadata> parametersToConsider)
		{
			Nonterminal left = ProductionBuilder.GetNonterminal(methodBase, attribute.Symbol, attribute.Alias);
			Nonterminal prior = first;
			var fragments = new List<IFragment>();
			foreach (ParameterMetadata tuple in parametersToConsider)
			{
				Nonterminal latest = ProductionBuilder.GetNonterminal(tuple.ParameterInfo, tuple.Token, tuple.Alias);
				var fragment = new Fragment(prior.Token, latest.Token, tuple.Alias);
				fragments.Add(fragment);
				// clean up loop
				prior = latest;
			}
			var right = new Choice(new Sequence(new Item(first)));
			var builder = new ProductionBuilder(left, right, attribute, fragments);
			return builder;
		}

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

		public static string MakeAlias(Tuple<ParameterInfo, SymbolAttribute> tuple)
		{
			ParameterInfo parameterInfo = tuple.Item1;
			SymbolAttribute attribute = tuple.Item2;
			Cardinality cardinality = GetCardinality(parameterInfo);
			string symbol = ProductionBuilder.GetSymbol(parameterInfo, attribute.Token);
			string alias = attribute.Alias ?? symbol;
			var terminal = new StringLiteral(alias);
			Tuple<StringLiteral, Cardinality> terminalTuple1 = Tuple.Create(terminal, cardinality);
			Tuple<StringLiteral, Cardinality> terminalTuple = terminalTuple1;
			string s = terminalTuple.Item1.Alias.Trim() + terminalTuple.Item2.ToEbnfString();
			return s;
		}

		private IEnumerable<Tuple<ParameterInfo, SymbolAttribute>> GetParameters(out string firstAlias,
			out string firstToken)
		{
			List<Tuple<ParameterInfo, SymbolAttribute>> parameters = GetParameterSymbols(_methodBase).ToList();

			if (_attribute.NameIsSymbol)
			{
				firstAlias = _attribute.Alias;
				firstToken = _methodBase.Name;
			}
			else
			{
				Tuple<ParameterInfo, SymbolAttribute> tuple = parameters.First();
				parameters.RemoveAt(0);
				firstAlias = tuple.Item2.Alias;
				firstToken = firstAlias ?? tuple.Item2.Token ?? tuple.Item1.Name;
				if (tuple.Item2.Terminal == false)
				{
					_priorTuple = tuple;
					_first = new Nonterminal(firstToken, firstAlias);
				}
			}
			return parameters;
		}

		private void Process(string firstToken)
		{
			if (_priorTuple == null)
			{
				_first = new Nonterminal(firstToken, _alias);
			}
			else
			{
				var metadata = new ParameterMetadata(_priorTuple.Item1, _priorTuple.Item2.Token, _alias);
				_metadatas.Add(metadata);
			}
		}

		public class ParameterMetadata
		{
			public ParameterMetadata(ParameterInfo parameterInfo, [CanBeNull] string token, [CanBeNull] string alias)
			{
				ParameterInfo = parameterInfo.ValidateArgumentIsNotNull();
				Token = token;
				Alias = alias;
			}

			public string Alias { get; private set; }
			public ParameterInfo ParameterInfo { get; private set; }
			public string Token { get; private set; }
		}
	}
}
