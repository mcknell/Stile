#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
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
			AssertFormat("Here's my printout: '{0:" + ExpressionFormatProvider.CSharp4 + "}'!", "Here's my printout: '() => i.ToString()'!");
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

		[Test]
		public void FormatMixed()
		{
			AssertFormat("Here's printout number {0}: '{1}'!", "Here's printout number 1: '() => i.ToString()'!", 1, _expression);
		}

		private void AssertFormat(string formatString, string expected, params object[] args)
		{
			object[] objects = args.Any() ? args : new object[] {_expression};
			string formatted = string.Format(new ExpressionFormatProvider(), formatString, objects);

			Assert.That(formatted, Is.EqualTo(expected));
		}
	}
}
