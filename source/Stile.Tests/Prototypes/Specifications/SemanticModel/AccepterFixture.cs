#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Threading.Tasks;
using NUnit.Framework;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
#endregion

namespace Stile.Tests.Prototypes.Specifications.SemanticModel
{
	[TestFixture]
	public class AccepterFixture
	{
		[Test, Ignore]
		public void FromExpression()
		{
/*
			var source = new Expectation<int>(x => x == 4 ? Outcome.Succeeded : Outcome.Failed);
			Assert.That(source.Evaluate<>(new Measurement<int>(4, TaskStatus.RanToCompletion, false),false,
				(s, i, e)=> new Evaluation<int>()), Is.EqualTo(Outcome.Succeeded));
			Assert.That(source.Accept(5), Is.EqualTo(Outcome.Failed));
			Assert.That(source.Description.Value, Is.EqualTo("x => (x == 4) ? Outcome.Succeeded : Outcome.Failed"));
*/
		}
	}
}
