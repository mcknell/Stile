#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.SpecificationBuilders;
using Stile.Prototypes.Specifications.Printable.Output.Explainers;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.ResultIs
{
	public interface IPrintableSpecificationInput : ISpecificationInput {}

	public interface IPrintableSpecificationInput<TSubject, TResult> : IPrintableSpecificationInput,
		ISpecificationInput<TSubject, TResult>
	{
		[NotNull]
		IExplainer<TSubject, TResult> Explainer { get; }
	}

	public class PrintableSpecificationInput<TSubject, TResult> : SpecificationInput<TSubject, TResult>,
		IPrintableSpecificationInput<TSubject, TResult>
	{
		public PrintableSpecificationInput(Predicate<TResult> accepter,
			[NotNull] Lazy<Func<TSubject, TResult>> lazyInstrument,
			IExplainer<TSubject, TResult> explainer)
			: base(accepter, lazyInstrument)
		{
			Explainer = explainer.ValidateArgumentIsNotNull();
		}

		public IExplainer<TSubject, TResult> Explainer { get; private set; }
	}
}
