#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Globalization;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Types.Primitives;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.ContextFree
{
	public partial class Symbol : IEquatable<Symbol>,
		IClauseMember
	{
		protected Symbol([NotNull] string token, string alias = null)
		{
			token = token.ValidateStringNotNullOrEmpty();
			Token = ToTitleCase(token);
			Alias = alias;
		}

		public string Alias { get; private set; }
		[NotNull]
		public string Token { get; private set; }

		public void Accept(IGrammarVisitor visitor)
		{
			visitor.Visit(this);
		}

		public TData Accept<TData>(IGrammarVisitor<TData> visitor, TData data)
		{

			return visitor.Visit(this, data);
		}

		public override string ToString()
		{
			if (Alias == null)
			{
				return Token;
			}
			if (Alias.Equals(Token, StringComparison.InvariantCultureIgnoreCase))
			{
				return Token;
			}
			return "{0} aka {1}".InvariantFormat(Token, Alias);
		}

		public static string ToTitleCase(string parameterName)
		{
			if (string.IsNullOrWhiteSpace(parameterName))
			{
				throw new ArgumentOutOfRangeException("parameterName");
			}
			string trimmed = parameterName.Trim();
			return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(trimmed.Substring(0, 1)) + trimmed.Substring(1);
		}

		public static implicit operator string(Symbol symbol)
		{
			return symbol.Token;
		}
	}

	public partial class Symbol
	{
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
			return string.Equals(Token, other.Token) 
				// ignore Alias!
				//&& string.Equals(Alias, other.Alias)
				;
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
			int hashCode = Token.GetHashCode();
			/* ignore Alias!
			 * if (Alias != null)
			{
				unchecked
				{
					hashCode ^= Alias.GetHashCode();
				}
			}*/
			return hashCode;
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
