#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Threading;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Specifications
{
	public interface IDeadline
	{
		CancellationToken? CancellationToken { get; }
		bool OnThisThread { get; }
		TimeSpan? Timeout { get; }
	}

	public class Deadline : IDeadline
	{
		public static TimeSpan DefaultTimeout = TimeSpan.FromSeconds(5);

		public Deadline(TimeSpan timeout, bool onThisThread = false)
			: this(timeout.Duration(), null, onThisThread) {}

		private Deadline(TimeSpan? timeout, CancellationToken? cancellationToken, bool onThisThread)
		{
			Timeout = timeout;
			CancellationToken = cancellationToken;
			OnThisThread = onThisThread;
		}

		public CancellationToken? CancellationToken { get; private set; }
		public bool OnThisThread { get; private set; }
		public TimeSpan? Timeout { get; private set; }
	}
}
