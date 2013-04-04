#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq;
using Stile.Prototypes.Compilation.Grammars;
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
            {Terminal.Be.ToString(), "'be'"},
            {Terminal.Was.ToString(), "'was'"},
            {Terminal.Have.ToString(), "'have'"},
            {Terminal.Had.ToString(), "'had'"},
            {Terminal.Null.ToString(), "'null'"},
            {Terminal.Contain.ToString(), "'contain'"},
            {Terminal.Contained.ToString(), "'was'"},
            {Terminal.StartWith.ToString(), "'start with'"},
            {Terminal.StartedWith.ToString(), "'started with'"},
            {Variable.Subject.ToString(), "subject-value"},
            {Variable.SubjectClause.ToString(), "subject-clause"},
            {Variable.ExpectationClause.ToString(), "expectation-clause"},
            {Variable.Specification.ToString(), "Specification"},
            {Variable.StartSymbol.ToString(), "grammar"},
            {Variable.ActualValue.ToString(), "actual-value"},
            {Variable.ExpectedValue.ToString(), "expected-value"},
            {Variable.Explainer.ToString(), "explain"},
            {Variable.Negated.ToString(), "'not'?"},
            {Variable.Conjunction.ToString(), "conjunction"},
            {Variable.Reason.ToString(), "reason"},
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
