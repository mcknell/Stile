#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

namespace Stile.Patterns.SelfDescribingPredicates
{
    public sealed class Negated
    {
        public static readonly Negated True = new Negated(true);
        public static readonly Negated False = new Negated(false);
        private readonly bool _negated;

        private Negated(bool negated)
        {
            _negated = negated;
        }

        public override string ToString()
        {
            return (string) this;
        }

        public static explicit operator Negated(bool negated)
        {
            return negated ? True : False;
        }

        public static explicit operator string(Negated negated)
        {
            return negated ? "not " : string.Empty;
        }

        public static implicit operator bool(Negated negated)
        {
            return negated._negated;
        }
    }
}
