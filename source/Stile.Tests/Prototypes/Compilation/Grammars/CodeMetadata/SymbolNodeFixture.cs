#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using NUnit.Framework;
using Stile.Prototypes.Compilation.Grammars.CodeMetadata;
using Stile.Tests.Prototypes.Specifications.SampleObjects;
#endregion

namespace Stile.Tests.Prototypes.Compilation.Grammars.CodeMetadata
{
	[TestFixture]
	public class SymbolNodeFixture
	{
		[Test]
		public void Children()
		{
			var node = new SymbolNode("foo", typeof(Foo<>));
			Assert.That(node.Symbol, Is.EqualTo("foo"));
			Assert.That(node.Children, Is.Not.Null);
		}
	}
}
