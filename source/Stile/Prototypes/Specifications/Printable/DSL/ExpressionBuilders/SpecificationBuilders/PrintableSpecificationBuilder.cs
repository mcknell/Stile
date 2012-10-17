#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultHas;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultIs;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.SpecificationBuilders;
using Stile.Prototypes.Specifications.DSL.SemanticModel;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.ResultIs;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.Sources;
using Stile.Prototypes.Specifications.Printable.Output;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SpecificationBuilders
{
	public interface IPrintableSpecificationBuilder : IEmittingSpecificationBuilder {}

	public interface IPrintableSpecificationBuilder<out TSubject, out TResult, out THas, out TNegatableIs, out TIs,
		out TSource, out TSpecifies, out TEvaluation> : IPrintableSpecificationBuilder,
			IEmittingSpecificationBuilder
				<TSubject, TResult, THas, TNegatableIs, TIs, TSpecifies, TEvaluation, ILazyReadableText>
		where THas : class, IHas<TSubject, TResult, TSource, TSpecifies>
		where TNegatableIs : class, INegatableIs<TSubject, TResult, TIs, TSpecifies>
		where TIs : class, IIs<TSubject, TResult, TSpecifies>
		where TSource : class, IPrintableSource<TSubject>
		where TSpecifies : class, IFluentSpecification<TSubject, TResult>
		where TEvaluation : class, IPrintableEvaluation<TResult, ILazyReadableText> {}

	public interface IFluentSpecificationBuilder<TSubject, out TResult> : IPrintableSpecificationBuilder< //
		TSubject, //
		TResult, //
		IHas<TSubject, TResult, IPrintableSource<TSubject>, IFluentSpecification<TSubject, TResult>>, //
		INegatableIs< //
			TSubject, //
			TResult, //
			IIs<TSubject, TResult, IFluentSpecification<TSubject, TResult>>, //
			IFluentSpecification<TSubject, TResult> //
			>, //
		IIs<TSubject, TResult, IFluentSpecification<TSubject, TResult>>, //
		IPrintableSource<TSubject>, //
		IFluentSpecification<TSubject, TResult>, //
		IPrintableEvaluation<TResult, ILazyReadableText>> {}

	public interface IFluentSpecificationBuilder<TSubject> : IFluentSpecificationBuilder<TSubject, TSubject> {}

	public interface IPrintableBoundSpecificationBuilder : IPrintableSpecificationBuilder,
		IBoundSpecificationBuilder {}

	public interface IPrintableBoundSpecificationBuilder<out TSubject, out TResult, out THas, out TNegatableIs, out TIs,
		out TSource, out TSpecifies, out TEvaluation> : IPrintableBoundSpecificationBuilder,
			IPrintableSpecificationBuilder<TSubject, TResult, THas, TNegatableIs, TIs, TSource, TSpecifies, TEvaluation> /*,
			IBoundSpecificationBuilder<TSubject, TResult, THas, TNegatableIs, TIs, TSpecifies, TEvaluation>*/
		where THas : class, IHas<TSubject, TResult, TSource, TSpecifies>
		where TNegatableIs : class, INegatableIs<TSubject, TResult, TIs, TSpecifies>
		where TIs : class, IIs<TSubject, TResult, TSpecifies>
		where TSource : class, IPrintableSource<TSubject>
		where TSpecifies : class, IFluentBoundSpecification<TSubject, TResult>
		where TEvaluation : class, IPrintableEvaluation<TResult, ILazyReadableText> {}

	public interface IFluentBoundSpecificationBuilder<TSubject, out TResult> : IPrintableBoundSpecificationBuilder< //
		TSubject, //
		TResult, //
		IHas<TSubject, TResult, IPrintableSource<TSubject>, IFluentBoundSpecification<TSubject, TResult>>, //
		INegatableIs< //
			TSubject, //
			TResult, //
			IIs<TSubject, TResult, IFluentBoundSpecification<TSubject, TResult>>, //
			IFluentBoundSpecification<TSubject, TResult> //
			>, //
		IIs<TSubject, TResult, IFluentBoundSpecification<TSubject, TResult>>, //
		IPrintableSource<TSubject>, //
		IFluentBoundSpecification<TSubject, TResult>, //
		IPrintableEvaluation<TResult, ILazyReadableText>> {}

	public interface IPrintableSpecificationBuilderState : ISpecificationBuilderState {}

	public interface IPrintableSpecificationBuilderState<TSubject, TResult> : IPrintableSpecificationBuilderState,
		ISpecificationBuilderState<TSubject, TResult, IPrintableSource<TSubject>> {}

	public class PrintableSpecificationBuilder<TSubject, TResult> : SpecificationBuilder< //
		TSubject, //
		TResult, //
		IHas<TSubject, TResult, IPrintableSource<TSubject>, IFluentBoundSpecification<TSubject, TResult>>, //
		INegatableIs< //
			TSubject, //
			TResult, //
			IIs<TSubject, TResult, IFluentBoundSpecification<TSubject, TResult>>, //
			IFluentBoundSpecification<TSubject, TResult>>, //
		IIs<TSubject, TResult, IFluentBoundSpecification<TSubject, TResult>>, //
		IPrintableSource<TSubject>, //
		IFluentBoundSpecification<TSubject, TResult>, //
		IPrintableEvaluation<TResult>, //
		ILazyReadableText //
		>,
		IFluentSpecificationBuilder<TSubject, TResult>,
		IFluentBoundSpecificationBuilder<TSubject, TResult>,
		IPrintableSpecificationBuilderState<TSubject, TResult>
	{
		private readonly Lazy<Func<TSubject, TResult>> _instrument;

		public PrintableSpecificationBuilder([NotNull] IPrintableSource<TSubject> source,
			[NotNull] Expression<Func<TSubject, TResult>> expression)
			: this(source, new Lazy<Func<TSubject, TResult>>(expression.Compile)) {}

		public PrintableSpecificationBuilder(IPrintableSource<TSubject> source,
			[NotNull] Lazy<Func<TSubject, TResult>> instrument)
			: base(source, instrument)
		{
			_instrument = instrument.ValidateArgumentIsNotNull();
		}

		IHas<TSubject, TResult, IPrintableSource<TSubject>, IFluentSpecification<TSubject, TResult>>
			ISpecificationBuilder
				<TResult, IHas<TSubject, TResult, IPrintableSource<TSubject>, IFluentSpecification<TSubject, TResult>>,
					INegatableIs
						<TSubject, TResult, IIs<TSubject, TResult, IFluentSpecification<TSubject, TResult>>,
							IFluentSpecification<TSubject, TResult>>, IIs<TSubject, TResult, IFluentSpecification<TSubject, TResult>>,
					IFluentSpecification<TSubject, TResult>>.Has
		{
			get { return Has; }
		}
		INegatableIs
			<TSubject, TResult, IIs<TSubject, TResult, IFluentSpecification<TSubject, TResult>>,
				IFluentSpecification<TSubject, TResult>>
			ISpecificationBuilder
				<TResult, IHas<TSubject, TResult, IPrintableSource<TSubject>, IFluentSpecification<TSubject, TResult>>,
					INegatableIs
						<TSubject, TResult, IIs<TSubject, TResult, IFluentSpecification<TSubject, TResult>>,
							IFluentSpecification<TSubject, TResult>>, IIs<TSubject, TResult, IFluentSpecification<TSubject, TResult>>,
					IFluentSpecification<TSubject, TResult>>.Is
		{
			get { return Is; }
		}

		protected override
			IHas<TSubject, TResult, IPrintableSource<TSubject>, IFluentBoundSpecification<TSubject, TResult>> MakeHas()
		{
			return new Has<TSubject, TResult, IPrintableSource<TSubject>, IFluentBoundSpecification<TSubject, TResult>>(
				Source, _instrument);
		}

		protected override INegatableIs< //
			TSubject, //
			TResult, //
			IIs<TSubject, TResult, IFluentBoundSpecification<TSubject, TResult>>, //
			IFluentBoundSpecification<TSubject, TResult>> MakeIs()
		{
			return new PrintableIs<TSubject, TResult>(Source, Negated.False, _instrument);
		}
	}

	public class PrintableSpecificationBuilder<TSubject> : PrintableSpecificationBuilder<TSubject, TSubject>,
		IFluentSpecificationBuilder<TSubject>
	{
		public PrintableSpecificationBuilder([NotNull] Expression<Func<TSubject, TSubject>> expression)
			: this(PrintableSource<TSubject>.Empty, expression) {}

		public PrintableSpecificationBuilder([NotNull] IPrintableSource<TSubject> source,
			[NotNull] Expression<Func<TSubject, TSubject>> expression)
			: base(source, expression) {}

		IHas<TSubject, TSubject, IPrintableSource<TSubject>, IFluentSpecification<TSubject, TSubject>>
			ISpecificationBuilder
				<TSubject, IHas<TSubject, TSubject, IPrintableSource<TSubject>, IFluentSpecification<TSubject, TSubject>>,
					INegatableIs
						<TSubject, TSubject, IIs<TSubject, TSubject, IFluentSpecification<TSubject, TSubject>>,
							IFluentSpecification<TSubject, TSubject>>, IIs<TSubject, TSubject, IFluentSpecification<TSubject, TSubject>>,
					IFluentSpecification<TSubject, TSubject>>.Has
		{
			get { return Has; }
		}
		INegatableIs
			<TSubject, TSubject, IIs<TSubject, TSubject, IFluentSpecification<TSubject, TSubject>>,
				IFluentSpecification<TSubject, TSubject>>
			ISpecificationBuilder
				<TSubject, IHas<TSubject, TSubject, IPrintableSource<TSubject>, IFluentSpecification<TSubject, TSubject>>,
					INegatableIs
						<TSubject, TSubject, IIs<TSubject, TSubject, IFluentSpecification<TSubject, TSubject>>,
							IFluentSpecification<TSubject, TSubject>>, IIs<TSubject, TSubject, IFluentSpecification<TSubject, TSubject>>,
					IFluentSpecification<TSubject, TSubject>>.Is
		{
			get { return Is; }
		}
	}
}
