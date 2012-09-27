#region License info...
// Propter for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Propter project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Types.Enumerables;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.ContextFree
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
                throw new ArgumentException("Right side must have at least one element that's not null or whitespace, but had " + string.Join(", ", badElements),
                    "right");
            }
        }

        public string Left { get; private set; }
        public IList<string> Right { get; private set; }
    }
}
