#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.SubjectBuilders;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.Sources;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SpecificationBuilders;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SubjectBuilders;
#endregion

namespace Stile.Prototypes.Specifications.Printable.Construction
{
	public static class Specify
	{
		[Pure]
		public static IBoundSubjectBuilder<TSubject> For<TSubject>(Expression<Func<TSubject>> expression)
		{
			return new BoundSubjectBuilder<TSubject>(new PrintableSource<TSubject>(expression));
		}

		[Pure]
		public static IPrintableBoundSubjectBuilder<TSubject> For<TSubject>(TSubject subject) where TSubject : class
		{
			return new PrintableBoundSubjectBuilder<TSubject>(new PrintableSource<TSubject>(subject));
		}

		[Pure]
		public static IPrintableBoundEnumerableSubjectBuilder<TSubject, TItem> For<TSubject, TItem>(TSubject subject)
			where TSubject : class, IEnumerable<TItem>
		{
			var source = new PrintableSource<TSubject>(subject);
			return new PrintableBoundEnumerableSubjectBuilder<TSubject, TItem>(source);
		}

		[Pure]
		public static IPrintableSubjectBuilder<TSubject> ForAny<TSubject>()
		{
			return new PrintableSubjectBuilder<TSubject>();
		}

		[Pure]
		public static IEnumerableSubjectBuilder<TSubject, TItem> ForAny<TSubject, TItem>()
			where TSubject : class, IEnumerable<TItem>
		{
			return new EnumerableSubjectBuilder<TSubject, TItem>();
		}

		[Pure]
		public static IFluentSpecificationBuilder<TSubject, TSubject> ThatAny<TSubject>()
		{
			return new PrintableSpecificationBuilder<TSubject, TSubject>(x => x);
		}
	}
}
