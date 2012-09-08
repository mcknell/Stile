#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using System;
using NUnit.Framework;
using Stile.Readability;
using Stile.Types.Reflection;
#endregion

namespace Stile.Tests.Types.Reflection
{
    [TestFixture]
    public class TypeExtensionsIsOrDerivesFromFixture
    {
        [Test]
        public void IsOrDerivesFrom()
        {
            // assert
            Assert.That(Null.Type.IsOrDerivesFrom(null), Is.False);
            Assert.That(Null.Type.IsOrDerivesFrom(typeof(object)), Is.False);
            Assert.That(typeof(int).IsOrDerivesFrom(null), Is.False);
            Assert.That(typeof(int).IsOrDerivesFrom(typeof(int)), Is.True);
            Assert.That(typeof(int).IsOrDerivesFrom(typeof(object)), Is.True);
            Assert.That(typeof(object).IsOrDerivesFrom(typeof(object)), Is.True);

            Assert.That(Null.Type.IsOrDerivesFrom<object>(), Is.False);
            Assert.That(typeof(int).IsOrDerivesFrom<int>(), Is.True);
            Assert.That(typeof(int).IsOrDerivesFrom<object>(), Is.True);
            Assert.That(typeof(string).IsOrDerivesFrom<object>(), Is.True);
            Assert.That(typeof(MulticastDelegate).IsOrDerivesFrom<object>(), Is.True);
            Assert.That(typeof(int).IsOrDerivesFrom<ValueType>(), Is.True);
            Assert.That(typeof(string).IsOrDerivesFrom<ValueType>(), Is.False);
            Assert.That(typeof(object).IsOrDerivesFrom<object>(), Is.True);
        }
    }
}
