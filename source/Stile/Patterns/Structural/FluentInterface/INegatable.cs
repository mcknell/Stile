#region License info...
// Propter for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Propter project on GitHub
#endregion

namespace Stile.Patterns.Structural.FluentInterface
{
    public interface INegatable {}

    public interface INegatable<out TReturn> : INegatable
    {
        TReturn Not { get; }
    }
}
