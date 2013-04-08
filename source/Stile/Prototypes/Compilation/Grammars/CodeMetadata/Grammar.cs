using System.Collections.Generic;
using System.Linq;

namespace Stile.Prototypes.Compilation.Grammars.CodeMetadata
{
	public class Grammar
	{
		public Grammar(IEnumerable<Symbol> roots)
		{
			Roots = roots.ToArray();
		}

		public IReadOnlyCollection<Symbol> Roots { get; private set; } 
	}
}