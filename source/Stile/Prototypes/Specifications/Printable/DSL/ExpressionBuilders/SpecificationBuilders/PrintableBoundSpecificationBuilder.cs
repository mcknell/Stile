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
	public interface IPrintableBoundSpecificationBuilder : IPrintableSpecificationBuilder,
		IBoundSpecificationBuilder {}

	public interface IPrintableBoundSpecificationBuilder<out TSubject, out TResult, out THas, out TNegatableIs, out TIs,
		out TSpecifies, out TEvaluation> : IPrintableBoundSpecificationBuilder,
			IPrintableSpecificationBuilder<TSubject, TResult, THas, TNegatableIs, TIs, TSpecifies, TEvaluation>,
			IBoundSpecificationBuilder<TSubject, TResult, THas, TNegatableIs, TIs, TSpecifies, TEvaluation>
		where THas : class, IHas<TSubject, TResult, TSpecifies>
		where TNegatableIs : class, INegatableIs<TSubject, TResult, TIs, TSpecifies>
		where TIs : class, IIs<TSubject, TResult, TSpecifies>
		where TSpecifies : class, IPrintableBoundSpecification<TSubject, TResult, TEvaluation>
		where TEvaluation : class, IPrintableBoundEvaluation<TResult, ILazyReadableText> {}

	public interface IPrintableBoundSpecificationBuilder<TSubject, out TResult, out THas, out TNegatableIs, out TIs> :
		IPrintableBoundSpecificationBuilder
			<TSubject, TResult, THas, TNegatableIs, TIs, IPrintableBoundSpecification<TSubject, TResult>,
				IPrintableBoundEvaluation<TResult, ILazyReadableText>>
		where THas : class, IHas<TSubject, TResult, IPrintableBoundSpecification<TSubject, TResult>>
		where TNegatableIs : class, INegatableIs<TSubject, TResult, TIs, IPrintableBoundSpecification<TSubject, TResult>>
		where TIs : class, IIs<TSubject, TResult, IPrintableBoundSpecification<TSubject, TResult>> {}

	public interface IFluentBoundSpecificationBuilder<TSubject, out TResult> : IPrintableBoundSpecificationBuilder< //
		TSubject, //
		TResult, //
		IHas<TSubject, TResult, IPrintableBoundSpecification<TSubject, TResult>>, //
		INegatableIs< //
			TSubject, //
			TResult, //
			IIs<TSubject, TResult, IPrintableBoundSpecification<TSubject, TResult>>, //
			IPrintableBoundSpecification<TSubject, TResult> //
			>, //
		IIs<TSubject, TResult, IPrintableBoundSpecification<TSubject, TResult>>, //
		IPrintableBoundSpecification<TSubject, TResult>, //
		IPrintableBoundEvaluation<TResult, ILazyReadableText>> {}

	public interface IPrintableBoundSpecificationBuilderState : IPrintableSpecificationBuilderState,
		IBoundSpecificationBuilderState {}

	public interface IPrintableBoundSpecificationBuilderState<out TSubject, out TSource> :
		IPrintableBoundSpecificationBuilderState,
		IBoundSpecificationBuilderState<TSubject, TSource>
		where TSource : class, IPrintableSource<TSubject> {}

	public abstract class PrintableBoundSpecificationBuilder<TSubject, TResult, THas, TNegatableIs, TIs> :
		BoundSpecificationBuilder
			<TSubject, TResult, THas, TNegatableIs, TIs, IPrintableBoundSpecification<TSubject, TResult>,
				IPrintableBoundEvaluation<TResult, ILazyReadableText>, IPrintableSource<TSubject>>,
		IPrintableBoundSpecificationBuilder<TSubject, TResult, THas, TNegatableIs, TIs>,
		IPrintableBoundSpecificationBuilderState<TSubject, IPrintableSource<TSubject>>
		where THas : class, IHas<TSubject, TResult, IPrintableBoundSpecification<TSubject, TResult>>
		where TNegatableIs : class, INegatableIs<TSubject, TResult, TIs, IPrintableBoundSpecification<TSubject, TResult>>
		where TIs : class, IIs<TSubject, TResult, IPrintableBoundSpecification<TSubject, TResult>>
	{
		protected PrintableBoundSpecificationBuilder([NotNull] IPrintableSource<TSubject> source)
			: base(source) {}

		public Lazy<string> SubjectDescription
		{
			get { return Source.Description; }
		}
	}

	public class PrintableBoundSpecificationBuilder<TSubject, TResult> : PrintableBoundSpecificationBuilder< //
		TSubject, //
		TResult, //
		IHas< //
			TSubject, //
			TResult, //
			IPrintableBoundSpecification<TSubject, TResult> //
			>, //
		INegatableIs< //
			TSubject, //
			TResult, //
			IIs<TSubject, TResult, IPrintableBoundSpecification<TSubject, TResult>>, //
			IPrintableBoundSpecification<TSubject, TResult> //
			>, //
		IIs<TSubject, TResult, IPrintableBoundSpecification<TSubject, TResult>> //
		>,
		IFluentBoundSpecificationBuilder<TSubject, TResult>
	{
		private readonly Lazy<Func<TSubject, TResult>> _instrument;

		public PrintableBoundSpecificationBuilder(IPrintableSource<TSubject> source,
			Func<Func<TSubject, TResult>> extractor)
			: this(source, new Lazy<Func<TSubject, TResult>>(extractor)) {}

		public PrintableBoundSpecificationBuilder(IPrintableSource<TSubject> source,
			Expression<Func<TSubject, TResult>> expression)
			: this(source, expression.Compile) {}

		public PrintableBoundSpecificationBuilder([NotNull] IPrintableSource<TSubject> source,
			[NotNull] Lazy<Func<TSubject, TResult>> instrument)
			: base(source)
		{
			_instrument = instrument.ValidateArgumentIsNotNull();
		}

		protected override IHas<TSubject, TResult, IPrintableBoundSpecification<TSubject, TResult>> MakeHas()
		{
			return new Has<TSubject, TResult, IPrintableBoundSpecification<TSubject, TResult>>(_instrument);
		}

		protected override INegatableIs< //
			TSubject, //
			TResult, //
			IIs<TSubject, TResult, IPrintableBoundSpecification<TSubject, TResult>>, //
			IPrintableBoundSpecification<TSubject, TResult>> MakeIs()
		{
			return new PrintableBoundIs<TSubject, TResult>(Negated.False, _instrument, Source);
		}
	}
}
