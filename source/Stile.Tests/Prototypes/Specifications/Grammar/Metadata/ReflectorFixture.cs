#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Stile.Prototypes.Compilation.Grammars.ContextFree;
using Stile.Prototypes.Specifications.Grammar.Metadata;
#endregion

namespace Stile.Tests.Prototypes.Specifications.Grammar.Metadata
{
	[TestFixture]
	public class ReflectorFixture
	{
		private const int Prior = 8;

		[Test]
		public void NoSymbolOrAlias()
		{
			Action<int> action = NoSymbolOrAliasTarget;
			MethodInfo methodBase = action.Method;
			RuleExpansionAttribute attribute = methodBase.GetCustomAttributes<RuleExpansionAttribute>(false).First();
			List<ILink> links = Reflector.GetLinks(methodBase, attribute).ToList();
			Assert.NotNull(links);
			Assert.That(links, Has.Count.EqualTo(1));
			ILink first = links.First();
			Assert.NotNull(first);
			Assert.NotNull(first.Prior);
			Assert.That(first.Prior.Token, Is.EqualTo(Prior.ToString(CultureInfo.InvariantCulture)));
			Assert.That(first.Symbol.Token, Is.EqualTo(methodBase.Name).IgnoreCase);
			string parameterName = methodBase.GetParameters()[0].Name;
			string titleCase = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(parameterName);
			string expected = string.Format("({0} {1} aka \"{2}\")", methodBase.Name, titleCase, parameterName);
			Assert.That(first.Clause.ToString(), Is.EqualTo(expected).IgnoreCase);
		}

		[RuleExpansion(Prior)]
		private void NoSymbolOrAliasTarget([Symbol] int foo) {}
	}
}
