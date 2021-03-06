#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
#endregion

namespace Stile.Tests.Prototypes.Specifications.SampleObjects
{
	public class Saboteur
	{
		private readonly bool _writeToConsole;

		public Saboteur(int? dudsBeforeThrow = null, bool? writeToConsole = null)
		{
			if (writeToConsole.HasValue)
			{
				_writeToConsole = writeToConsole.Value;
			}
			else
			{
				bool b;
				_writeToConsole = bool.TryParse(ConfigurationManager.AppSettings["TraceThreadsToConsole"], out b) && b;
			}
			MisfiresRemaining = dudsBeforeThrow;
		}

		public TimeSpan? Fuse { get; set; }
		public Lazy<Exception> LazyThrower { get; private set; }
		/// <summary>
		/// Count of times that <see cref="SuicidalSideEffect"/> can be invoked before throwing.
		/// </summary>
		public int? MisfiresRemaining { get; private set; }
		public Saboteur SuicidalSideEffect
		{
			get
			{
				if (MisfiresRemaining.HasValue && MisfiresRemaining.Value > 0)
				{
					MisfiresRemaining--;
				}
				else
				{
					Throw();
				}
				return this;
			}
		}
		public bool ThrowCalled { get; private set; }

		public void Load<TException>(Func<TException> thrower) where TException : Exception
		{
			LazyThrower = new Lazy<Exception>(thrower.Invoke);
		}

		public void Throw()
		{
			var stopwatch = new Stopwatch();
			if (Fuse.HasValue)
			{
				if (_writeToConsole)
				{
					Thread currentThread = Thread.CurrentThread;
					Console.WriteLine(
						"Saboteur sleeping at {0:HH:mm:ss.fff} for {1}ms on thread {2}, background=={3}, pool=={4}",
						DateTime.Now,
						Fuse.Value.TotalMilliseconds,
						currentThread.ManagedThreadId,
						currentThread.IsBackground,
						currentThread.IsThreadPoolThread);
				}
				stopwatch.Start();
				Thread.Sleep(Fuse.Value);
			}
			else
			{
				stopwatch.Start();
			}
			ThrowCalled = true;
			if (_writeToConsole)
			{
				Console.WriteLine("Saboteur about to throw at: {0:HH:mm:ss.fff}", DateTime.Now);
				Console.WriteLine("Elapsed Saboteur time: {0}ms", stopwatch.ElapsedMilliseconds);
			}
			throw LazyThrower.Value;
		}
	}
}
