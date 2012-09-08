#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using System;
using NUnit.Framework;
using Stile.Types.Equality;
#endregion

namespace Stile.Tests.Types.Equality
{
    [TestFixture]
    public class EqualityExtensionsFixture
    {
        [Test]
        public void EqualsOrIsEquallyNull()
        {
            EqualsOrIsEquallyNull_ForValueType(1, 2);
            EqualsOrIsEquallyNull_ForValueType(DateTime.MinValue.AddDays(1), DateTime.MaxValue);
            EqualsOrIsEquallyNull_ForValueType(decimal.MinValue, decimal.MaxValue);
            EqualsOrIsEquallyNull_ForReferenceType(string.Empty, "foo");
        }

        private static void EqualsOrIsEquallyNull<TValue>(TValue first, TValue second)
        {
            Assert.AreNotEqual(first, second, "Precondition");
            TValue nil = default(TValue);
            Assert.AreNotEqual(first, nil, "Precondition");
            Assert.AreNotEqual(second, nil, "Precondition");
            Assert.IsTrue(first.EqualsOrIsEquallyNull(first));
            Assert.IsTrue(nil.EqualsOrIsEquallyNull(nil));
            EqualsOrIsEquallyNull_FailsSymmetrically(first, second);
            EqualsOrIsEquallyNull_FailsSymmetrically(first, nil);
            EqualsOrIsEquallyNull_FailsSymmetrically(nil, second);
        }

        private static void EqualsOrIsEquallyNull_FailsSymmetrically<TValue>(TValue first, TValue second)
        {
            Assert.IsFalse(first.EqualsOrIsEquallyNull(second));
            Assert.IsFalse(second.EqualsOrIsEquallyNull(first));
        }

        private static void EqualsOrIsEquallyNull_ForReferenceType<TValue>(TValue first, TValue second) where TValue : class
        {
            EqualsOrIsEquallyNull(first, second);
        }

        private static void EqualsOrIsEquallyNull_ForValueType<TValue>(TValue first, TValue second) where TValue : struct
        {
            EqualsOrIsEquallyNull(first, second);
            EqualsOrIsEquallyNull<TValue?>(first, second);
        }
    }
}
