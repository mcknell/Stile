#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultIs;
using Stile.Prototypes.Specifications.DSL.SemanticModel;
using Stile.Prototypes.Specifications.Printable.Output.Explainers;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.ResultIs
{
	public interface IPrintableIsState : IIsState {}

	public interface IPrintableIsState<TSubject, TResult> : IPrintableIsState,
		IIsState<TSubject, TResult> {}

	public interface IPrintableIsState<TSubject, TResult, out TSpecifies> : IPrintableIsState<TSubject, TResult>
		where TSpecifies : class, ISpecification<TSubject, TResult>
	{
		TSpecifies Make(Predicate<TResult> accepter, IExplainer<TSubject, TResult> explainer);
	}

	public interface IPrintableIsState<TSubject, TResult, out TSpecifies, in TInput> :
		IPrintableIsState<TSubject, TResult, TSpecifies>,
		IIsState<TSubject, TResult, TSpecifies, TInput>
		where TSpecifies : class, ISpecification<TSubject, TResult>
		where TInput : class, IPrintableSpecificationInput<TSubject, TResult> {}

	public interface IFluentIsState<TSubject, TResult, out TSpecifies> :
		IPrintableIsState<TSubject, TResult, TSpecifies, IPrintableSpecificationInput<TSubject, TResult>>
		where TSpecifies : class, ISpecification<TSubject, TResult> {}

	public abstract class PrintableIs<TSubject, TResult, TSpecifies, TInput> :
		Is<TSubject, TResult, IIs<TSubject, TResult, TSpecifies>, TSpecifies, TInput>,
		IPrintableIsState<TSubject, TResult, TSpecifies, TInput>
		where TSpecifies : class, ISpecification<TSubject, TResult>
		where TInput : class, IPrintableSpecificationInput<TSubject, TResult>
	{
		protected PrintableIs(Negated negated, [NotNull] Lazy<Func<TSubject, TResult>> instrument)
			: base(negated, instrument) {}

		public abstract TSpecifies Make(Predicate<TResult> accepter, IExplainer<TSubject, TResult> explainer);
	}

	public class PrintableIs<TSubject, TResult> :
		PrintableIs
			<TSubject, TResult, IFluentSpecification<TSubject, TResult>, IPrintableSpecificationInput<TSubject, TResult>>,
		IFluentIsState<TSubject, TResult, IFluentSpecification<TSubject, TResult>>
	{
		public PrintableIs(Negated negated, [NotNull] Lazy<Func<TSubject, TResult>> instrument)
			: base(negated, instrument) {}

		public override IFluentSpecification<TSubject, TResult> Make(Predicate<TResult> accepter,
			IExplainer<TSubject, TResult> explainer)
		{
			return Make(Instrument, accepter, explainer);
		}

		public override IFluentSpecification<TSubject, TResult> Make(IPrintableSpecificationInput<TSubject, TResult> input)
		{
			return Make(input.LazyInstrument, input.Accepter, input.Explainer);
		}

		protected override IIs<TSubject, TResult, IFluentSpecification<TSubject, TResult>> Factory(Negated negated,
			Lazy<Func<TSubject, TResult>> instrument)
		{
			return new PrintableIs<TSubject, TResult>(negated, instrument);
		}

		private static PrintableSpecification<TSubject, TResult> Make(Lazy<Func<TSubject, TResult>> instrument,
			Predicate<TResult> accepter,
			IExplainer<TSubject, TResult> explainer)
		{
			return new PrintableSpecification<TSubject, TResult>(instrument, accepter, explainer);
		}
	}
}
