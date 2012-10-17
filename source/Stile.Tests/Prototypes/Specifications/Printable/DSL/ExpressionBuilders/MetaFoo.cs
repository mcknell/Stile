#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

namespace Stile.Tests.Prototypes.Specifications.Printable.DSL.ExpressionBuilders
{
	public interface IMetaFoo<out T>
	{
		IFoo<T> Foo { get; }
	}

	public class MetaFoo<T> : IMetaFoo<T>
	{
		public MetaFoo(IFoo<T> foo = null)
		{
			Foo = foo ?? new Foo<T>();
		}

		public IFoo<T> Foo { get; private set; }
	}
}
