#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using System;
using System.Collections.Generic;
using NUnit.Framework;
using Stile.Readability;
using Stile.Types.Reflection;
#endregion

namespace Stile.Tests.Types.Reflection
{
    [TestFixture]
    public class TypeStringBuilderFixture
    {
        [Test]
        public void Array()
        {
            Assert.That(Print<int[]>(), Is.EqualTo("int[]"));
            Assert.That(Print<Int32[]>(), Is.EqualTo("int[]"));
        }

        [Test]
        public void Generic()
        {
            Assert.That(Print<List<int>>(), Is.EqualTo("List<int>"));
            Assert.That(Print<List<int?>>(), Is.EqualTo("List<int?>"));
            Assert.That(Print(typeof(List<>)), Is.EqualTo("List<>"));
            Assert.That(Print<Dictionary<string, int?>>(), Is.EqualTo("Dictionary<string, int?>"));
            Assert.That(Print<Dictionary<DateTime, IList<decimal>>>(), Is.EqualTo("Dictionary<DateTime, IList<decimal>>"));
        }

        [Test]
        public void GenericArgumentDelimiter()
        {
            Assert.That(typeof(List<>).Name.Contains(TypeStringBuilder.GenericArgumentDelimiter));
        }

        [Test]
        public void Int16()
        {
            Assert.That(Print<short>(), Is.EqualTo("short"));
            Assert.That(Print<Int16>(), Is.EqualTo("short"));
        }

        [Test]
        public void Int32()
        {
            Assert.That(Print<int>(), Is.EqualTo("int"));
            Assert.That(Print<Int32>(), Is.EqualTo("int"));
        }

        [Test]
        public void Int64()
        {
            Assert.That(Print<long>(), Is.EqualTo("long"));
            Assert.That(Print<Int64>(), Is.EqualTo("long"));
        }

        [Test]
        public void MultidimensionalArray()
        {
            Assert.That(Print<string[,]>(), Is.EqualTo("string[,]"));
            Assert.That(Print<string[,]>(), Is.EqualTo("string[,]"));
        }

        [Test]
        public void Null()
        {
            Assert.That(Print(null), Is.EqualTo(PrintExtensions.ReadableNullString));
        }

        [Test]
        public void Nullable()
        {
            Assert.That(Print<int?>(), Is.EqualTo("int?"));
            Assert.That(Print<Int32?>(), Is.EqualTo("int?"));
        }

        [Test]
        public void UserType()
        {
            Assert.That(Print<TypeStringBuilderFixture>(), Is.EqualTo(GetType().Name));
        }

        private static string Print<TType>()
        {
            return Print(typeof(TType));
        }

        private static string Print(Type type)
        {
            return new TypeStringBuilder(type).ToString();
        }
    }
}
