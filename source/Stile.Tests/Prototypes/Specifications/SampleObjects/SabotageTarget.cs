#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
#endregion

namespace Stile.Tests.Prototypes.Specifications.SampleObjects
{
	public class SabotageTarget
	{
		public SabotageTarget([NotNull] Saboteur saboteur)
		{
			Saboteur = saboteur.ValidateArgumentIsNotNull();
		}

		public Saboteur Saboteur { get; private set; }
	}
}
