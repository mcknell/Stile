﻿#region License info...
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
using Stile.Types.Equality;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.ContextFree.Builders
{
	public interface IProductionBuilder : IEquatable<IProductionBuilder>
	{
		bool CanBeInlined { get; }
		IReadOnlyList<IFragment> Fragments { get; }
		Nonterminal Left { get; }
		[CanBeNull]
		IChoice Right { get; }
		int SortOrder { get; }

		IProduction Assemble(HashSet<IFragment> fragments);
	}

	public partial class ProductionBuilder : IProductionBuilder
	{
		public ProductionBuilder(Nonterminal left,
			IChoice right,
			RuleAttribute attribute,
			IReadOnlyList<IFragment> fragments = null)
			: this(left, right, fragments, attribute.StartsGrammar ? -1 : 0, attribute.CanBeInlined) {}

		public ProductionBuilder(Nonterminal left,
			IChoice right,
			IReadOnlyList<IFragment> fragments = null,
			int sortOrder = 0,
			bool canBeInlined = false)
		{
			Left = left.ValidateArgumentIsNotNull();
			CanBeInlined = canBeInlined;
			Fragments = fragments ?? Default.Fragments;
			Right = right;
			SortOrder = sortOrder;
		}

		public bool CanBeInlined { get; private set; }
		public IReadOnlyList<IFragment> Fragments { get; private set; }
		public Nonterminal Left { get; private set; }
		public IChoice Right { get; private set; }
		public int SortOrder { get; set; }
		public IProduction Assemble(HashSet<IFragment> fragments)
		{
			var accumulator = new ProductionAccumulator(fragments, Left, Right);
			return accumulator.Build();
		}

		public static IProductionBuilder Make(MemberInfo memberInfo, RuleAttribute attribute)
		{
			var propertyInfo = memberInfo as PropertyInfo;
			if (propertyInfo != null)
			{
				return Make(propertyInfo, attribute);
			}
			var methodBase = memberInfo as MethodBase;
			if (methodBase != null)
			{
				return new ProductionExtractorFromMethod(methodBase, attribute).Build();
			}
			throw new NotImplementedException();
		}

		public static IProductionBuilder Make(PropertyInfo propertyInfo, RuleAttribute attribute)
		{
			var left = new Nonterminal(attribute.Left);
			var symbol = new Nonterminal(propertyInfo.Name, attribute.Alias);
			IChoice right = new Choice(new Sequence(new Item(symbol)));
			var builder = new ProductionBuilder(left, right, canBeInlined : attribute.CanBeInlined);
			if (attribute.StartsGrammar)
			{
				builder.SortOrder = -1;
			}
			return builder;
		}

		public static class Default
		{
			public static readonly IFragment[] Fragments = new IFragment[0];
		}
	}

	public partial class ProductionBuilder
	{
		public bool Equals(IProductionBuilder other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}
			return CanBeInlined.Equals(other.CanBeInlined) && Equals(Left, other.Left) && Equals(Right, other.Right)
				&& SortOrder == other.SortOrder && Fragments.SequenceEqual(other.Fragments);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			if (ReferenceEquals(this, obj))
			{
				return true;
			}
			var other = obj as IProductionBuilder;
			return other != null && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = CanBeInlined.GetHashCode();
				hashCode = (hashCode * 397) ^ Left.GetHashCode();
				hashCode = (hashCode * 397) ^ (Right != null ? Right.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ SortOrder;
				hashCode = (hashCode * 397) ^ Fragments.Aggregate(0, EqualityExtensions.HashForAccumulation);
				return hashCode;
			}
		}

		public static bool operator ==(ProductionBuilder left, IProductionBuilder right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(ProductionBuilder left, IProductionBuilder right)
		{
			return !Equals(left, right);
		}
	}
}
