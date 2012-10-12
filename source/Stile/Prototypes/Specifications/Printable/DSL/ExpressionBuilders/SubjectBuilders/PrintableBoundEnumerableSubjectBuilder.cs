#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.SubjectBuilders;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.Sources;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SubjectBuilders
{
	public interface IPrintableBoundEnumerableSubjectBuilder : IBoundEnumerableSubjectBuilder,
		IPrintableBoundSubjectBuilder {}

	public interface IPrintableBoundEnumerableSubjectBuilder<TSubject, TItem> : IPrintableBoundEnumerableSubjectBuilder,
		IBoundEnumerableSubjectBuilder<TSubject, TItem>,
		IPrintableBoundSubjectBuilder<TSubject>
		where TSubject : class, IEnumerable<TItem> {}

	public class PrintableBoundEnumerableSubjectBuilder<TSubject, TItem> :
		BoundEnumerableSubjectBuilder<TSubject, TItem, IPrintableSource<TSubject>>,
		IPrintableBoundEnumerableSubjectBuilder<TSubject, TItem>,
		IPrintableBoundEnumerableSubjectBuilderState
			<TSubject, TItem, PrintableBoundEnumerableSubjectBuilder<TSubject, TItem>>
		where TSubject : class, IEnumerable<TItem>
	{
		public PrintableBoundEnumerableSubjectBuilder([NotNull] IPrintableSource<TSubject> source)
			: base(source) {}

		public Lazy<string> SubjectDescription
		{
			get { return Source.Description; }
		}

		public PrintableBoundEnumerableSubjectBuilder<TSubject, TItem> Make(Lazy<string> description)
		{
			var source = new PrintableSource<TSubject>(() => Source.Get, description);
			return new PrintableBoundEnumerableSubjectBuilder<TSubject, TItem>(source);
		}
	}

	public interface IPrintableBoundEnumerableSubjectBuilderState : IBoundEnumerableSubjectBuilderState,
		IPrintableSubjectBuilderState,
		IPrintableBoundSubjectBuilderState {}

	public interface IPrintableBoundEnumerableSubjectBuilderState<out TSubject, out TItem, out TBuilder> :
		IPrintableBoundEnumerableSubjectBuilderState,
		IBoundEnumerableSubjectBuilderState<TSubject, TItem, IPrintableSource<TSubject>>,
		IPrintableSubjectBuilderState<TBuilder>,
		IPrintableBoundSubjectBuilderState<TSubject>
		where TSubject : class, IEnumerable<TItem>
		where TBuilder : class, IPrintableBoundEnumerableSubjectBuilder<TSubject, TItem> {}
}
