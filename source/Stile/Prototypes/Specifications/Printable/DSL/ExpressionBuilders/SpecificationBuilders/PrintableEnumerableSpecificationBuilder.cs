#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultHas;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultIs;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.SpecificationBuilders;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SpecificationBuilders
{
	public interface IPrintableEnumerableSpecificationBuilder : IPrintableSpecificationBuilder,
		IEnumerableSpecificationBuilder {}

	public interface IPrintableEnumerableSpecificationBuilder<out TResult, out TItem, out THas, out TNegatableIs,
		out TIs, out TSpecifies, out TQuantified> : IPrintableEnumerableSpecificationBuilder,
			IEnumerableSpecificationBuilder<TResult, TItem, THas, TNegatableIs, TIs, TSpecifies, TQuantified>
		where TResult : class, IEnumerable<TItem>
		where THas : class, IEnumerableHas<TResult, TItem, TSpecifies, TQuantified>
		where TNegatableIs : class, INegatableEnumerableIs<TResult, TItem, TIs, TSpecifies>
		where TIs : class, IEnumerableIs<TResult, TItem, TSpecifies>
		where TSpecifies : class, IPrintableSpecification
		where TQuantified : class, IQuantifiedEnumerableHas<TResult, TItem, TSpecifies> {}

	public interface IPrintableEnumerableSpecificationBuilder<out TSubject, out TResult, out TItem, out THas,
		out TNegatableIs, out TIs, out TSpecifies, out TQuantified> :
			IPrintableEnumerableSpecificationBuilder<TResult, TItem, THas, TNegatableIs, TIs, TSpecifies, TQuantified>,
			IEnumerableSpecificationBuilder<TSubject, TResult, TItem, THas, TNegatableIs, TIs, TSpecifies, TQuantified>
		where TResult : class, IEnumerable<TItem>
		where THas : class, IEnumerableHas<TSubject, TResult, TItem, TSpecifies, TQuantified>
		where TNegatableIs : class, INegatableEnumerableIs<TSubject, TResult, TItem, TIs, TSpecifies>
		where TIs : class, IEnumerableIs<TSubject, TResult, TItem, TSpecifies>
		where TSpecifies : class, IPrintableSpecification<TSubject, TResult>
		where TQuantified : class, IQuantifiedEnumerableHas<TResult, TItem, TSpecifies> {}

	public interface IFluentEnumerableSpecificationBuilder<TSubject, out TResult, TItem> :
		IPrintableEnumerableSpecificationBuilder< //
			TSubject, //
			TResult, //
			TItem, //
			IEnumerableHas< //
				TSubject, //
				TResult, //
				TItem, //
				IFluentSpecification<TSubject, TResult>, //
				IQuantifiedEnumerableHas< //
					TResult, //
					TItem, //
					IFluentSpecification<TSubject, TResult> //
					> //
				>, //
			INegatableEnumerableIs< //
				TSubject, //
				TResult, //
				TItem, //
				IEnumerableIs<TSubject, TResult, TItem, IFluentSpecification<TSubject, TResult>>, //
				IFluentSpecification<TSubject, TResult> //
				>, //
			IEnumerableIs<TSubject, TResult, TItem, IFluentSpecification<TSubject, TResult>>, //
			IFluentSpecification<TSubject, TResult>, //
			IQuantifiedEnumerableHas<TResult, TItem, IFluentSpecification<TSubject, TResult>>>
		where TResult : class, IEnumerable<TItem> {}

/*
	public class PrintableEnumerableSpecificationBuilder<TSubject, TResult, TItem> : //
		EnumerableSpecificationBuilder< //
			TSubject, //
			TResult, //
			TItem, //
			IEnumerableHas<TSubject, TResult, TItem, IFluentSpecification<TSubject, TResult>, int>, //
			INegatableEnumerableIs<TSubject, TResult, TItem>, //
			IEnumerableIs<TSubject, TResult, TItem>, //
			IPrintableSpecification<TSubject, TResult>, //
			IQuantifiedEnumerableHas<TSubject, TResult, TItem> //
			>,
		IPrintableEnumerableSpecificationBuilder<TSubject, TResult, TItem>
		where TResult : class, IEnumerable<TItem>
	{
		private readonly Lazy<Func<TSubject, TResult>> _instrument;

		public PrintableEnumerableSpecificationBuilder([NotNull] Lazy<Func<TSubject, TResult>> instrument)
		{
			_instrument = instrument.ValidateArgumentIsNotNull();
		}

		protected override IEnumerableHas<TSubject, TResult, TItem> MakeHas()
		{
			return new EnumerableHas<TSubject, TResult, TItem>(_instrument);
		}

		protected override INegatableEnumerableIs<TSubject, TResult, TItem> MakeIs()
		{
			return new EnumerableIs<TSubject, TResult, TItem>(Negated.False, _instrument);
		}
	}
*/
}
