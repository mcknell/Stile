#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using NUnit.Framework;
using Stile.Prototypes.Collections.Graphs;
#endregion

namespace Stile.Tests.Prototypes.Collections.Graphs
{
	[TestFixture]
	public class GraphExtensionsFixture
	{
		[Test]
		public void EnsureEdgesAreOnVertices()
		{
			var vertices = new HashSet<int> {2, 3, 5, 7};
			Assert.Null(new[] {DirectedEdge.Create(2, 3)}.VerifyAreOn(vertices));
			AssertFindsError("(3,4)", vertices, DirectedEdge.Create(3, 4), DirectedEdge.Create(2, 3)); // first
			AssertFindsError("(5,6)", vertices, DirectedEdge.Create(5, 6), DirectedEdge.Create(2, 3)); // second
			AssertFindsError("(8,9)", vertices, DirectedEdge.Create(8, 9), DirectedEdge.Create(2, 3)); // both
		}

		private static void AssertFindsError(string expected,
			HashSet<int> vertices,
			params IDirectedEdge<int>[] edges)
		{
			StringAssert.Contains(expected, edges.VerifyAreOn(vertices));
		}
	}
}
