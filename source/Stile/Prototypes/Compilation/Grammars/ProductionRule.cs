#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.Grammar;
using Stile.Types.Enumerables;
#endregion

namespace Stile.Prototypes.Compilation.Grammars
{
	public partial class ProductionRule
	{
		public ProductionRule(Nonterminal left, Nonterminal right, params Nonterminal[] rights)
			: this(left.ToString(), rights.Select(x => x.ToString()).Unshift(right.ToString()).ToList()) {}

		public ProductionRule([NotNull] string left, [NotNull] string right, params string[] rights)
			: this(left, rights.Unshift(right).ToList()) {}

		public ProductionRule([NotNull] string left, [NotNull] IList<string> right)
		{
			Left = left.ValidateArgumentIsNotNull();
			Right = right.Validate().EnumerableOf<string>().IsNotNullOrEmpty();
			string[] badElements = Right.Where(string.IsNullOrWhiteSpace).ToArray();
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
		public string Left { get; private set; }
		[NotNull]
		public IList<string> Right { get; private set; }
		public int SortOrder { get; set; }

		public ProductionRule RewriteRightSideWith(string pattern, string replacement)
		{
			string wrappedReplacement = string.Format("( {0} )", replacement);
			List<string> newRight = Right.Select(x => Regex.Replace(x, pattern, wrappedReplacement)).ToList();
			var rule = new ProductionRule(Left, newRight) {CanBeInlined = CanBeInlined, SortOrder = SortOrder};
			return rule;
		}

		public override string ToString()
		{
			return string.Join(" ", new[] {Left, "::="}.Concat(Right));
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
