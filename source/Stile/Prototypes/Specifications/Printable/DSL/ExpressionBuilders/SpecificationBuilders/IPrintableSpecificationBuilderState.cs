#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.SpecificationBuilders;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SpecificationBuilders
{
    public interface IPrintableSpecificationBuilderState : ISpecificationBuilderState
    {
        [NotNull]
        Lazy<string> SubjectDescription { get; }
    }
}
