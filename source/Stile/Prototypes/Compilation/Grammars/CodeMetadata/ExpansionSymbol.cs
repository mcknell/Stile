#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.CodeMetadata
{
	public class ExpansionSymbol : Symbol
	{
		private readonly Lazy<IReadOnlyCollection<SymbolLink>> _lazyFollowers;
		private readonly Lazy<IReadOnlyCollection<SymbolLink>> _lazyPriors;

		public ExpansionSymbol([NotNull] string token,
			[NotNull] Func<IReadOnlyCollection<SymbolLink>> priorsSource,
			[NotNull] Func<IReadOnlyCollection<SymbolLink>> followersSource)
			: base(token)
		{
			Func<IReadOnlyCollection<SymbolLink>> priors = priorsSource.ValidateArgumentIsNotNull();
			Func<IReadOnlyCollection<SymbolLink>> followers = followersSource.ValidateArgumentIsNotNull();
			_lazyPriors = new Lazy<IReadOnlyCollection<SymbolLink>>(priors);
			_lazyFollowers = new Lazy<IReadOnlyCollection<SymbolLink>>(followers);
		}

		[NotNull]
		public IReadOnlyCollection<SymbolLink> Followers
		{
			get
			{
				IReadOnlyCollection<SymbolLink> value = _lazyFollowers.Value;
				return value;
			}
		}
		[NotNull]
		public IReadOnlyCollection<SymbolLink> Priors
		{
			get
			{
				IReadOnlyCollection<SymbolLink> value = _lazyPriors.Value;
				return value;
			}
		}
	}
}
