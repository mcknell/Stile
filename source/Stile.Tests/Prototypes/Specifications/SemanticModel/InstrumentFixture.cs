#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using NUnit.Framework;
using Stile.Prototypes.Specifications.SemanticModel;
#endregion

namespace Stile.Tests.Prototypes.Specifications.SemanticModel
{
	[TestFixture]
	public class InstrumentFixture
	{
		[Test]
		public void Describes()
		{
			int subject = 5;
			var instrument = new Instrument<int, string>(x => x.ToString(), null);
			Assert.That(instrument.Measure(new Source<int>(() => subject)).Value, Is.EqualTo(subject.ToString()));
		}

		[Test]
		public void Samples()
		{
			int subject = 5;
			var instrument = new Instrument<int, string>(x => x.ToString(), null);
			Assert.That(instrument.Measure(new Source<int>(() => subject)).Value, Is.EqualTo(subject.ToString()));
		}
	}
}
