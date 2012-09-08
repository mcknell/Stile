#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using System;
using Stile.Types.Reflection;
#endregion

namespace Stile.Patterns.Disposables
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
