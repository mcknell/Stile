#region License info...
// Propter for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Propter project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
#endregion

namespace Stile.Prototypes.Specifications
{
    public interface ISource {}

    public interface ISource<out TSubject> : ISource
    {
        [CanBeNull]
        TSubject Get();
    }
}
