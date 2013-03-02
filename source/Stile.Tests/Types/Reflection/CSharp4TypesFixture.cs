#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Linq;
using NUnit.Framework;
using Stile.Types;
#endregion

namespace Stile.Tests.Types.Reflection
{
	[TestFixture]
	public class CSharp4TypesFixture
	{
		[Test]
		public void CSharpTypeAliases()
		{
			CollectionAssert.IsEmpty(CSharp4Types.SimpleTypes.Except(CSharp4Types.TypeAliases.Keys).ToList());
		}

		[Test]
		public void Primitives()
		{
			CollectionAssert.IsEmpty(
				typeof(int).Assembly.GetTypes().Where(x => x.IsPrimitive).Except(CSharp4Types.Primitives).ToList());
		}
	}
}
