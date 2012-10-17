#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.Sources;
using Stile.Readability;
using Stile.Types.Expressions;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.Sources
{
	public interface IPrintableSource : ISource
	{
		[NotNull]
		Lazy<string> Description { get; }
	}

	public interface IPrintableSource<out TSubject> : IPrintableSource,
		ISource<TSubject> {}

	public class PrintableSource<TSubject> : Source<TSubject>,
		IPrintableSource<TSubject>
	{
		public static readonly PrintableSource<TSubject> Empty = new PrintableSource<TSubject>();

		public PrintableSource([NotNull] Expression<Func<TSubject>> expression, Lazy<string> description = null)
			: this(expression.Compile, description ?? expression.ToLazyDebugString()) {}

		public PrintableSource(TSubject subject, Lazy<string> description = null)
			: this(() => () => subject, description ?? subject.ToLazyDebugString()) {}

		public PrintableSource(Lazy<string> description = null)
			: this(() => () => default(TSubject), description ?? typeof(TSubject).ToLazyDebugString()) {}

		public PrintableSource(Func<Func<TSubject>> doubleFunc, [NotNull] Lazy<string> description)
			: base(doubleFunc)
		{
			Description = description.ValidateArgumentIsNotNull();
		}

		public Lazy<string> Description { get; private set; }
	}
}
