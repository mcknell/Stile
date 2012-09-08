#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using System.Linq;
using NUnit.Framework;
using Stile.Types;
#endregion

namespace Stile.Tests.Types.Reflection
{
    [TestFixture]
    public class CSharp4TypesFixture
    {
        [Test]
        public void CSharpTypeAliases()
        {
            CollectionAssert.IsEmpty(CSharp4Types.SimpleTypes.Except(CSharp4Types.TypeAliases.Keys).ToList());
        }

        [Test]
        public void Primitives()
        {
            CollectionAssert.IsEmpty(
                typeof(int).Assembly.GetTypes().Where(x => x.IsPrimitive).Except(CSharp4Types.Primitives).ToList());
        }
    }
}
