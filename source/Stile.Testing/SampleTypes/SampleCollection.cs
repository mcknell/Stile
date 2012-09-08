#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using System.Collections.Generic;
using System.Collections.ObjectModel;
#endregion

namespace Stile.Testing.SampleTypes
{
    public interface ISampleCollection<TItem> : ICollection<TItem> {}

    public class SampleCollection<TItem> : Collection<TItem>,
        ISampleCollection<TItem> {}
}
