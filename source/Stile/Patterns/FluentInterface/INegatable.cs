#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

namespace Stile.Patterns.FluentInterface
{
    public interface INegatable {}

    public interface INegatable<out TReturn> : INegatable
    {
        TReturn Not { get; }
    }
}
