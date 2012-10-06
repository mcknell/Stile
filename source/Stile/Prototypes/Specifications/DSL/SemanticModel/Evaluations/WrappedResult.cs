#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Stile.Types.Enumerables;
#endregion

namespace Stile.Prototypes.Specifications.DSL.SemanticModel.Evaluations
{
    public interface IWrappedResult
    {
        IReadOnlyCollection<IError> Errors { get; }
        Outcome Outcome { get; }
        object Value { get; }
    }

    public interface IWrappedResult<out TValue> : IWrappedResult
    {
        new TValue Value { get; }
    }

    public class WrappedResult<TValue> : IWrappedResult<TValue>
    {
        public WrappedResult(Outcome outcome, TValue value, [NotNull] Exception e, params Exception[] errors)
            : this(outcome, value, errors.Unshift(e).Select(x => (IError) new Error(x)).ToArray()) {}

        public WrappedResult(Outcome outcome, TValue value, params IError[] errors)
        {
            Outcome = outcome;
            Value = value;
            Errors = errors;
        }

        public IReadOnlyCollection<IError> Errors { get; private set; }
        public Outcome Outcome { get; private set; }
        public TValue Value { get; private set; }
        object IWrappedResult.Value
        {
            get { return Value; }
        }
    }
}
