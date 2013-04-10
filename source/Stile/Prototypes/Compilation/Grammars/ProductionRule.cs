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
	public interface IProductionRule : IEquatable<ProductionRule>
	{
		bool CanBeInlined { get; set; }
		[NotNull]
		Symbol Left { get; }
		[NotNull]
		IClause Right { get; }
		int SortOrder { get; set; }
	}

	public partial class ProductionRule : IProductionRule
	{
		public ProductionRule([NotNull] Symbol left, [NotNull] Symbol right, params Symbol[] rights)
			: this(left, rights.Unshift(right).ToList()) {}

		public ProductionRule([NotNull] Symbol left, [NotNull] IEnumerable<Symbol> right)
			: this(left, new Clause(right)) {}

		public ProductionRule([NotNull] Symbol left, [NotNull] IClause right)
		{
			Left = left.ValidateArgumentIsNotNull();
			Right = right.ValidateArgumentIsNotNull();
		}

		public bool CanBeInlined { get; set; }
		public Symbol Left { get; private set; }
		public IClause Right { get; private set; }
		public int SortOrder { get; set; }

		public override string ToString()
		{
			return string.Join(" ", new[] {Left, TerminalSymbol.EBNFAssignment, Right.ToString()});
		}
	}

	public partial class ProductionRule
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
			return CanBeInlined.Equals(other.CanBeInlined) && Left.Equals(other.Left) && Right.Equals(other.Right);
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
