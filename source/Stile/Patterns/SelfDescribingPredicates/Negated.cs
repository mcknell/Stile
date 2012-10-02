#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
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

        public bool AgreesWith(bool value)
        {
            return value != _negated;
        }

        public Negated Invert()
        {
            return (Negated) (this == false);
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
