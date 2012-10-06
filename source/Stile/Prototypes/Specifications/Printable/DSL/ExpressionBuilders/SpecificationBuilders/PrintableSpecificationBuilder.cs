#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.SpecificationBuilders;
using Stile.Prototypes.Specifications.DSL.SemanticModel;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.ResultHas;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.ResultIs;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SubjectBuilders;
using Stile.Readability;
using Stile.Types.Expressions;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SpecificationBuilders
{
    public interface IPrintableSpecificationBuilder : ISpecificationBuilder {}

    public interface IPrintableSpecificationBuilder<TSubject, out TResult, out THas, out TNegatableIs, out TIs> :
        IPrintableSpecificationBuilder,
        ISpecificationBuilder
            <TSubject, TResult, THas, TNegatableIs, TIs, IPrintableSpecification<TSubject, TResult>,
                IPrintableEvaluation<TResult>>
        where THas : class, IPrintableHas<TResult, TSubject>
        where TNegatableIs : class, IPrintableNegatableIs<TSubject, TResult, TIs>
        where TIs : class, IPrintableIs<TSubject, TResult> {}

    public abstract class PrintableSpecificationBuilder<TSubject, TResult, THas, TNegatableIs, TIs> :
        SpecificationBuilder
            <TSubject, TResult, THas, TNegatableIs, TIs, IPrintableSpecification<TSubject, TResult>,
                IPrintableEvaluation<TResult>>,
        IPrintableSpecificationBuilder<TSubject, TResult, THas, TNegatableIs, TIs>,
        IPrintableSpecificationBuilderState
        where THas : class, IPrintableHas<TResult, TSubject>
        where TNegatableIs : class, IPrintableNegatableIs<TSubject, TResult, TIs>
        where TIs : class, IPrintableIs<TSubject, TResult>
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
        IPrintableHas<TResult, TSubject>, //
        IPrintableNegatableIs<TSubject, TResult, IPrintableIs<TSubject, TResult>>, //
        IPrintableIs<TSubject, TResult>>,
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

        protected override IPrintableHas<TResult, TSubject> MakeHas()
        {
            return new PrintableHas<TResult, TSubject>(_instrument, this);
        }

        protected override IPrintableNegatableIs<TSubject, TResult, IPrintableIs<TSubject, TResult>> MakeIs()
        {
            return new PrintableIs<TSubject, TResult>(Negated.False, _instrument);
        }
    }

    public class PrintableSpecificationBuilder<TSubject> : PrintableSpecificationBuilder<TSubject, TSubject>,
        IFluentSpecificationBuilder<TSubject>
    {
// ReSharper disable StaticFieldInGenericType
// ReSharper disable InconsistentNaming
        private static readonly Lazy<string> sSubjectDescription = typeof(TSubject).ToLazyDebugString();
// ReSharper restore InconsistentNaming
// ReSharper restore StaticFieldInGenericType

        public PrintableSpecificationBuilder(ISubjectBuilderState<TSubject> state = null)
            : base(Instrument.Trivial<TSubject>.Map, state == null ? sSubjectDescription : state.SubjectDescription) {}
    }
}
