#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.Sources;
#endregion

namespace Stile.Prototypes.Specifications.DSL.ExpressionBuilders.SubjectBuilders
{
	/// <summary>
	/// This non-generic interface exists for typewashing, such as in collections.
	/// </summary>
	public interface ISubjectBuilder {}

	public interface ISubjectBuilder<TSubject> : ISubjectBuilder {}

	public interface IBoundSubjectBuilder : ISubjectBuilder {}

	public interface IBoundSubjectBuilder<TSubject> : IBoundSubjectBuilder,
		ISubjectBuilder<TSubject> {}

	/// <summary>
	/// This non-generic interface exists for typewashing, such as in collections.
	/// </summary>
	public interface ISubjectBuilderState {}

// ReSharper disable UnusedTypeParameter
	public interface ISubjectBuilderState<out TSubject, out TSource> // ReSharper restore UnusedTypeParameter
		: ISubjectBuilderState
	{
		TSource Source { get; }
	}

	public class SubjectBuilder<TSubject, TSource> : ISubjectBuilder<TSubject>,
		ISubjectBuilderState<TSubject, TSource>
		where TSource : class, ISource<TSubject>
	{
		private readonly TSource _source;

		protected SubjectBuilder([NotNull] TSource source)
		{
			_source = source.ValidateArgumentIsNotNull();
		}

		public TSource Source
		{
			get { return _source; }
		}
	}
}
