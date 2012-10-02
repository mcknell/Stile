#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.SemanticModel.Specifications
{
    public interface ISingularSpecification<out TSubject> : ISpecification {}

    public class SingularSpecification<TSubject> : ISingularSpecification<TSubject> {}
}
