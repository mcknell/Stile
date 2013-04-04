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
using Stile.Types.Enumerables;
#endregion

namespace Stile.Prototypes.Compilation.Grammars
{
	public class ProductionRule
	{
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
		public string Left { get; private set; }
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
}
