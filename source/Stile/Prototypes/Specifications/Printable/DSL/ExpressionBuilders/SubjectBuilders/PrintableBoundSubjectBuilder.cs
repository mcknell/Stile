#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.SubjectBuilders;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.Sources;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SubjectBuilders
{
	public interface IPrintableBoundSubjectBuilder : IPrintableSubjectBuilder,
		IBoundSubjectBuilder {}

	public interface IPrintableBoundSubjectBuilder<TSubject> : IPrintableBoundSubjectBuilder,
		IPrintableSubjectBuilder<TSubject>,
		IBoundSubjectBuilder<TSubject> {}

	public class PrintableBoundSubjectBuilder<TSubject> : BoundSubjectBuilder<TSubject, IPrintableSource<TSubject>>,
		IPrintableBoundSubjectBuilder<TSubject>,
		IPrintableBoundSubjectBuilderState<TSubject>
	{
		public PrintableBoundSubjectBuilder([NotNull] IPrintableSource<TSubject> source)
			: base(source) {}
	}

	public interface IPrintableBoundSubjectBuilderState : IBoundSubjectBuilderState {}

	public interface IPrintableBoundSubjectBuilderState<out TSubject> : IPrintableBoundSubjectBuilderState,
		IBoundSubjectBuilderState<TSubject, IPrintableSource<TSubject>> {}
}
