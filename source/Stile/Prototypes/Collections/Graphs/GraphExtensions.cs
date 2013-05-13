#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq;
using Stile.Types.Primitives;
#endregion

namespace Stile.Prototypes.Collections.Graphs
{
	public static class GraphExtensions
	{
		public static string Print<TVertex>(this IDirectedEdge<TVertex> x)
		{
			return "({0},{1})".InvariantFormat(x.From, x.To);
		}

		public static string VerifyAreOn<TVertex>(this IEnumerable<IDirectedEdge<TVertex>> edges,
			HashSet<TVertex> vertices)
		{
			List<IDirectedEdge<TVertex>> badEdges = edges.Where(x => vertices.Contains(x.From) == false //
				|| vertices.Contains(x.To) == false).ToList();
			if (badEdges.Any())
			{
				string message =
					ErrorMessages.GraphExtensions_VerifyAreOn_EdgesNotInGraph.InvariantFormat(Environment.NewLine,
						string.Join(Environment.NewLine, badEdges.Select(Print)));
				return message;
			}
			return null;
		}
	}
}
