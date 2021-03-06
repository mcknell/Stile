﻿#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace Stile.Testing.SampleTypes
{
	public class Sample
	{
		public Sample(int intInt = Default.Int, IEnumerable<int> intEnumerable = null)
		{
			Int = intInt;
			IntList = new List<int>();
			IntEnumerable = intEnumerable ?? new int[0];
			IntCollection = new SampleCollection<int>();
		}

		public bool Bool { get; set; }
		public double Double { get; set; }

		[SampleProperty]
		public int Int { get; set; }
		public ISampleCollection<int> IntCollection { get; private set; }
		public IEnumerable<int> IntEnumerable { get; private set; }
		public IList<int> IntList { get; private set; }
		public int? NullableInt { get; set; }
		public string String { get; set; }
		public Action Thrower { get; set; }

		public void AddToEnumerable(int i, params int[] additional)
		{
			IntEnumerable = IntEnumerable.Concat(new[] {i}.Concat(additional)).ToList();
		}

		public void Throw()
		{
			Thrower.Invoke();
		}

		public static class Default
		{
			public const int Int = 1;
		}
	}
}
