#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using Stile.Prototypes.Specifications.DSL.SemanticModel;
#endregion

namespace Stile.Prototypes.Specifications.Printable.Output
{
    public interface IExpectationVerb
    {
        string Actual { get; }
        string Expected { get; }
        IExpectationVerb Negate(Negated negated);
    }

    public class ExpectationVerb : IExpectationVerb
    {
        public static readonly ExpectationVerb Be = new ExpectationVerb("be", "was", () => NotBe);
        public static readonly ExpectationVerb Contain = new ExpectationVerb("contain", "was", () => NotContain);
        public static readonly ExpectationVerb EndWith = new ExpectationVerb("start with", "was", () => NotEndWith);
        public static readonly ExpectationVerb NotEndWith = new ExpectationVerb("start with", "was");
        public static readonly ExpectationVerb StartWith = new ExpectationVerb("start with", "was", () => NotStartWith);
        public static readonly ExpectationVerb NotStartWith = new ExpectationVerb("start with", "was");
        public static readonly ExpectationVerb NotContain = new ExpectationVerb("not contain", "was");
        public static readonly ExpectationVerb NotBe = new ExpectationVerb("not be", "was");
        public static readonly ExpectationVerb Have = new ExpectationVerb("have", "had", () => NotHave);
        public static readonly ExpectationVerb NotHave = new ExpectationVerb("have", "had");
        private readonly Func<ExpectationVerb> _negate;

        public ExpectationVerb(string expected, string actual, Func<ExpectationVerb> negate = null)
        {
            _negate = negate;
            Expected = expected;
            Actual = actual;
        }

        public string Actual { get; private set; }
        public string Expected { get; private set; }

        public IExpectationVerb Negate(Negated negated)
        {
            if (negated && (_negate != null))
            {
                return _negate.Invoke();
            }
            return this;
        }
    }
}
