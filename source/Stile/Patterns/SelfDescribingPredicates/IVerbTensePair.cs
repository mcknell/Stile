#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using System;
#endregion

namespace Stile.Patterns.SelfDescribingPredicates
{
    public interface IVerbTensePair : IEquatable<IVerbTensePair>
    {
        string FollowingBut { get; }
        string FollowingWould { get; }
    }
}
