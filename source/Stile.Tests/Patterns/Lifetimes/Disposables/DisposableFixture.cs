#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
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
