#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Stile.Prototypes.Collections.Sorting;
#endregion

namespace Stile.Tests.Prototypes.Collections.Sorting
{
	[TestFixture]
	public class ToposortFixture
	{
		[Test]
		public void SortDag()
		{
			const string cat = "cat";
			const string dog = "dog";
			const string mammal = "mammal";
			const string fish = "fish";
			const string vertebrate = "vertebrate";
			var items = new List<string> {vertebrate, mammal, cat, dog, fish};
			Assert.That(items.IndexOf(cat), Is.Not.LessThan(items.IndexOf(mammal)));
			Assert.That(items.IndexOf(dog), Is.Not.LessThan(items.IndexOf(mammal)));
			Assert.That(items.IndexOf(mammal), Is.Not.LessThan(items.IndexOf(vertebrate)));
			Assert.That(items.IndexOf(fish), Is.Not.LessThan(items.IndexOf(vertebrate)));
			Func<string, string[]> adjacencyFunction = s =>
			{
				switch (s)
				{
					case mammal:
						return new[] {cat, dog};
					case vertebrate:
						return new[] {mammal, fish};
				}
				return new string[0];
			};
			var testSubject = new Toposort<string>(items, adjacencyFunction);

			// act
			List<string> sorted = testSubject.SortDag().ToList();

			Assert.That(sorted.IndexOf(cat), Is.LessThan(sorted.IndexOf(mammal)));
			Assert.That(sorted.IndexOf(dog), Is.LessThan(sorted.IndexOf(mammal)));
			Assert.That(sorted.IndexOf(mammal), Is.LessThan(sorted.IndexOf(vertebrate)));
			Assert.That(sorted.IndexOf(fish), Is.LessThan(sorted.IndexOf(vertebrate)));
		}
	}
}
