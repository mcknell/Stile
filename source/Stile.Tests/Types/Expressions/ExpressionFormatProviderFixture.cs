﻿#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework;
using Stile.Types.Expressions.Printing;
#endregion

namespace Stile.Tests.Types.Expressions
{
	[TestFixture]
	public class ExpressionFormatProviderFixture
	{
		private Expression<Func<string>> _expression;

		[SetUp]
		public void Init()
		{
			int? i = 7;
			_expression = () => i.ToString();
		}

		[Test]
		public void FormatCSharp4()
		{
			AssertFormat("Here's my printout: '{0:" + ExpressionFormatProvider.CSharp4 + "}'!",
				"Here's my printout: '() => i.ToString()'!");
		}

		[Test]
		public void FormatMixed()
		{
			AssertFormat("Here's printout number {0}: '{1}'!",
				"Here's printout number 1: '() => i.ToString()'!",
				1,
				_expression);
		}

		[Test]
		public void FormatOther()
		{
			AssertFormat("Here's my printout: '{0}'!", "Here's my printout: '56'!", 56);
		}

		[Test]
		public void FormatRaw()
		{
			AssertFormat("Here's my printout: '{0}'!", "Here's my printout: '() => i.ToString()'!");
		}

		private void AssertFormat(string formatString, string expected, params object[] args)
		{
			object[] objects = args.Any() ? args : new object[] {_expression};
			string formatted = string.Format(new ExpressionFormatProvider(), formatString, objects);

			Assert.That(formatted, Is.EqualTo(expected));
		}
	}
}
