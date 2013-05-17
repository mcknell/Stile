#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExceptionFilters
{
	public interface IFluentBoundExceptionFilterBuilder<TSubject> :
		IExceptionFilterBuilder
			<IBoundFaultSpecification<TSubject, IFluentBoundExceptionFilterBuilder<TSubject>>, TSubject> {}

	public class FluentBoundExceptionFilterBuilder<TSubject> :
		ExceptionFilterBuilder
			<IBoundFaultSpecification<TSubject, IFluentBoundExceptionFilterBuilder<TSubject>>, TSubject,
				IFluentBoundExceptionFilterBuilder<TSubject>>,
		IFluentBoundExceptionFilterBuilder<TSubject>
	{
		public FluentBoundExceptionFilterBuilder([NotNull] IProcedure<TSubject> procedure,
			[CanBeNull] IBoundFaultSpecification<TSubject, IFluentBoundExceptionFilterBuilder<TSubject>> prior)
			: base(procedure, prior) {}

		public override object ChainFrom(object specification)
		{
			return new FluentBoundExceptionFilterBuilder<TSubject>(Inspection,
				(IBoundFaultSpecification<TSubject, IFluentBoundExceptionFilterBuilder<TSubject>>) specification);
		}

		protected override
			Func
				<IExceptionFilter<TSubject>,
					IBoundFaultSpecification<TSubject, IFluentBoundExceptionFilterBuilder<TSubject>>> SpecFactory
		{
			get { return Factory; }
		}
	}
}
