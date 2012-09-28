#region License info...
// Propter for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Stile.Types.Enumerables;
#endregion

namespace Stile.Prototypes.Specifications.Evaluations
{
    public interface IWrappedResult
    {
        IReadOnlyCollection<IError> Errors { get; }
        Outcome Outcome { get; }
        object Value { get; }
    }

    public interface IWrappedResult<out TSubject, out TValue> : IWrappedResult
    {
        TSubject Subject { get; }
        new TValue Value { get; }
    }

    public class WrappedResult<TSubject, TValue> : IWrappedResult<TSubject, TValue>
    {
        public WrappedResult(TSubject subject, Outcome outcome, TValue value, [NotNull] Exception e, params Exception[] errors)
            : this(subject, outcome, value, errors.Unshift(e).Select(x => (IError) new Error(x)).ToArray()) {}

        public WrappedResult(TSubject subject, Outcome outcome, TValue value, params IError[] errors)
        {
            Subject = subject;
            Outcome = outcome;
            Value = value;
            Errors = errors;
        }

        public IReadOnlyCollection<IError> Errors { get; private set; }
        public Outcome Outcome { get; private set; }
        public TSubject Subject { get; set; }
        public TValue Value { get; private set; }
        object IWrappedResult.Value
        {
            get { return Value; }
        }
    }
}
