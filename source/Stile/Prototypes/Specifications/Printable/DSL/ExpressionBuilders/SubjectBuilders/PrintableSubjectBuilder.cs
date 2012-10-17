#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.SubjectBuilders;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.Sources;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SpecificationBuilders;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SubjectBuilders
{
	public interface IPrintableSubjectBuilder : ISubjectBuilder {}

	public interface IPrintableSubjectBuilder<TSubject> : IPrintableSubjectBuilder,
		ISubjectBuilder<TSubject> {}

	public interface IPrintableBoundSubjectBuilder : IPrintableSubjectBuilder,
		IBoundSubjectBuilder {}

	public interface IPrintableBoundSubjectBuilder<TSubject> : IPrintableBoundSubjectBuilder,
		IPrintableSubjectBuilder<TSubject>,
		IBoundSubjectBuilder<TSubject> {}

	public interface IPrintableSubjectBuilderState : ISubjectBuilderState {}

	public interface IPrintableSubjectBuilderState<TSubject> : IPrintableSubjectBuilderState
	{
		[NotNull]
		IPrintableSubjectBuilder<TSubject> Make([NotNull] Lazy<string> description);

		[NotNull]
		IFluentSpecificationBuilder<TSubject, TResult> Make<TResult>(
			[NotNull] Expression<Func<TSubject, TResult>> expression);

		[NotNull]
		IPrintableBoundSubjectBuilder<TSubject> MakeBound([NotNull] Lazy<string> description);

		[NotNull]
		IFluentBoundSpecificationBuilder<TSubject, TResult> MakeBound<TResult>(
			[NotNull] Expression<Func<TSubject, TResult>> expression);
	}

	public class PrintableSubjectBuilder<TSubject> : SubjectBuilder<TSubject, IPrintableSource<TSubject>>,
		IPrintableBoundSubjectBuilder<TSubject>,
		IPrintableSubjectBuilderState<TSubject>
	{
		public PrintableSubjectBuilder(IPrintableSource<TSubject> source = null)
			: base(source ?? PrintableSource<TSubject>.Empty) {}

		public IPrintableSubjectBuilder<TSubject> Make(Lazy<string> description)
		{
			return MakeBound(description);
		}

		public IFluentSpecificationBuilder<TSubject, TResult> Make<TResult>(Expression<Func<TSubject, TResult>> expression)
		{
			return new PrintableSpecificationBuilder<TSubject, TResult>(Source, expression);
		}

		public IPrintableBoundSubjectBuilder<TSubject> MakeBound(Lazy<string> description)
		{
			var source = new PrintableSource<TSubject>(() => Source.Get, description);
			return new PrintableSubjectBuilder<TSubject>(source);
		}

		public IFluentBoundSpecificationBuilder<TSubject, TResult> MakeBound<TResult>(
			Expression<Func<TSubject, TResult>> expression)
		{
			return new PrintableSpecificationBuilder<TSubject, TResult>(Source, expression);
		}
	}
}
