#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using System.Linq;
using Stile.Patterns.Behavioral.Validation;
using Stile.Types.Enumerables;
#endregion

namespace Stile.Prototypes.Compilation.Grammars
{
	public class Clause
	{
		public Clause(Term term, params Term[] terms)
			: this(term, Grammars.Cardinality.One, terms) {}

		public Clause(Term term, Cardinality cardinality, params Term[] terms)
		{
			Cardinality = cardinality;
			Terms = terms.Unshift(term.ValidateArgumentIsNotNull()).ToArray();
		}

		public Cardinality Cardinality { get; private set; }

		public IReadOnlyCollection<Term> Terms { get; set; }
	}
}
