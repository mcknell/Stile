#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using Stile.Types.Reflection;
#endregion

namespace Stile.Patterns.Lifetimes.Disposables
{
	public abstract class Disposable : IDisposable
	{
		private bool _recursivelyDisposing;
		public bool AlreadyDisposed { get; private set; }

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (AlreadyDisposed)
			{
				return;
			}
			if (disposing)
			{
				if (!_recursivelyDisposing)
				{
					_recursivelyDisposing = true;
					Disposing();
					_recursivelyDisposing = false;
				}
			}
			else
			{
				Finalizing();
			}
			AlreadyDisposed = true;
		}

		protected abstract void Disposing();
		protected virtual void Finalizing() {}

		protected void RequireNotAlreadyDisposed()
		{
			if (AlreadyDisposed)
			{
				throw new ObjectDisposedException(GetType().ToDebugString());
			}
		}
	}
}
