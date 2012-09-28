#region License info...
// Propter for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq;
using Stile.Prototypes.Compilation.Grammars.ContextFree;
using Stile.Prototypes.Specifications.Printable.Output.GrammarMetadata;
#endregion

namespace Stile.DocumentationGeneration
{
    public static class ProductionRuleExtensions
    {
        public static readonly Dictionary<string, string> DefaultLexicon = new Dictionary<string, string>
        {
            {Terminal.DescriptionPrefix.ToString(), "'would'"},
            {Terminal.SubjectPrefix.ToString(), "'expected'"},
            {Terminal.Because.ToString(), "'because'"},
            {Variable.SubjectClause.ToString(), "subject-clause"},
            {Variable.ExpectationClause.ToString(), "expectation-clause"},
            {Variable.Specification.ToString(), "specification"},
            {Variable.StartSymbol.ToString(), "output-grammar"},
            {Variable.Explainer.ToString(), "explain"},
            {Variable.Negated.ToString(), "'not'?"},
            {Variable.Negated.ToString(), "'not'?"},
        };

        static ProductionRuleExtensions()
        {
            Lexicon = DefaultLexicon;
        }

        public static Dictionary<string, string> Lexicon { get; set; }

        public static string Substitute(this IEnumerable<string> symbols, Dictionary<string, string> literalLexicon)
        {
            IEnumerable<string> many = symbols.SelectMany(x => x.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries));
            IEnumerable<string> substituted = many.Select(x => x.Substitute(literalLexicon));
            return string.Join(" ", substituted);
        }

        public static string Substitute(this string token, Dictionary<string, string> literalLexicon)
        {
            string value;
            if (literalLexicon.TryGetValue(token, out value))
            {
                return value;
            }
            return token;
        }

        public static string ToEBNF(this ProductionRule rule, Dictionary<string, string> literalLexicon)
        {
            IEnumerable<string> symbols = new[] {rule.Left, "::="}.Concat(rule.Right);
            string substituted = symbols.Substitute(literalLexicon);
            return substituted;
        }
    }
}
