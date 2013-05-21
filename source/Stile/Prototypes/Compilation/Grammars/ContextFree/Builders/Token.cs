#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using Stile.Patterns.Behavioral.Validation;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.ContextFree.Builders
{
	public partial struct Token : IEquatable<Token>
	{
		private readonly bool _initializedCorrectly;
		private readonly string _token;

		private Token(string token)
		{
			_token = token.ValidateArgumentIsNotNull();
			_initializedCorrectly = true;
		}

		public static Token For(string s)
		{
			return new Token(s);
		}

		private void ThrowIfNotInitializedCorrectly()
		{
			if (_initializedCorrectly == false)
			{
				throw new InvalidOperationException(ErrorMessages.Token_DefaultCtorInvalid);
			}
		}

		public override string ToString()
		{
			ThrowIfNotInitializedCorrectly();
			return base.ToString();
		}
	}

	public partial struct Token
	{
		public bool Equals(Token other)
		{
			ThrowIfNotInitializedCorrectly();
			return string.Equals(_token, other._token);
		}

		public override bool Equals(object obj)
		{
			ThrowIfNotInitializedCorrectly();
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			return obj is Token && Equals((Token) obj);
		}

		public override int GetHashCode()
		{
			ThrowIfNotInitializedCorrectly();
			return _token.GetHashCode();
		}

		public static bool operator ==(Token left, Token right)
		{
			left.ThrowIfNotInitializedCorrectly();
			right.ThrowIfNotInitializedCorrectly();
			return left.Equals(right);
		}

		public static bool operator !=(Token left, Token right)
		{
			left.ThrowIfNotInitializedCorrectly();
			right.ThrowIfNotInitializedCorrectly();
			return !left.Equals(right);
		}
	}
}
