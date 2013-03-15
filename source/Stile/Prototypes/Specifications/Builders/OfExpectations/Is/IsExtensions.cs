#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Diagnostics.Contracts;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations.Is
{
	public static class IsExtensions
	{
		[Pure]
		public static TSpecification EqualTo<TSpecification, TSubject, TResult>(
			this IIs<TSpecification, TSubject, TResult> builder, TResult result)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
		{
			return Is.EqualTo.Make(x => x.Equals(result), result, builder.Xray);
		}

		[Pure]
		public static TSpecification Null<TSpecification, TSubject, TResult>(
			this IIs<TSpecification, TSubject, TResult?> builder)
			where TSpecification : class, ISpecification<TSubject, TResult?>, IChainableSpecification
			where TResult : struct
		{
			var result = new Nullable<TSpecification, TSubject, TResult>(builder.Xray);
			return result.Build();
		}

		[Pure]
		public static TSpecification Null<TSpecification, TSubject, TResult>(
			this IIs<TSpecification, TSubject, TResult> builder)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
			where TResult : class
		{
			var result = new Null<TSpecification, TSubject, TResult>(builder.Xray);
			return result.Build();
		}
	}
}
