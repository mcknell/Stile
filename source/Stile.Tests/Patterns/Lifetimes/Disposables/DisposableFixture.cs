#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using NUnit.Framework;
using Stile.Patterns.Lifetimes.Disposables;
#endregion

namespace Stile.Tests.Patterns.Lifetimes.Disposables
{
	[TestFixture]
	public class DisposableFixture
	{
		[Test]
		public void Dispose()
		{
			DisposableSample disposableSample;
			using (disposableSample = new DisposableSample())
			{
				Assert.That(disposableSample.AlreadyDisposed, Is.False);
			}
			Assert.That(disposableSample.AlreadyDisposed, Is.True);

			Assert.Throws<ObjectDisposedException>(() => disposableSample.Frob());
		}

		public class DisposableSample : Disposable
		{
			public void Frob()
			{
				RequireNotAlreadyDisposed();
			}

			protected override void Disposing() {}
		}
	}
}
