#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations
{
	public interface IExpectationTerm<out TPrior> : IAcceptExpectationVisitors
		where TPrior : class, IAcceptExpectationVisitors
	{
		[NotNull]
		TPrior Prior { get; }
	}

	public abstract class ExpectationTerm<TPrior> : IExpectationTerm<TPrior>
		where TPrior : class, IAcceptExpectationVisitors
	{
		protected ExpectationTerm([NotNull] TPrior prior)
		{
			Prior = prior.ValidateArgumentIsNotNull();
		}

		public IAcceptExpectationVisitors Parent
		{
			get { return Prior; }
		}
		public TPrior Prior { get; private set; }
		public abstract void Accept(IExpectationVisitor visitor);
		public abstract TData Accept<TData>(IExpectationVisitor<TData> visitor, TData data);
	}
}
