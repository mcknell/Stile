#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
#endregion

namespace Stile.Prototypes.Collections.Graphs
{
	public interface IVertex<out TValue>
	{
		int InDegree { get; }
		int OutDegree { get; }
		TValue Value { get; }
		int Index { get; }
	}

	public class Vertex<TValue> : IVertex<TValue>
	{
		private readonly Lazy<int> _lazyIn;
		private readonly Lazy<int> _lazyOut;

		public Vertex(int index, TValue value, Func<int> indegreeFunc, Func<int> outdegreeFunc)
		{
			Index = index;
			Value = value;
			_lazyIn = new Lazy<int>(indegreeFunc);
			_lazyOut = new Lazy<int>(outdegreeFunc);
		}

		public int InDegree
		{
			get
			{
				int value = _lazyIn.Value;
				return value;
			}
		}
		public int OutDegree
		{
			get
			{
				int value = _lazyOut.Value;
				return value;
			}
		}
		public TValue Value { get; private set; }
		public int Index { get; private set; }
	}
}
