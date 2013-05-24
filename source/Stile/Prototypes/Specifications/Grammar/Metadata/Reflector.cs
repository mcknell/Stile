#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Compilation.Grammars.ContextFree.Builders;
using Stile.Types.Enumerables;
#endregion

namespace Stile.Prototypes.Specifications.Grammar.Metadata
{
	public class Reflector
	{
		private const BindingFlags Everything =
			BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
		private readonly List<Assembly> _assemblies;
		private readonly IExtractor _extractor;
		private readonly IGrammarBuilder _grammarBuilder;

		public Reflector(IGrammarBuilder grammarBuilder)
			: this(grammarBuilder, typeof(Reflector).Assembly) {}

		public Reflector(IGrammarBuilder grammarBuilder, Assembly stile, params Assembly[] others)
		{
			_grammarBuilder = grammarBuilder.ValidateArgumentIsNotNull();
			_assemblies = others.Unshift(stile).ToList() //
				.ValidateArgumentIsNotNullOrEmpty();
			_extractor = new Extractor();
		}

		public void Find()
		{
			foreach (Tuple<MethodBase, RuleAttribute> tuple in GetMethods<RuleAttribute>())
			{
				_grammarBuilder.Add(ProductionBuilder.Make(tuple.Item1, tuple.Item2));
			}
			foreach (Tuple<PropertyInfo, RuleAttribute> tuple in GetProperties<RuleAttribute>())
			{
				_grammarBuilder.Add(ProductionBuilder.Make(tuple.Item1, tuple.Item2));
			}
			foreach (Tuple<MethodBase, RuleFragmentAttribute> tuple in GetMethods<RuleFragmentAttribute>())
			{
				_grammarBuilder.Add(_extractor.Find(tuple.Item1, tuple.Item2));
			}
			foreach (Tuple<MethodBase, RuleCategoryAttribute> tuple in GetMethods<RuleCategoryAttribute>(false))
			{
				_grammarBuilder.Add(_extractor.Find(tuple.Item1, tuple.Item2));
			}
			foreach (Tuple<PropertyInfo, RuleFragmentAttribute> tuple in GetProperties<RuleFragmentAttribute>())
			{
				_grammarBuilder.Add(_extractor.Find(tuple.Item1, tuple.Item2));
			}
		}

		public IEnumerable<Tuple<PropertyInfo, TAttribute>> GetProperties<TAttribute>() where TAttribute : Attribute
		{
			foreach (Type type in Types)
			{
				IEnumerable<PropertyInfo> methodBases = type.GetProperties(Everything) //
					.Where(x => x.ReflectedType == x.DeclaringType);
				foreach (PropertyInfo propertyInfo in methodBases)
				{
					var attribute = propertyInfo.GetCustomAttribute<TAttribute>(false);
					if (attribute != null)
					{
						yield return Tuple.Create(propertyInfo, attribute);
					}
				}
			}
		}

		private IEnumerable<Type> Types
		{
			get { return _assemblies.SelectMany(x => x.GetTypes()); }
		}

		private IEnumerable<Tuple<MethodBase, TAttribute>> GetMethods<TAttribute>(
			bool reflectedTypeIsDeclaringType = true) where TAttribute : Attribute
		{
			foreach (Type type in Types)
			{
				IEnumerable<MethodBase> methodBases = type.GetMethods(Everything).Cast<MethodBase>() //
					.Concat(type.GetConstructors(Everything)) //
					.Where(x => (x.ReflectedType == x.DeclaringType) == reflectedTypeIsDeclaringType);
				foreach (MethodBase methodInfo in methodBases)
				{
					IEnumerable<TAttribute> attributes = methodInfo.GetCustomAttributes<TAttribute>(false);
					foreach (TAttribute attribute in attributes)
					{
						yield return Tuple.Create(methodInfo, attribute);
					}
				}
			}
		}
	}
}
