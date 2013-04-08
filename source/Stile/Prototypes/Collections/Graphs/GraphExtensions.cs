#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace Stile.Prototypes.Collections.Graphs
{
	public static class GraphExtensions
	{
		public static string VerifyAreOn<TVertex>(this IEnumerable<IDirectedEdge<TVertex>> edges,
			HashSet<TVertex> vertices)
		{
			List<IDirectedEdge<TVertex>> badEdges = edges.Where(x => vertices.Contains(x.From) == false //
				|| vertices.Contains(x.To) == false).ToList();
			if (badEdges.Any())
			{
				string message = string.Format("These edges had vertices that were not in the graph:{0}{1}",
					Environment.NewLine,
					string.Join(Environment.NewLine, badEdges.Select(x => string.Format("({0},{1})", x.From, x.To))));
				return message;
			}
			return null;
		}
	}
}
