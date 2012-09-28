#region License info...
// Propter for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

namespace Stile.Prototypes.Specifications.Printable.Output
{
    public interface IExpectationVerb
    {
        string Actual { get; }
        string Expected { get; }
    }

    public class ExpectationVerb : IExpectationVerb
    {
        public static readonly ExpectationVerb Be = new ExpectationVerb("be", "was");
        public static readonly ExpectationVerb NotBe = new ExpectationVerb("not be", "was");
        public static readonly ExpectationVerb Has = new ExpectationVerb("has", "had");
        public static readonly ExpectationVerb Have = new ExpectationVerb("have", "had");

        public ExpectationVerb(string expected, string actual)
        {
            Expected = expected;
            Actual = actual;
        }

        public string Actual { get; private set; }
        public string Expected { get; private set; }
    }
}
