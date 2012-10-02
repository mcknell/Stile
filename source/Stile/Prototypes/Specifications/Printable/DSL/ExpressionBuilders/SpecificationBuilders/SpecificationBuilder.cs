#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Patterns.SelfDescribingPredicates;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.Has;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.Is;
using Stile.Types.Expressions;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SpecificationBuilders
{
    public interface ISpecificationBuilder<out TSubject, out THas, out TNegatableIs, out TIs>
        where THas : class, IHas<TSubject>
        where TNegatableIs : class, INegatableIs<TSubject, TIs>
        where TIs : class, IIs<TSubject>
    {
        THas Has { get; }
        TNegatableIs Is { get; }
    }

    public interface ISpecificationBuilder<out TSubject, out THas, out TNegatableIs, out TIs, out TResult> :
        ISpecificationBuilder<TResult, THas, TNegatableIs, TIs>
        where THas : class, IHas<TResult>
        where TNegatableIs : class, INegatableIs<TResult, TIs>
        where TIs : class, IIs<TSubject, TResult> {}

    public interface ISpecificationBuilder<out TSubject> :
        ISpecificationBuilder<TSubject, IHas<TSubject>, INegatableIs<TSubject, IIs<TSubject>>, IIs<TSubject>> {}

    public interface ISpecificationBuilder<out TSubject, out TResult> :
        ISpecificationBuilder
            <TSubject, IHas<TSubject, TResult>, INegatableIs<TSubject, TResult, IIs<TSubject, TResult>>, IIs<TSubject, TResult>,
                TResult> {}

    public abstract class SpecificationBuilderBase<TSubject, THas, TNegatableIs, TIs> :
        ISpecificationBuilder<TSubject, THas, TNegatableIs, TIs>
        where THas : class, IHas<TSubject>
        where TNegatableIs : class, INegatableIs<TSubject, TIs>
        where TIs : class, IIs<TSubject>
    {
        private readonly Lazy<THas> _lazyHas;
        private readonly Lazy<TNegatableIs> _lazyIs;

        protected SpecificationBuilderBase()
        {
            _lazyHas = new Lazy<THas>(MakeHas);
            _lazyIs = new Lazy<TNegatableIs>(MakeIs);
        }

        public THas Has
        {
            get
            {
                THas value = _lazyHas.Value;
                return value;
            }
        }
        public TNegatableIs Is
        {
            get
            {
                TNegatableIs value = _lazyIs.Value;
                return value;
            }
        }

        protected abstract THas MakeHas();
        protected abstract TNegatableIs MakeIs();
    }

    public class SpecificationBuilder<TSubject, TResult> :
        SpecificationBuilderBase
            <TResult, IHas<TSubject, TResult>, INegatableIs<TSubject, TResult, IIs<TSubject, TResult>>, IIs<TSubject, TResult>>,
        ISpecificationBuilder<TSubject, TResult>
    {
        private readonly Lazy<Func<TSubject, TResult>> _extractor;
        private readonly Lazy<string> _subjectDescription;

        public SpecificationBuilder(Expression<Func<TSubject, TResult>> expression)
            : this(expression.Compile, expression.ToLazyDebugString()) {}

        protected SpecificationBuilder([NotNull] Func<Func<TSubject, TResult>> extractor, [NotNull] Lazy<string> subjectDescription)
            : this(new Lazy<Func<TSubject, TResult>>(extractor), subjectDescription) {}

        protected SpecificationBuilder([NotNull] Lazy<Func<TSubject, TResult>> extractor, [NotNull] Lazy<string> subjectDescription)
        {
            _subjectDescription = subjectDescription;
            _extractor = extractor.ValidateArgumentIsNotNull();
        }

        public new IHas<TResult> Has
        {
            get { return base.Has; }
        }

        protected static INegatableIs<TSubject, TResult, IIs<TSubject, TResult>> Factory(IIsState<TSubject, TResult> state)
        {
            return new Is<TSubject, TResult, INegatableIs<TSubject, TResult, IIs<TSubject, TResult>>>(state.Negated.Invert(),
                Factory,
                state.Extractor);
        }

        protected override IHas<TSubject, TResult> MakeHas()
        {
            return new Has<TSubject, TResult>(_extractor, _subjectDescription);
        }

        protected override INegatableIs<TSubject, TResult, IIs<TSubject, TResult>> MakeIs()
        {
            return new Is<TSubject, TResult, IIs<TSubject, TResult>>(Negated.False, Factory, _extractor);
        }
    }

    public class SpecificationBuilder<TSubject> :
        SpecificationBuilderBase<TSubject, IHas<TSubject>, INegatableIs<TSubject, IIs<TSubject>>, IIs<TSubject>>,
        ISpecificationBuilder<TSubject>,
        ISpecificationBuilderState<TSubject>
    {
        private readonly Lazy<string> _subjectDescription;

        public SpecificationBuilder([NotNull] Lazy<string> subjectDescription)
        {
            _subjectDescription = subjectDescription.ValidateArgumentIsNotNull();
        }

        protected IIs<TSubject> Factory(IIsState<TSubject> state)
        {
            return new Is<TSubject, IIs<TSubject>>(state.Negated.Invert(), Factory);
        }

        protected override IHas<TSubject> MakeHas()
        {
            return new Has<TSubject>(_subjectDescription);
        }

        protected override INegatableIs<TSubject, IIs<TSubject>> MakeIs()
        {
            return new Is<TSubject, IIs<TSubject>>(Negated.False, Factory);
        }
    }
}
