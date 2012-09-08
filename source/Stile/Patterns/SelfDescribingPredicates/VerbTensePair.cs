#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

namespace Stile.Patterns.SelfDescribingPredicates
{
    public class VerbTensePair : IVerbTensePair
    {
        public static readonly VerbTensePair NoOp = new VerbTensePair(string.Empty, string.Empty);
        public static readonly VerbTensePair WouldBeButWas = new VerbTensePair("be", "was");
        public static readonly VerbTensePair WouldBeGreaterThanButWas = new VerbTensePair("be >", "was");
        public static readonly VerbTensePair WouldBeLessThanButWas = new VerbTensePair("be <", "was");
        public static readonly VerbTensePair WouldBeLessThanOrEqualToButWas = new VerbTensePair("be <=", "was");
        public static readonly VerbTensePair WouldBeGreaterThanOrEqualToButWas = new VerbTensePair("be >=", "was");
        public static readonly VerbTensePair WouldChangeButWasAlways = new VerbTensePair("change", "was always");
        public static readonly VerbTensePair WouldChangeToButWas = new VerbTensePair("change to", "was");
        public static readonly VerbTensePair WouldContainButWas = new VerbTensePair("contain", "was");
        public static readonly VerbTensePair WouldEndWithButWas = new VerbTensePair("end with", "was");
        public static readonly VerbTensePair WouldHaveButHad = new VerbTensePair("have", "had");
        public static readonly VerbTensePair WouldSatisfyButWas = new VerbTensePair("satisfy", "was");
        public static readonly VerbTensePair WouldStartWithButWas = new VerbTensePair("start with", "was");

        public VerbTensePair(string followingWould, string followingBut)
        {
            FollowingWould = followingWould;
            FollowingBut = followingBut;
        }

        public string FollowingBut { get; private set; }
        public string FollowingWould { get; private set; }

        public bool Equals(IVerbTensePair other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(other.FollowingWould, FollowingWould) && Equals(other.FollowingBut, FollowingBut);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (!(obj is IVerbTensePair))
            {
                return false;
            }
            return Equals((IVerbTensePair) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (FollowingWould.GetHashCode() * 397) ^ FollowingBut.GetHashCode();
            }
        }

        public override string ToString()
        {
            return FollowingWould + "|" + FollowingBut;
        }

        public static bool operator ==(VerbTensePair left, VerbTensePair right)
        {
            return Equals(left, right);
        }

        public static bool operator ==(IVerbTensePair left, VerbTensePair right)
        {
            return Equals(left, right);
        }

        public static bool operator ==(VerbTensePair left, IVerbTensePair right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(VerbTensePair left, VerbTensePair right)
        {
            return !Equals(left, right);
        }

        public static bool operator !=(IVerbTensePair left, VerbTensePair right)
        {
            return !Equals(left, right);
        }

        public static bool operator !=(VerbTensePair left, IVerbTensePair right)
        {
            return !Equals(left, right);
        }
    }
}
