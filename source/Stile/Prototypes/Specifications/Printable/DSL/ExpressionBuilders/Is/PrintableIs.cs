#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.Is;
using Stile.Prototypes.Specifications.DSL.SemanticModel;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.Is
{
    public interface IPrintableIs : IIs {}

    public interface IPrintableIs<out TResult> : IPrintableIs,
        IIs<TResult> {}

    public interface IPrintableIs<out TResult, TSubject> : IPrintableIs<TResult>,
        IIs<TResult, TSubject, IPrintableSpecification<TSubject, TResult>, IPrintableEvaluation<TResult>> {}

    public abstract class PrintableIs<TResult, TSubject, TNegated> :
        IPrintableNegatableIs<TResult, TNegated, TSubject>,
        IIsState<TSubject, TResult>
        where TNegated : class, IPrintableIs<TResult, TSubject>
    {
        protected PrintableIs(Negated negated, [NotNull] Lazy<Func<TSubject, TResult>> extractor)
        {
            Negated = negated;
            Instrument = extractor.ValidateArgumentIsNotNull();
        }

        public Lazy<Func<TSubject, TResult>> Instrument { get; private set; }
        public Negated Negated { get; private set; }
        public TNegated Not
        {
            get { return Factory(); }
        }

        [NotNull]
        protected abstract TNegated Factory();
    }

    public class PrintableIs<TSubject, TResult> : PrintableIs<TResult, TSubject, IPrintableIs<TResult, TSubject>>
    {
        private readonly Lazy<Func<TSubject, TResult>> _extractor;

        public PrintableIs(Negated negated, [NotNull] Lazy<Func<TSubject, TResult>> extractor)
            : base(negated, extractor)
        {
            _extractor = extractor;
        }

        protected override IPrintableIs<TResult, TSubject> Factory()
        {
            return new PrintableIs<TSubject, TResult>(Negated.True, _extractor);
        }
    }
}
