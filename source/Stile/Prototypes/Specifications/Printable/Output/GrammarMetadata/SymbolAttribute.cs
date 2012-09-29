#region License info...
// Propter for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
#endregion

namespace Stile.Prototypes.Specifications.Printable.Output.GrammarMetadata
{
    /// <summary>
    /// A symbol on the right of a production rule in the grammar for describing <see cref="ILazyReadableText"/> objects.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public class SymbolAttribute : Attribute
    {
        private string _prefixToken;
        private string _suffixToken;
        private string _symbolToken;

        public SymbolAttribute()
        {
            Symbol = Variable.UseReflection;
        }

        public SymbolAttribute(Variable variable)
        {
            Symbol = variable;
        }

        public Terminal Prefix { get; set; }
        [CanBeNull]
        public string PrefixToken
        {
            get { return ChooseToken(_prefixToken, Prefix); }
            set { _prefixToken = value; }
        }
        public Terminal Suffix { get; set; }
        [CanBeNull]
        public string SuffixToken
        {
            get { return ChooseToken(_suffixToken, Suffix); }
            set { _suffixToken = value; }
        }
        public Variable Symbol { get; private set; }
        [CanBeNull]
        public string SymbolToken
        {
            get { return RuleAttribute.ChooseToken(_symbolToken, Symbol); }
            set { _symbolToken = value; }
        }

        private static string ChooseToken(string token, Terminal variable)
        {
            return token ?? (variable == Terminal.None ? null : variable.ToString());
        }
    }
}
