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
using Stile.Readability;
using Stile.Types.Primitives;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.ContextFree.Builders
{
	public abstract class ExtractorFromMethod : Extractor
	{
		protected static string MakeAlias(Tuple<ParameterInfo, SymbolAttribute> tuple)
		{
			ParameterInfo parameterInfo = tuple.Item1;
			SymbolAttribute attribute = tuple.Item2;
			Cardinality cardinality = ProductionExtractorFromMethod.GetCardinality(parameterInfo);
			string token = GetToken(parameterInfo, attribute.Token);
			Symbol symbol = attribute.Terminal
				? (Symbol) new StringLiteral(token, attribute.Alias)
				: new Nonterminal(token, attribute.Alias);
			string s = symbol.Alias ?? symbol.Token;
			s += cardinality.ToEbnfString();
			return s;
		}

		protected class ParameterMetadata
		{
			public ParameterMetadata(ParameterInfo parameterInfo, [CanBeNull] string token, [CanBeNull] string alias)
			{
				ParameterInfo = parameterInfo.ValidateArgumentIsNotNull();
				Token = token ?? ParameterInfo.Name;
				Alias = alias ?? token;
			}

			public string Alias { get; private set; }
			public ParameterInfo ParameterInfo { get; private set; }
			public string Token { get; private set; }

			public override string ToString()
			{
				return "{0} {1} {2}".InvariantFormat(Token, Alias.ToDebugString(), ParameterInfo.Name);
			}
		}
	}

	public abstract class ExtractorFromMethod<TAttribute, TOutput> : ExtractorFromMethod
		where TAttribute : Attribute
		where TOutput : class
	{
		private readonly List<string> _aliases;
		private readonly List<ParameterMetadata> _metadatas;
		private string _alias;
		private Nonterminal _first;
		private Tuple<ParameterInfo, SymbolAttribute> _priorTuple;

		protected ExtractorFromMethod(MethodBase methodBase, TAttribute attribute)
		{
			MethodBase = methodBase.ValidateArgumentIsNotNull();
			Attribute = attribute.ValidateArgumentIsNotNull();
			_aliases = new List<string>();
			_metadatas = new List<ParameterMetadata>();
		}

		public TOutput Build()
		{
			string firstAlias;
			string firstToken;
			IEnumerable<Tuple<ParameterInfo, SymbolAttribute>> parameterSymbols = GetParameters(out firstAlias,
				out firstToken);
			_aliases.Add(firstAlias ?? Symbol.ToTitleCase(firstToken));

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
					// setup next loop
					_aliases.Clear();
					_aliases.Add(MakeAlias(tuple));
					_priorTuple = tuple;
				}
			}

			if (_aliases.Any()) // if any terminals occurred after the last nonterminal, process once more
			{
				_alias = String.Join(" ", _aliases);
				Process(firstToken);
			}
			return Build(_first, _metadatas);
		}

		protected TAttribute Attribute { get; set; }
		protected MethodBase MethodBase { get; private set; }

		protected TOutput Build(Nonterminal first, IEnumerable<ParameterMetadata> parametersToConsider)
		{
			Nonterminal prior = first;
			var fragments = new List<IFragment>();
			foreach (ParameterMetadata metadata in parametersToConsider)
			{
				Nonterminal latest = GetNonterminal(metadata.ParameterInfo, metadata.Token, metadata.Alias);
				var fragment = new Fragment(prior, latest);
				fragments.Add(fragment);
				// clean up loop
				prior = latest;
			}
			TOutput builder = MakeOutput(first, fragments);
			return builder;
		}

		protected abstract IEnumerable<Tuple<ParameterInfo, SymbolAttribute>> GetParameters(out string firstAlias,
			out string firstToken);

		protected abstract TOutput MakeOutput(Nonterminal first, List<IFragment> fragments);

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
	}
}
