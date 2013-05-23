#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using NUnit.Framework;
using Stile.Patterns.Behavioral.Validation;
#endregion

namespace Stile.Tests.Patterns.Behavioral.Validation
{
	[TestFixture]
	public class ValidateArgumentFixture
	{
		[Test]
		public void ArgumentIsNotNullOrEmpty()
		{
			AssertThrows(() => ReferenceNullOrEmpty(null, null), "ints");
			AssertThrows<ArgumentException>(() => ReferenceNullOrEmpty(new List<int>(), null), "ints");

			Assert.DoesNotThrow(() => ReferenceNullOrEmpty(new List<int> {1}, "hi"));
		}

		[Test]
		public void IsInEnum()
		{
			const DayOfWeek bogus = (DayOfWeek) 40;

			var exception = Assert.Throws<ArgumentOutOfRangeException>(() => EnumArgument(bogus));
			Assert.That(exception.Message, Contains.Substring("dayOfWeek"));
			Assert.That(exception.Message, Contains.Substring(bogus.ToString()));
			StringAssert.DoesNotContain("=>", exception.Message);

			const DayOfWeek friday = DayOfWeek.Friday;
			DayOfWeek output = EnumArgument(friday);
			Assert.That(output, Is.EqualTo(friday));
		}

		[Test]
		public void IsNotDefaultReference()
		{
			AssertThrows(() => ReferenceArgument(null), "fixture");

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

		[Test]
		public void IsNotNull()
		{
			AssertThrows(() => ReferenceArgumentNull(null), "fixture");
			AssertThrows(() => ReferenceArgumentNull_DiverseArguments(null, "kerfuffle"), "fixture");
			string message =
				AssertThrows(() => ReferenceArgumentNull_MultipleArgumentsOfSameType(null, new EventArgs()),
					", validated at line");
			Assert.That(message, Contains.Substring("saboteur"));
			Assert.That(message, Contains.Substring("another"));

			ValidateArgumentFixture vaf = null;
			TestDelegate testDelegate = () =>
			{
				vaf = ReferenceArgument_Returns(this);
			};
			Assert.DoesNotThrow(testDelegate);
			Assert.That(vaf, Is.EqualTo(this));

			Assert.DoesNotThrow(() => ReferenceArgumentNull(this));
		}

		[Test]
		public void IsNotNullOrEmpty()
		{
			AssertThrows(() => ReferenceArgumentNullOrEmpty_Compact(null, null), "strings");
			AssertThrows(() => ReferenceArgumentNullOrEmpty_Compact(new List<string>(), null), "strings");
			Assert.DoesNotThrow(()=> ReferenceArgumentNullOrEmpty_Compact(new List<string>(){string.Empty}, "hi"));
		}

		private static string AssertThrows(TestDelegate testDelegate, string substring)
		{
			string message = AssertThrows<ArgumentNullException>(testDelegate, substring);
			Assert.That(message, Contains.Substring("null"));
			return message;
		}

		private static string AssertThrows<TException>(TestDelegate testDelegate, string substring)
			where TException : Exception
		{
			var exception = Assert.Throws<TException>(testDelegate);
			Assert.That(exception.Message, Contains.Substring(substring));
			StringAssert.DoesNotContain("=>", exception.Message);
			return exception.Message;
		}

		private static DayOfWeek EnumArgument(DayOfWeek dayOfWeek)
		{
			return ValidateArgument.IsInEnum(() => dayOfWeek);
		}

		private static void ReferenceArgument(ValidateArgumentFixture fixture)
		{
			ValidateArgument.IsNotDefault(() => fixture);
		}

		private static void ReferenceArgumentNull(ValidateArgumentFixture fixture)
		{
			fixture.ValidateArgumentIsNotNull();
		}

		private static void ReferenceArgumentNullOrEmpty_Compact(IList<string> strings, string decoy)
		{
			strings.ValidateNotNullOrEmpty();
		}

		private static void ReferenceArgumentNull_DiverseArguments(ValidateArgumentFixture fixture, string decoy)
		{
			fixture.ValidateArgumentIsNotNull();
			decoy.ValidateArgumentIsNotNull();
		}

		private static void ReferenceArgumentNull_MultipleArgumentsOfSameType(EventArgs saboteur, EventArgs another)
		{
			saboteur.ValidateArgumentIsNotNull();
			another.ValidateArgumentIsNotNull();
		}

		private static ValidateArgumentFixture ReferenceArgument_Returns(ValidateArgumentFixture fixture)
		{
			return fixture.ValidateArgumentIsNotNull();
		}

		private static void ReferenceNullOrEmpty(IList<int> ints, string decoy)
		{
			ints.ValidateArgumentIsNotNullOrEmpty();
		}

		private static void ValueArgument(int someNumber)
		{
			ValidateArgument.IsNotDefault(() => someNumber);
		}
	}
}
