#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
#endregion

namespace Stile.Prototypes.Collections.Graphs
{
	public interface IDigraph<TValue>
	{
		[NotNull]
		IReadOnlyCollection<IDirectedEdge<TValue>> Edges { get; }
		[NotNull]
		IReadOnlyCollection<IVertex<TValue>> Vertices { get; }
	}

	public class Digraph<TValue> : IDigraph<TValue>
	{
		private readonly HashSet<DirectedEdge<TValue>> _edges;
		private readonly HashSet<IVertex<TValue>> _vertices;

		public Digraph([NotNull] IList<TValue> vertices,
			IEnumerable<Tuple<TValue, TValue>> edges,
			IEqualityComparer<TValue> vertexComparer = null)
		{
			Dictionary<TValue, int> inDegrees = vertices.ValidateArgumentIsNotNull()
				.ToDictionary(x => x, x => 0, vertexComparer);
			Dictionary<TValue, int> outDegrees = vertices.ToDictionary(x => x, x => 0, vertexComparer);
			var badTuples = new HashSet<Tuple<TValue, TValue>>();
			foreach (Tuple<TValue, TValue> tuple in edges)
			{
				if (inDegrees.ContainsKey(tuple.Item1))
				{
					inDegrees[tuple.Item1] += 1;
				}
				else
				{
					badTuples.Add(tuple);
				}
				if (outDegrees.ContainsKey(tuple.Item2))
				{
					outDegrees[tuple.Item2] += 1;
				}
				else
				{
					badTuples.Add(tuple);
				}
			}
			_vertices = new HashSet<IVertex<TValue>>();
			for (int i = 0; i < vertices.Count; i++) {}

			Vertices = _vertices.ToArray();
			_edges = new HashSet<DirectedEdge<TValue>>(edges.Select(DirectedEdge.Create),
				DirectedEdge<TValue>.FromToComparer);
			Edges = _edges.ToArray();

			//string message = Edges.VerifyAreOn(_vertices);
			//if (message != null)
			//{
			//	throw new ArgumentException(message);
			//}
		}

		public IReadOnlyCollection<IDirectedEdge<TValue>> Edges { get; private set; }

		public IReadOnlyCollection<IVertex<TValue>> Vertices { get; private set; }
	}
}
