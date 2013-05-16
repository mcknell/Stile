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
	public interface IFluentExceptionFilterBuilder<TSubject> :
		IExceptionFilterBuilder<IFaultSpecification<TSubject, IFluentExceptionFilterBuilder<TSubject>>, TSubject> {}

	public class FluentExceptionFilterBuilder<TSubject> :
		ExceptionFilterBuilder
			<IFaultSpecification<TSubject, IFluentExceptionFilterBuilder<TSubject>>, TSubject,
				IFluentExceptionFilterBuilder<TSubject>>,
		IFluentExceptionFilterBuilder<TSubject>
	{
		public FluentExceptionFilterBuilder([NotNull] IProcedure<TSubject> procedure,
			[CanBeNull] IFaultSpecification<TSubject, IFluentExceptionFilterBuilder<TSubject>> prior,
			ISource<TSubject> source = null)
			: base(procedure, prior, source) {}

		public override
			Func<IExceptionFilter<TSubject>, IFaultSpecification<TSubject, IFluentExceptionFilterBuilder<TSubject>>>
			Factory
		{
			get { return MakeSpecification; }
		}

		public override object ChainFrom(object specification)
		{
			return new FluentExceptionFilterBuilder<TSubject>(Inspection,
				specification as IFaultSpecification<TSubject, IFluentExceptionFilterBuilder<TSubject>>,
				Source);
		}

		protected override IFluentExceptionFilterBuilder<TSubject> Builder
		{
			get { return this; }
		}
	}
}
