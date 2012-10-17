#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultIs;
using Stile.Prototypes.Specifications.DSL.SemanticModel;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.Sources;
using Stile.Prototypes.Specifications.Printable.Output;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.ResultIs
{
	public class PrintableEnumerableIs<TSubject, TResult, TItem> :
		EnumerableIs
			<TSubject, TResult, TItem, IPrintableSource<TSubject>,
				IEnumerableIs<TSubject, TResult, TItem, IFluentSpecification<TSubject, TResult>>,
				IFluentSpecification<TSubject, TResult>, IPrintableEvaluation<TResult, ILazyReadableText>>,
		INegatableEnumerableIs
			<TSubject, TResult, TItem, IEnumerableIs<TSubject, TResult, TItem, IFluentSpecification<TSubject, TResult>>,
				IFluentSpecification<TSubject, TResult>>
		where TResult : class, IEnumerable<TItem>
	{
		public PrintableEnumerableIs(IPrintableSource<TSubject> source,
			Negated negated,
			Lazy<Func<TSubject, TResult>> instrument)
			: base(source, negated, instrument) {}

		protected override IEnumerableIs<TSubject, TResult, TItem, IFluentSpecification<TSubject, TResult>> Factory(
			Negated negated, Lazy<Func<TSubject, TResult>> instrument, IPrintableSource<TSubject> source)
		{
			return new PrintableEnumerableIs<TSubject, TResult, TItem>(source, negated, instrument);
		}
	}
}
