using Stile.Prototypes.Specifications.Printable.Construction;

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SubjectBuilders
{
    /// <summary>
    /// Intended purely as a hack for type inference in a fluent interface, e.g., 
    /// <see cref="Specify"/>.For(fooCollection).Containing(<see cref="Items"/>.OfType&lt;&gt;())
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITypeSpecifier<T> {}
}