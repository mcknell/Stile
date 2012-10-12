#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

namespace Stile.Prototypes.Specifications.DSL.ExpressionBuilders.SubjectBuilders
{
	/// <summary>
	/// This non-generic interface exists for typewashing, such as in collections.
	/// </summary>
	public interface ISubjectBuilder {}

	public interface ISubjectBuilder<TSubject> : ISubjectBuilder {}

	/// <summary>
	/// This non-generic interface exists for typewashing, such as in collections.
	/// </summary>
	public interface ISubjectBuilderState {}

// ReSharper disable UnusedTypeParameter
	public interface ISubjectBuilderState<out TSubject> // ReSharper restore UnusedTypeParameter
		: ISubjectBuilderState {}

	public class SubjectBuilder<TSubject> : ISubjectBuilder<TSubject>,
		ISubjectBuilderState<TSubject> {}
}
