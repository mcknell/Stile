#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
#endregion

namespace Stile.Prototypes.Specifications.DSL.ExpressionBuilders.Sources
{
	public interface ISource {}

	public interface ISource<out TSubject> : ISource
	{
		[CanBeNull]
		TSubject Get();
	}

	public class Source<TSubject> : ISource<TSubject>
	{
		private readonly Lazy<TSubject> _subjectGetter;

		public Source()
			: this(() => () => default(TSubject)) {}

		public Source([NotNull] Expression<Func<TSubject>> expression)
			: this(expression.Compile) {}

		public Source(TSubject subject)
			: this(() => () => subject) {}

		protected Source(Func<Func<TSubject>> doubleFunc)
		{
			_subjectGetter = new Lazy<TSubject>(doubleFunc.Invoke());
		}

		public TSubject Get()
		{
			return _subjectGetter.Value;
		}
	}
}
