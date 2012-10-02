﻿#region License info...
// Propter for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
#endregion

namespace Stile.Prototypes.Specifications.Printable.Output.GrammarMetadata
{
    /// <summary>
    /// A symbol on the left of a production rule in the grammar for describing <see cref="IPrintableSpecification"/> objects.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor, AllowMultiple = false, Inherited = false)]
    public class RuleAttribute : Attribute
    {
        private object[] _items;
        private string _symbolToken;

        public RuleAttribute(bool startSymbol = false)
            : this(Variable.UseReflection, startSymbol) {}

        public RuleAttribute(Variable variable, bool startSymbol = false)
        {
            if (startSymbol)
            {
                _symbolToken = "Explanation";
            }
            Symbol = variable;
            Items = new object[0];
        }

        public bool Inline { get; set; }
        [NotNull]
        public object[] Items
        {
            get { return _items; }
            set { _items = value.ValidateArgumentIsNotNull(); }
        }
        public Variable Symbol { get; private set; }
        [CanBeNull]
        public string SymbolToken
        {
            get { return ChooseToken(_symbolToken, Symbol); }
            set { _symbolToken = value; }
        }

        internal static string ChooseToken(string token, Variable variable)
        {
            return token ?? (variable == Variable.UseReflection ? null : variable.ToString());
        }
    }
}