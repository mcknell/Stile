#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
#endregion

namespace Stile.Prototypes.Collections.Graphs
{
	public interface IDirectedEdge<TVertex> : IEquatable<DirectedEdge<TVertex>>
	{
		TVertex From { get; }
		TVertex To { get; }
	}

	public abstract class DirectedEdge
	{
		public static DirectedEdge<TVertex> Create<TVertex>(Tuple<TVertex, TVertex> tuple)
		{
			return new DirectedEdge<TVertex>(tuple.Item1, tuple.Item2);
		}

		public static DirectedEdge<TVertex> Create<TVertex>(TVertex from, TVertex to)
		{
			return new DirectedEdge<TVertex>(from, to);
		}
	}

	public partial class DirectedEdge<TVertex> : DirectedEdge
	{
		public DirectedEdge(TVertex from, TVertex to)
		{
			From = @from;
			To = to;
		}

		public TVertex From { get; private set; }
		public TVertex To { get; private set; }
	}

	public partial class DirectedEdge<TVertex> : IDirectedEdge<TVertex>
	{
		private static readonly IEqualityComparer<DirectedEdge<TVertex>> FromToComparerInstance =
			new FromToEqualityComparer();
		public static IEqualityComparer<DirectedEdge<TVertex>> FromToComparer
		{
			get { return FromToComparerInstance; }
		}

		public bool Equals(DirectedEdge<TVertex> other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}
			return EqualityComparer<TVertex>.Default.Equals(From, other.From)
				&& EqualityComparer<TVertex>.Default.Equals(To, other.To);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			if (ReferenceEquals(this, obj))
			{
				return true;
			}
			var other = obj as DirectedEdge<TVertex>;
			return other != null && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (EqualityComparer<TVertex>.Default.GetHashCode(From) * 397)
					^ EqualityComparer<TVertex>.Default.GetHashCode(To);
			}
		}

		public static bool operator ==(DirectedEdge<TVertex> left, DirectedEdge<TVertex> right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(DirectedEdge<TVertex> left, DirectedEdge<TVertex> right)
		{
			return !Equals(left, right);
		}

		private sealed class FromToEqualityComparer : IEqualityComparer<DirectedEdge<TVertex>>
		{
			public bool Equals(DirectedEdge<TVertex> x, DirectedEdge<TVertex> y)
			{
				if (ReferenceEquals(x, y))
				{
					return true;
				}
				if (ReferenceEquals(x, null))
				{
					return false;
				}
				if (ReferenceEquals(y, null))
				{
					return false;
				}
				if (x.GetType() != y.GetType())
				{
					return false;
				}
				return EqualityComparer<TVertex>.Default.Equals(x.From, y.From)
					&& EqualityComparer<TVertex>.Default.Equals(x.To, y.To);
			}

			public int GetHashCode(DirectedEdge<TVertex> obj)
			{
				unchecked
				{
					return (EqualityComparer<TVertex>.Default.GetHashCode(obj.From) * 397)
						^ EqualityComparer<TVertex>.Default.GetHashCode(obj.To);
				}
			}
		}
	}
}
