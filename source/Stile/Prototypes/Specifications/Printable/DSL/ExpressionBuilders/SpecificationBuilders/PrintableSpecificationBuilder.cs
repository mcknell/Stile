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
using Stile.Prototypes.Specifications.Printable.Output;
using Stile.Types.Expressions;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SpecificationBuilders
{
	public interface IPrintableSpecificationBuilder : ISpecificationBuilder {}

	public interface IPrintableSpecificationBuilder<out TSubject, out TResult, out THas, out TNegatableIs, out TIs,
		out TSpecifies, out TEvaluation> : IPrintableSpecificationBuilder,
			ISpecificationBuilder<TSubject, TResult, THas, TNegatableIs, TIs, TSpecifies>
		where THas : class, IHas<TSubject, TResult, TSpecifies>
		where TNegatableIs : class, INegatableIs<TSubject, TResult, TIs, TSpecifies>
		where TIs : class, IIs<TSubject, TResult, TSpecifies>
		where TSpecifies : class, IPrintableSpecification<TSubject, TResult, TEvaluation, ILazyReadableText>
		where TEvaluation : class, IPrintableEvaluation<TResult, ILazyReadableText> {}

	public interface IFluentSpecificationBuilder<TSubject, out TResult> : IPrintableSpecificationBuilder< //
		TSubject, //
		TResult, //
		IHas<TSubject, TResult, IFluentSpecification<TSubject, TResult>>, //
		INegatableIs< //
			TSubject, //
			TResult, //
			IIs<TSubject, TResult, IFluentSpecification<TSubject, TResult>>, //
			IFluentSpecification<TSubject, TResult> //
			>, //
		IIs<TSubject, TResult, IFluentSpecification<TSubject, TResult>>, //
		IFluentSpecification<TSubject, TResult>, //
		IPrintableEvaluation<TResult, ILazyReadableText>> {}

	public interface IPrintableSpecificationBuilderState : ISpecificationBuilderState
	{
		[NotNull]
		Lazy<string> SubjectDescription { get; }
	}

	public abstract class PrintableSpecificationBuilder<TSubject, TResult, THas, TNegatableIs, TIs, TSpecifies,
		TEvaluation> : SpecificationBuilder<TSubject, TResult, THas, TNegatableIs, TIs, TSpecifies>,
			IPrintableSpecificationBuilder<TSubject, TResult, THas, TNegatableIs, TIs, TSpecifies, TEvaluation>,
			IPrintableSpecificationBuilderState
		where THas : class, IHas<TSubject, TResult, TSpecifies>
		where TNegatableIs : class, INegatableIs<TSubject, TResult, TIs, TSpecifies>
		where TIs : class, IIs<TSubject, TResult, TSpecifies>
		where TSpecifies : class, IPrintableSpecification<TSubject, TResult, TEvaluation, ILazyReadableText>
		where TEvaluation : class, IPrintableEvaluation<TResult, ILazyReadableText>
	{
		protected PrintableSpecificationBuilder([NotNull] Lazy<string> subjectDescription)
		{
			SubjectDescription = subjectDescription.ValidateArgumentIsNotNull();
		}

		public Lazy<string> SubjectDescription { get; private set; }
	}

	public class PrintableSpecificationBuilder<TSubject, TResult> : PrintableSpecificationBuilder< //
		TSubject, //
		TResult, //
		IHas<TSubject, TResult, IFluentSpecification<TSubject, TResult>>, //
		INegatableIs< //
			TSubject, //
			TResult, //
			IIs<TSubject, TResult, IFluentSpecification<TSubject, TResult>>, //
			IFluentSpecification<TSubject, TResult>>, //
		IIs<TSubject, TResult, IFluentSpecification<TSubject, TResult>>, //
		IFluentSpecification<TSubject, TResult>, //
		IPrintableEvaluation<TResult>>,
		IFluentSpecificationBuilder<TSubject, TResult>
	{
		private readonly Lazy<Func<TSubject, TResult>> _instrument;

		public PrintableSpecificationBuilder([NotNull] Expression<Func<TSubject, TResult>> expression)
			: this(expression.Compile, expression.ToLazyDebugString()) {}

		protected PrintableSpecificationBuilder([NotNull] Func<Func<TSubject, TResult>> extractor,
			[NotNull] Lazy<string> subjectDescription)
			: this(new Lazy<Func<TSubject, TResult>>(extractor), subjectDescription) {}

		protected PrintableSpecificationBuilder([NotNull] Lazy<Func<TSubject, TResult>> instrument,
			[NotNull] Lazy<string> subjectDescription)
			: base(subjectDescription)
		{
			_instrument = instrument.ValidateArgumentIsNotNull();
		}

		protected override IHas<TSubject, TResult, IFluentSpecification<TSubject, TResult>> MakeHas()
		{
			return new Has<TSubject, TResult, IFluentSpecification<TSubject, TResult>>(_instrument);
		}

		protected override INegatableIs< //
			TSubject, //
			TResult, //
			IIs<TSubject, TResult, IFluentSpecification<TSubject, TResult>>, //
			IFluentSpecification<TSubject, TResult>> MakeIs()
		{
			return new PrintableIs<TSubject, TResult>(Negated.False, _instrument);
		}
	}
}
