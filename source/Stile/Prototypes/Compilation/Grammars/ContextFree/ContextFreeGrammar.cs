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
using Stile.Readability;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.ContextFree
{
    public class ContextFreeGrammar
    {
        public ContextFreeGrammar([NotNull] HashSet<string> variables,
            [NotNull] HashSet<string> terminals,
            [NotNull] List<ProductionRule> productionRules,
            [NotNull] string initialToken)
        {
            Variables = variables.ValidateArgumentIsNotNull();
            Terminals = terminals.ValidateArgumentIsNotNull();
            ProductionRules = productionRules.ValidateArgumentIsNotNull();
            InitialToken = initialToken.ValidateArgumentIsNotNull();

            List<string> overlap = variables.Intersect(terminals).ToList();
            if (overlap.Any())
            {
                int count = overlap.Count;
                throw new ArgumentException(
                    string.Format("Variables and terminals should not overlap but had {0} {1} in common:\r\n{2}",
                        count,
                        count.Pluralize("item"),
                        string.Join(", ", overlap)));
            }
        }

        public string InitialToken { get; private set; }
        public List<ProductionRule> ProductionRules { get; private set; }
        public HashSet<string> Terminals { get; private set; }
        public HashSet<string> Variables { get; private set; }
    }
}
