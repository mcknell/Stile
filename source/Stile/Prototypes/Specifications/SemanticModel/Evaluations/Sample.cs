#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Readability;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Evaluations
{
	public interface ISample {}

	public interface ISample<out TSubject> : ISample
	{
		[NotNull]
		Lazy<string> RuntimeDescription { get; }
		[NotNull]
		ISource<TSubject> Source { get; }
		DateTime Taken { get; }
		TSubject Value { get; }
	}

	public class Sample<TSubject> : ISample<TSubject>
	{
		public Sample(TSubject value,
			[NotNull] ISource<TSubject> source,
			DateTime taken)
		{
			Taken = taken;
			Value = value;
			Source = source.ValidateArgumentIsNotNull();
			RuntimeDescription = value.ToLazyDebugString();
		}

		public Lazy<string> RuntimeDescription { get; private set; }
		public ISource<TSubject> Source { get; private set; }
		public DateTime Taken { get; private set; }
		public TSubject Value { get; private set; }
	}
}
