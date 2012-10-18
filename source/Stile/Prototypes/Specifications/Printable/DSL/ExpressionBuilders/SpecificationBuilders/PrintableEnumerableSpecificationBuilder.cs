#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultHas;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultIs;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.SpecificationBuilders;
using Stile.Prototypes.Specifications.DSL.SemanticModel;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.ResultIs;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.Sources;
using Stile.Prototypes.Specifications.Printable.Output;
using Stile.Prototypes.Specifications.Printable.Output.Explainers;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SpecificationBuilders
{
	public interface IPrintableEnumerableSpecificationBuilder : IPrintableSpecificationBuilder,
		IEnumerableSpecificationBuilder {}

	public interface IPrintableEnumerableSpecificationBuilder<out TResult, out TItem, out THas, out TNegatableIs,
		out TIs, out TSpecifies> : IPrintableEnumerableSpecificationBuilder,
			IEnumerableSpecificationBuilder<TResult, TItem, THas, TNegatableIs, TIs, TSpecifies>
		where TResult : class, IEnumerable<TItem>
		where THas : class, IEnumerableHas<TResult, TItem, TSpecifies>
		where TNegatableIs : class, INegatableEnumerableIs<TResult, TItem, TIs, TSpecifies>
		where TIs : class, IEnumerableIs<TResult, TItem, TSpecifies>
		where TSpecifies : class, IPrintableSpecification {}

	public interface IPrintableEnumerableSpecificationBuilder<out TSubject, out TResult, out TItem, out THas,
		out TNegatableIs, out TIs, out TSource, out TSpecifies> :
			IPrintableEnumerableSpecificationBuilder<TResult, TItem, THas, TNegatableIs, TIs, TSpecifies>,
			IEnumerableSpecificationBuilder<TSubject, TResult, TItem, THas, TNegatableIs, TIs, TSource, TSpecifies>
		where TResult : class, IEnumerable<TItem>
		where THas : class, IEnumerableHas<TSubject, TResult, TItem, TSource, TSpecifies>
		where TNegatableIs : class, INegatableEnumerableIs<TSubject, TResult, TItem, TIs, TSpecifies>
		where TIs : class, IEnumerableIs<TSubject, TResult, TItem, TSpecifies>
		where TSource : class, IPrintableSource<TSubject>
		where TSpecifies : class, IFluentSpecification<TSubject, TResult> {}

	public interface IFluentEnumerableSpecificationBuilder<TSubject, out TResult, TItem> :
		IPrintableEnumerableSpecificationBuilder
			<TSubject, TResult, TItem,
				IEnumerableHas<TSubject, TResult, TItem, IPrintableSource<TSubject>, IFluentSpecification<TSubject, TResult>>,
				INegatableEnumerableIs
					<TSubject, TResult, TItem, IEnumerableIs<TSubject, TResult, TItem, IFluentSpecification<TSubject, TResult>>,
						IFluentSpecification<TSubject, TResult>>,
				IEnumerableIs<TSubject, TResult, TItem, IFluentSpecification<TSubject, TResult>>, IPrintableSource<TSubject>,
				IFluentSpecification<TSubject, TResult>>
		where TResult : class, IEnumerable<TItem> {}

	public interface IFluentBoundEnumerableSpecificationBuilder<TSubject, out TResult, TItem> :
		IFluentEnumerableSpecificationBuilder<TSubject, TResult, TItem>
		where TResult : class, IEnumerable<TItem> {}

	public class PrintableEnumerableSpecificationBuilder<TSubject, TResult, TItem> : //
		SpecificationBuilder
			<TSubject, TResult,
				IEnumerableHas<TSubject, TResult, TItem, IPrintableSource<TSubject>, IFluentSpecification<TSubject, TResult>>,
				INegatableEnumerableIs
					<TSubject, TResult, TItem, IEnumerableIs<TSubject, TResult, TItem, IFluentSpecification<TSubject, TResult>>,
						IFluentSpecification<TSubject, TResult>>,
				IEnumerableIs<TSubject, TResult, TItem, IFluentSpecification<TSubject, TResult>>, IPrintableSource<TSubject>,
				IFluentSpecification<TSubject, TResult>, IPrintableEvaluation<TResult>, ILazyReadableText,
				IPrintableSpecificationArguments<TSubject, TResult, IPrintableEvaluation<TResult>>>,
		IFluentBoundEnumerableSpecificationBuilder<TSubject, TResult, TItem>
		where TResult : class, IEnumerable<TItem>
	{
		public PrintableEnumerableSpecificationBuilder([NotNull] IPrintableSource<TSubject> source,
			[NotNull] Lazy<Func<TSubject, TResult>> instrument)
			: base(source, instrument) {}

		IEnumerableHas<TSubject, TResult, TItem, IPrintableSource<TSubject>, IFluentSpecification<TSubject, TResult>>
			ISpecificationBuilder
				<TResult,
					IEnumerableHas<TSubject, TResult, TItem, IPrintableSource<TSubject>, IFluentSpecification<TSubject, TResult>>,
					INegatableEnumerableIs
						<TSubject, TResult, TItem, IEnumerableIs<TSubject, TResult, TItem, IFluentSpecification<TSubject, TResult>>,
							IFluentSpecification<TSubject, TResult>>,
					IEnumerableIs<TSubject, TResult, TItem, IFluentSpecification<TSubject, TResult>>,
					IFluentSpecification<TSubject, TResult>>.Has
		{
			get { return Has; }
		}
		INegatableEnumerableIs
			<TSubject, TResult, TItem, IEnumerableIs<TSubject, TResult, TItem, IFluentSpecification<TSubject, TResult>>,
				IFluentSpecification<TSubject, TResult>>
			ISpecificationBuilder
				<TResult,
					IEnumerableHas<TSubject, TResult, TItem, IPrintableSource<TSubject>, IFluentSpecification<TSubject, TResult>>,
					INegatableEnumerableIs
						<TSubject, TResult, TItem, IEnumerableIs<TSubject, TResult, TItem, IFluentSpecification<TSubject, TResult>>,
							IFluentSpecification<TSubject, TResult>>,
					IEnumerableIs<TSubject, TResult, TItem, IFluentSpecification<TSubject, TResult>>,
					IFluentSpecification<TSubject, TResult>>.Is
		{
			get { return Is; }
		}

		public override IFluentSpecification<TSubject, TResult> Build(
			IPrintableSpecificationArguments<TSubject, TResult, IPrintableEvaluation<TResult>> arguments)
		{
			IPrintableSource<TSubject> source = Source;
			Lazy<Func<TSubject, TResult>> instrument = Instrument;
			Predicate<TResult> accepter = arguments.Accepter;
			IExplainer<TSubject, TResult> explainer = arguments.Explainer;
			Func<TResult, Exception, IPrintableEvaluation<TResult>> exceptionFilter =
				(result, exception) => arguments.ExceptionFilter.Invoke(result, exception);
			return new PrintableSpecification<TSubject, TResult>(source,
				instrument,
				accepter,
				explainer,
				exceptionFilter : exceptionFilter);
		}

		protected override
			IEnumerableHas<TSubject, TResult, TItem, IPrintableSource<TSubject>, IFluentSpecification<TSubject, TResult>>
			MakeHas()
		{
			return
				new EnumerableHas<TSubject, TResult, TItem, IPrintableSource<TSubject>, IFluentSpecification<TSubject, TResult>>(
					Source, Instrument);
		}

		protected override
			INegatableEnumerableIs
				<TSubject, TResult, TItem, IEnumerableIs<TSubject, TResult, TItem, IFluentSpecification<TSubject, TResult>>,
					IFluentSpecification<TSubject, TResult>> MakeIs()
		{
			return new PrintableEnumerableIs<TSubject, TResult, TItem>(Source, Negated.False, Instrument);
		}
	}
}
