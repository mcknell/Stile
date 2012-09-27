#region License info...
// Propter for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...

#endregion

namespace Stile.Prototypes.Specifications.Printable.Output.GrammarMetadata
{
    /// <summary>
    /// Specialized syntax for a <see cref="SymbolAttribute"/> that's just a prefix.
    /// </summary>
    public class PrefixAttribute : SymbolAttribute
    {
        public PrefixAttribute(Terminal terminal)
        {
            Prefix = terminal;
        }
    }
}
