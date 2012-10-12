#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultIs;
using Stile.Prototypes.Specifications.DSL.SemanticModel;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.ResultIs
{
	/*public interface IPrintableIs : IIs {}

	public interface IPrintableIs<out TSubject, out TResult, out TSpecifies> : IPrintableIs,
		IIs<TSubject, TResult, TSpecifies>
		where TSpecifies : class, ISpecification<TSubject, TResult> {}
*/

	public class PrintableIs<TSubject, TResult> :
		Is
			<TSubject, TResult, IIs<TSubject, TResult, IFluentSpecification<TSubject, TResult>>,
				IFluentSpecification<TSubject, TResult>>
	{
		public PrintableIs(Negated negated, [NotNull] Lazy<Func<TSubject, TResult>> instrument)
			: base(negated, instrument) {}

		protected override IIs<TSubject, TResult, IFluentSpecification<TSubject, TResult>> Factory()
		{
			return new PrintableIs<TSubject, TResult>(Negated.True, Instrument);
		}
	}
}
