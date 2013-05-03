#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using NUnit.Framework;
#endregion

namespace Stile.NUnit
{
	public abstract class EqualityFixture<TSubject>
		where TSubject : IEquatable<TSubject>
	{
		[SetUp]
		public virtual void Init()
		{
			TestSubject = GetTestSubject();
			Other = GetOther();
			Assert.NotNull(TestSubject);
			Assert.NotNull(Other);
		}

		[Test]
		public void Left()
		{
			RequireDistinctObjects();
			AssertEqualsAndHashCode(TestSubject, Other);
		}

		[Test]
		public void Reflexive()
		{
			AssertEqualsAndHashCode(TestSubject, TestSubject, true);
		}

		[Test]
		public void Right()
		{
			RequireDistinctObjects();
			AssertEqualsAndHashCode(Other, TestSubject);
		}

		protected TSubject Other { get; set; }
		protected TSubject TestSubject { get; set; }

		protected void AssertEqualsAndHashCode(TSubject testSubject, TSubject other, bool expected = false)
		{
			Assert.That(testSubject.Equals(other), Is.EqualTo(expected));
			Assert.That(testSubject.GetHashCode().Equals(other.GetHashCode()), Is.EqualTo(expected));
		}

		protected abstract TSubject GetOther();
		protected abstract TSubject GetTestSubject();

		protected void RequireDistinctObjects(TSubject subject = default(TSubject))
		{
			if (ReferenceEquals(null, subject) || subject.Equals(default(TSubject)))
			{
				subject = Other;
			}
			Assert.That(ReferenceEquals(TestSubject, subject), Is.False, "precondition");
		}
	}

	public abstract class EqualityFixtureWithClone<TSubject> : EqualityFixture<TSubject>
		where TSubject : IEquatable<TSubject>
	{
		[SetUp]
		public override void Init()
		{
			base.Init();
			Cloner = GetCloner();
		}

		[Test]
		public void Clone()
		{
			TSubject clone = Cloner.Invoke(TestSubject);
			RequireDistinctObjects(clone);
			AssertEqualsAndHashCode(TestSubject, clone, true);
		}

		[NotNull]
		protected Func<TSubject, TSubject> Cloner { get; set; }

		protected abstract Func<TSubject, TSubject> GetCloner();
	}
}
