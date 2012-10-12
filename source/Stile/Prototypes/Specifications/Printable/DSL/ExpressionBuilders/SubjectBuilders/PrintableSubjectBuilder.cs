#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.SubjectBuilders;
using Stile.Readability;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SubjectBuilders
{
	public interface IPrintableSubjectBuilder : ISubjectBuilder {}

	public interface IPrintableSubjectBuilder<TSubject> : IPrintableSubjectBuilder,
		ISubjectBuilder<TSubject> {}

	public interface IPrintableSubjectBuilderState : ISubjectBuilderState
	{
		[NotNull]
		Lazy<string> SubjectDescription { get; }
	}

	public interface IPrintableSubjectBuilderState<out TBuilder> : IPrintableSubjectBuilderState
		where TBuilder : class, IPrintableSubjectBuilder
	{
		[NotNull]
		TBuilder Make([NotNull] Lazy<string> description);
	}

	public class PrintableSubjectBuilder<TSubject> : IPrintableSubjectBuilder<TSubject>,
		IPrintableSubjectBuilderState<IPrintableSubjectBuilder<TSubject>>
	{
// ReSharper disable StaticFieldInGenericType
// ReSharper disable InconsistentNaming
		private static readonly Lazy<string> sSubjectDescription = typeof(TSubject).ToLazyDebugString();
// ReSharper restore InconsistentNaming
// ReSharper restore StaticFieldInGenericType

		public PrintableSubjectBuilder()
			: this(sSubjectDescription) {}

		public PrintableSubjectBuilder([NotNull] Lazy<string> subjectDescription)
		{
			SubjectDescription = subjectDescription.ValidateArgumentIsNotNull();
		}

		public Lazy<string> SubjectDescription { get; private set; }

		public IPrintableSubjectBuilder<TSubject> Make(Lazy<string> description)
		{
			return new PrintableSubjectBuilder<TSubject>(description);
		}
	}
}
