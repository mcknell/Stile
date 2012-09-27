#region License info...
// Propter for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Propter project on GitHub
#endregion

#region using...
using System;
#endregion

namespace Stile.Prototypes.Specifications.Printable.Construction.GrammarMetadata
{
    /// <summary>
    /// A production rule the fluent DSL for declaring <see cref="IPrintableSpecification"/> instances.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class SpecificationRuleAttribute : Attribute {}
}
