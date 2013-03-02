#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using NUnit.Framework;
using Stile.Readability;
using Stile.Types.Reflection;
#endregion

namespace Stile.Tests.Types.Reflection
{
	[TestFixture]
	public class TypeExtensionsIsOrDerivesFromFixture
	{
		[Test]
		public void IsOrDerivesFrom()
		{
			// assert
			Assert.That(Null.Type.IsOrDerivesFrom(null), Is.False);
			Assert.That(Null.Type.IsOrDerivesFrom(typeof(object)), Is.False);
			Assert.That(typeof(int).IsOrDerivesFrom(null), Is.False);
			Assert.That(typeof(int).IsOrDerivesFrom(typeof(int)), Is.True);
			Assert.That(typeof(int).IsOrDerivesFrom(typeof(object)), Is.True);
			Assert.That(typeof(object).IsOrDerivesFrom(typeof(object)), Is.True);

			Assert.That(Null.Type.IsOrDerivesFrom<object>(), Is.False);
			Assert.That(typeof(int).IsOrDerivesFrom<int>(), Is.True);
			Assert.That(typeof(int).IsOrDerivesFrom<object>(), Is.True);
			Assert.That(typeof(string).IsOrDerivesFrom<object>(), Is.True);
			Assert.That(typeof(MulticastDelegate).IsOrDerivesFrom<object>(), Is.True);
			Assert.That(typeof(int).IsOrDerivesFrom<ValueType>(), Is.True);
			Assert.That(typeof(string).IsOrDerivesFrom<ValueType>(), Is.False);
			Assert.That(typeof(object).IsOrDerivesFrom<object>(), Is.True);
		}
	}
}
