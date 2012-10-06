#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using NUnit.Framework;
using Stile.Prototypes.Specifications.Printable.Construction;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.ResultHas;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SpecificationBuilders;
#endregion

namespace Stile.Tests.Prototypes.Specifications.Printable
{
    [TestFixture]
    public class SpecifyFixture
    {
        [Test]
        public void ThatAny()
        {
            IFluentSpecificationBuilder<string, string> builder = Specify.ThatAny<string>();
            IPrintableHas<string, string> has = builder.Has;
            
        }
    }
}
