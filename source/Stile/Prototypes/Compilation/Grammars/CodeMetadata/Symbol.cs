#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.Grammar;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.CodeMetadata
{
	public class Symbol : IEquatable<Symbol>
	{
		public static readonly Symbol Deadline = new Symbol(Nonterminal.Deadline.ToString());
		public static readonly Symbol Expectation = new Symbol(Nonterminal.Expectation.ToString());
		public static readonly Symbol Specification = new Symbol(Nonterminal.Specification.ToString());

		public Symbol([NotNull] string token)
		{
			Token = token.Trim().Validate().EnumerableOf<char>().IsNotNullOrEmpty();
		}

		[NotNull]
		public string Token { get; private set; }

		public bool Equals(Symbol other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}
			return string.Equals(Token, other.Token);
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
			var symbol = obj as Symbol;
			if (symbol == null)
			{
				return false;
			}
			return Equals(symbol);
		}

		public override int GetHashCode()
		{
			return Token.GetHashCode();
		}

		public static Symbol Make([NotNull] string token)
		{
			return new Symbol(token);
		}

		public override string ToString()
		{
			return Token;
		}

		public static bool operator ==(Symbol left, Symbol right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Symbol left, Symbol right)
		{
			return !Equals(left, right);
		}
	}
}
