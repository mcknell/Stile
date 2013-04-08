#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Types.Enumerables;
#endregion

namespace Stile.Prototypes.Compilation.Grammars
{
	public partial class ProductionRule
	{
		public ProductionRule([NotNull] Symbol left, [NotNull] Symbol right, params Symbol[] rights)
			: this(left, rights.Unshift(right).ToList()) {}

		public ProductionRule([NotNull] Symbol left, [NotNull] IList<Symbol> right)
		{
			Left = left.ValidateArgumentIsNotNull();
			Right = right.Validate().EnumerableOf<Symbol>().IsNotNullOrEmpty();
			string[] badElements = Right.Select(x => x.Token).Where(string.IsNullOrWhiteSpace).ToArray();
			if (badElements.Length == Right.Count)
			{
				throw new ArgumentException(
					"Right side must have at least one element that's not null or whitespace, but had "
						+ string.Join(", ", badElements),
					"right");
			}
		}

		public bool CanBeInlined { get; set; }
		[NotNull]
		public Symbol Left { get; private set; }
		[NotNull]
		public IList<Symbol> Right { get; private set; }
		public int SortOrder { get; set; }

		public ProductionRule Inline(Symbol target, IList<Symbol> replacement)
		{
			List<Symbol> newRight = Right.SelectMany(x => x == target ? replacement : new[] {x}).ToList();
			var rule = new ProductionRule(Left, newRight) {CanBeInlined = CanBeInlined, SortOrder = SortOrder};
			return rule;
		}

		public override string ToString()
		{
			return string.Join(" ", new[] {Left, Symbol.EBNFAssignment}.Concat(Right));
		}
	}

	public partial class ProductionRule : IEquatable<ProductionRule>
	{
		public bool Equals(ProductionRule other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}
			return CanBeInlined.Equals(other.CanBeInlined) && string.Equals(Left, other.Left)
				&& Right.SequenceEqual(other.Right);
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
			var other = obj as ProductionRule;
			return other != null && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = Left.GetHashCode();
				hashCode = (hashCode * 397) ^ Right.GetHashCode();
				hashCode = (hashCode * 397) ^ CanBeInlined.GetHashCode();
				return hashCode;
			}
		}

		public static bool operator ==(ProductionRule left, ProductionRule right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(ProductionRule left, ProductionRule right)
		{
			return !Equals(left, right);
		}
	}
}
