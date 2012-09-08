#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using System;
using NUnit.Framework;
using Stile.Validation;
#endregion

namespace Stile.Tests.Validation
{
	[TestFixture]
	public class ValidateArgumentFixture
	{
		[Test]
		public void IsInEnum()
		{
			const DayOfWeek bogus = (DayOfWeek) 40;

			var exception = Assert.Throws<ArgumentOutOfRangeException>(() => EnumArgument(bogus));
			Assert.That(exception.Message, Contains.Substring("dayOfWeek"));
			Assert.That(exception.Message, Contains.Substring(bogus.ToString()));
			StringAssert.DoesNotContain("=>", exception.Message);
		}

		[Test]
		public void IsNotDefaultReference()
		{
			var exception = Assert.Throws<ArgumentNullException>(() => ReferenceArgument(null));
			Assert.That(exception.Message, Contains.Substring("fixture"));
			Assert.That(exception.Message, Contains.Substring("null"));
			StringAssert.DoesNotContain("=>", exception.Message);

			Assert.DoesNotThrow(() => ReferenceArgument(this));
		}

		[Test]
		public void IsNotDefaultValue()
		{
			var exception = Assert.Throws<ArgumentOutOfRangeException>(() => ValueArgument(0));
			Assert.That(exception.Message, Contains.Substring("someNumber"));
			StringAssert.DoesNotContain("=>", exception.Message);

			Assert.DoesNotThrow(() => ValueArgument(1));
		}

		private static void EnumArgument(DayOfWeek dayOfWeek)
		{
			ValidateArgument.IsInEnum(dayOfWeek, () => dayOfWeek);
		}

		private static void ReferenceArgument(ValidateArgumentFixture fixture)
		{
			ValidateArgument.IsNotDefault(() => fixture);
		}

		private static void ValueArgument(int someNumber)
		{
			ValidateArgument.IsNotDefault(() => someNumber);
		}
	}
}
