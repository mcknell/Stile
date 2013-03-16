#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using Stile.Patterns.Structural.FluentInterface;
using Stile.Prototypes.Specifications.Builders.Lifecycle;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfInstruments
{
	/// <summary>
	/// This non-generic interface exists for typewashing, such as in collections.
	/// </summary>
	public interface IProcedureBuilder {}

	public interface IProcedureBuilder<TSubject> : IProcedureBuilder,
		IHides<IProcedureBuilderState<TSubject>> {}

	public interface IProcedureBuilderState<TSubject> : IHasSource<TSubject>,
		IAcceptSpecificationVisitors {}

	public class ProcedureBuilder<TSubject> : IProcedureBuilder<TSubject>,
		IProcedureBuilderState<TSubject>
	{
		public ProcedureBuilder(ISource<TSubject> source = null)
		{
			Source = source;
		}

		public IAcceptSpecificationVisitors Parent
		{
			get { return Source; }
		}

		public ISource<TSubject> Source { get; private set; }
		public IProcedureBuilderState<TSubject> Xray
		{
			get { return this; }
		}

		public void Accept(ISpecificationVisitor visitor)
		{
			visitor.Visit1(this);
		}

		public TData Accept<TData>(ISpecificationVisitor<TData> visitor, TData data)
		{
			return visitor.Visit1(this, data);
		}
	}
}
