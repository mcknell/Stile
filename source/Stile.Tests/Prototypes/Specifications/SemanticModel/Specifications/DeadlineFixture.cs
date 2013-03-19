#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using NUnit.Framework;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Tests.Prototypes.Specifications.SemanticModel.Specifications
{
	[TestFixture]
	public class DeadlineFixture
	{
		[Test]
		public void DefaultTimeoutIsSettable()
		{
			TimeSpan timeout = TimeSpan.FromMilliseconds(10);
			Assert.That(Deadline.DefaultTimeout, Is.Not.EqualTo(timeout));
			Deadline.DefaultTimeout = timeout;
			Assert.That(Deadline.DefaultTimeout, Is.EqualTo(timeout));
		}

		[Test]
		public void FromTimeSpan()
		{
			const bool onThisThread = true;
			TimeSpan timeout = TimeSpan.FromMilliseconds(10);
			var deadline = new Deadline(timeout, onThisThread);
			Assert.That(deadline.Timeout, Is.EqualTo(timeout));
			Assert.That(deadline.OnThisThread, Is.EqualTo(onThisThread));
		}
	}
}
