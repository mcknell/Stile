#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.Builders.OfExpectations;
using Stile.Prototypes.Specifications.Builders.OfInstruments;
using Stile.Prototypes.Specifications.SemanticModel;
#endregion

namespace Stile.Prototypes.Specifications
{
	public static class Specify
	{
		[System.Diagnostics.Contracts.Pure]
		public static IBoundProcedureBuilder<TSubject> For<TSubject>([CanBeNull] TSubject subject)
		{
			var source = new Source<TSubject>(subject);
			return new BoundProcedureBuilder<TSubject>(source);
		}

		[System.Diagnostics.Contracts.Pure]
		public static IBoundProcedureBuilder<TSubject> For<TSubject>([NotNull] Expression<Func<TSubject>> expression)
		{
			var source = new Source<TSubject>(expression.ValidateArgumentIsNotNull());
			return new BoundProcedureBuilder<TSubject>(source);
		}

		[System.Diagnostics.Contracts.Pure]
		public static IProcedureBuilder<TSubject> ForAny<TSubject>()
		{
			return new ProcedureBuilder<TSubject>();
		}

		[System.Diagnostics.Contracts.Pure]
		public static IFluentBoundExpectationBuilder<TSubject, TSubject> That<TSubject>(TSubject subject)
		{
			var instrument = new Instrument<TSubject, TSubject>(x => x);
			return new FluentBoundExpectationBuilder<TSubject, TSubject>(instrument, For(subject).Xray);
		}

		[System.Diagnostics.Contracts.Pure]
		public static IFluentExpectationBuilder<TSubject, TSubject> ThatAny<TSubject>()
		{
			var instrument = new Instrument<TSubject, TSubject>(x => x);
			return new FluentExpectationBuilder<TSubject, TSubject>(instrument);
		}
	}
}
