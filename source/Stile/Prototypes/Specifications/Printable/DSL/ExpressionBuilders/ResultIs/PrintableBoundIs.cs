#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultIs;
using Stile.Prototypes.Specifications.DSL.SemanticModel;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.Sources;
using Stile.Prototypes.Specifications.Printable.Output.Explainers;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.ResultIs
{
	public interface IPrintableBoundIsState : IPrintableIsState {}

	public interface IPrintableBoundIsState<TSubject, TResult> : IPrintableBoundIsState,
		IPrintableIsState<TSubject, TResult> {}

	public interface IPrintableBoundIsState<TSubject, TResult, out TSpecifies, in TInput> :
		IPrintableBoundIsState<TSubject, TResult>,
		IPrintableIsState<TSubject, TResult, TSpecifies, TInput>
		where TSpecifies : class, ISpecification<TSubject, TResult>
		where TInput : class, IPrintableSpecificationInput<TSubject, TResult> {}

	public class PrintableBoundIs<TSubject, TResult> :
		BoundIs
			<TSubject, TResult, IIs<TSubject, TResult, IPrintableBoundSpecification<TSubject, TResult>>,
				IPrintableBoundSpecification<TSubject, TResult>, IPrintableSource<TSubject>,
				IPrintableSpecificationInput<TSubject, TResult>>,
		IPrintableBoundIsState
			<TSubject, TResult, IPrintableBoundSpecification<TSubject, TResult>,
				IPrintableSpecificationInput<TSubject, TResult>>,
		IFluentIsState<TSubject, TResult, IPrintableBoundSpecification<TSubject, TResult>>
	{
		public PrintableBoundIs(Negated negated,
			[NotNull] Lazy<Func<TSubject, TResult>> instrument,
			[NotNull] IPrintableSource<TSubject> source)
			: base(negated, instrument, source) {}

		public override IPrintableBoundSpecification<TSubject, TResult> Make(
			IPrintableSpecificationInput<TSubject, TResult> input)
		{
			return new PrintableBoundSpecification<TSubject, TResult>(Source, input);
		}

		public IPrintableBoundSpecification<TSubject, TResult> Make(Predicate<TResult> accepter,
			IExplainer<TSubject, TResult> explainer)
		{
			return new PrintableBoundSpecification<TSubject, TResult>(Source, Instrument, accepter, explainer);
		}

		protected override IIs<TSubject, TResult, IPrintableBoundSpecification<TSubject, TResult>> BoundFactory(
			Negated negated, Lazy<Func<TSubject, TResult>> instrument, IPrintableSource<TSubject> source)
		{
			return new PrintableBoundIs<TSubject, TResult>(negated, instrument, source);
		}
	}
}
