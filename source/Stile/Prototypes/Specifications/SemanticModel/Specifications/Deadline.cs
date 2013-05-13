#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Configuration;
using System.Threading;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Specifications
{
	public interface IDeadline
	{
		CancellationToken CancellationToken { get; }
		bool OnThisThread { get; }
		TimeSpan Timeout { get; }
	}

	public class Deadline : IDeadline
	{
		public static readonly Deadline Async = new Deadline(false);
		public static readonly Deadline Synchronous = new Deadline(true);

		static Deadline()
		{
			DefaultTimeout = TimeSpan.FromSeconds(5);
			try
			{
				TimeSpan timeout;
				if (TimeSpan.TryParse(ConfigurationManager.AppSettings["defaultEvaluationTimeoutForTimespanParse"],
					out timeout))
				{
					DefaultTimeout = timeout;
				}
			}
			catch (ConfigurationErrorsException) {}
		}

		protected Deadline(bool onThisThread)
			: this(DefaultTimeout, onThisThread) {}

		public Deadline(TimeSpan timeout, bool onThisThread)
			: this(timeout.Duration(), CancellationToken.None, onThisThread) {}

		private Deadline(TimeSpan timeout, CancellationToken cancellationToken, bool onThisThread)
		{
			Timeout = timeout;
			CancellationToken = cancellationToken;
			OnThisThread = onThisThread;
		}

		public CancellationToken CancellationToken { get; private set; }
		public static TimeSpan DefaultTimeout { get; private set; }
		public bool OnThisThread { get; private set; }
		public TimeSpan Timeout { get; private set; }

		public static void SetDefaultTimeout(TimeSpan timeout)
		{
			DefaultTimeout = timeout.Duration();
		}

		public static Deadline ToDeadline(TimeSpan timeSpan)
		{
			return new Deadline(timeSpan, false);
		}

		public static implicit operator Deadline(TimeSpan timeSpan)
		{
			return new Deadline(timeSpan, false);
		}
	}
}
