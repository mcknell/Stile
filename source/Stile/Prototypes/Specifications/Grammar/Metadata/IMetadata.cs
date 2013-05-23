#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
#endregion

namespace Stile.Prototypes.Specifications.Grammar.Metadata
{
	public interface IMetadata
	{
		[CanBeNull]
		string Alias { get; }
		[CanBeNull]
		string Token { get; }
	}

	public interface IMetadataWithPrior : IMetadata
	{
		[CanBeNull]
		string Prior { get; }
	}
}
