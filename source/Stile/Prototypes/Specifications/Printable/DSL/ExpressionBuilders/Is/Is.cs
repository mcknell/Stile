#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Patterns.SelfDescribingPredicates;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.Is
{
    public interface IIs<out TSubject> {}

    public interface IIs<out TSubject, out TResult> : IIs<TResult> {}

    public abstract class IsBase<TSubject, TNegated> : INegatableIs<TSubject, TNegated>,
        IIsState<TSubject>
        where TNegated : class, IIs<TSubject>
    {
        protected IsBase(Negated negated)
        {
            Negated = negated;
        }

        public Negated Negated { get; private set; }
        public TNegated Not
        {
            get { return Factory(); }
        }

        [NotNull]
        protected abstract TNegated Factory();
    }

    public class Is<TSubject, TNegated> : IsBase<TSubject, TNegated>
        where TNegated : class, IIs<TSubject>
    {
        private readonly Func<IIsState<TSubject>, TNegated> _factory;

        public Is(Negated negated, [NotNull] Func<IIsState<TSubject>, TNegated> factory)
            : base(negated)
        {
            _factory = factory.ValidateArgumentIsNotNull();
        }

        protected override TNegated Factory()
        {
            return _factory.Invoke(this);
        }
    }

    public class Is<TSubject, TResult, TNegated> : IsBase<TResult, TNegated>,
        INegatableIs<TSubject, TResult, TNegated>,
        IIsState<TSubject, TResult>
        where TNegated : class, IIs<TSubject, TResult>
    {
        private readonly Func<IIsState<TSubject, TResult>, TNegated> _factory;

        public Is(Negated negated, [NotNull] Func<IIsState<TSubject, TResult>, TNegated> factory,
            [NotNull] Lazy<Func<TSubject, TResult>> extractor)
            : base(negated)
        {
            _factory = factory.ValidateArgumentIsNotNull();
            Extractor = extractor.ValidateArgumentIsNotNull();
        }

        public Lazy<Func<TSubject, TResult>> Extractor { get; private set; }

        protected override TNegated Factory()
        {
            return _factory.Invoke(this);
        }
    }
}
