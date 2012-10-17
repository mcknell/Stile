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
	public interface IPrintableIs : IIs {}

	public interface IPrintableIs<out TResult, out TSpecifies> : IPrintableIs,
		IIs<TResult, TSpecifies>
		where TSpecifies : class, ISpecification {}

	public interface IPrintableIs<out TSubject, out TResult, out TSpecifies> : IPrintableIs,
		IIs<TSubject,TResult, TSpecifies>
		where TSpecifies : class, ISpecification<TSubject, TResult> {}

	public interface IPrintableIsState : IIsState {}

	public interface IPrintableIsState<TSubject, TResult> : IPrintableIsState,
		IIsState<TSubject, TResult> {}

	public interface IPrintableIsState<TSubject, TResult, out TSpecifies> : IPrintableIsState<TSubject, TResult>
		where TSpecifies : class, ISpecification<TSubject, TResult>
	{
		TSpecifies Make(Predicate<TResult> accepter, IExplainer<TSubject, TResult> explainer);
	}

	public interface IFluentIsState<TSubject, TResult, out TSpecifies> :
		IPrintableIsState<TSubject, TResult, TSpecifies>
		where TSpecifies : class, ISpecification<TSubject, TResult> {}

	public abstract class PrintableIs<TSubject, TResult, TSource, TSpecifies> :
		Is<TSubject, TResult, TSource, IIs<TSubject, TResult, TSpecifies>, TSpecifies>,
		IPrintableIsState<TSubject, TResult, TSpecifies>
		where TSource : class, IPrintableSource<TSubject>
		where TSpecifies : class, ISpecification<TSubject, TResult>
	{
		protected PrintableIs([NotNull] TSource source, Negated negated, [NotNull] Lazy<Func<TSubject, TResult>> instrument)
			: base(source, negated, instrument) {}

		public abstract TSpecifies Make(Predicate<TResult> accepter, IExplainer<TSubject, TResult> explainer);
	}

	public class PrintableIs<TSubject, TResult> :
		PrintableIs
			<TSubject, TResult, IPrintableSource<TSubject>, IFluentBoundSpecification<TSubject, TResult>>,
		IFluentIsState<TSubject, TResult, IFluentBoundSpecification<TSubject, TResult>>
	{
		public PrintableIs([NotNull] IPrintableSource<TSubject> source,
			Negated negated,
			[NotNull] Lazy<Func<TSubject, TResult>> instrument)
			: base(source, negated, instrument) {}

		public override IFluentBoundSpecification<TSubject, TResult> Make(Predicate<TResult> accepter,
			IExplainer<TSubject, TResult> explainer)
		{
			return Make(Source, Instrument, accepter, explainer);
		}

		protected override IIs<TSubject, TResult, IFluentBoundSpecification<TSubject, TResult>> Factory(Negated negated,
			Lazy<Func<TSubject, TResult>> instrument,
			IPrintableSource<TSubject> source)
		{
			return new PrintableIs<TSubject, TResult>(source, negated, instrument);
		}

		private static PrintableSpecification<TSubject, TResult> Make(IPrintableSource<TSubject> source,
			Lazy<Func<TSubject, TResult>> instrument,
			Predicate<TResult> accepter,
			IExplainer<TSubject, TResult> explainer)
		{
			return new PrintableSpecification<TSubject, TResult>(source, instrument, accepter, explainer);
		}
	}
}
