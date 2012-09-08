#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using System;
#endregion

namespace Stile.Types.Expressions.Printing.Tokens
{
	public class TokenFormat
	{
		private static readonly Lazy<TokenFormat> SharedInstance = new Lazy<TokenFormat>(() => new TokenFormat());

		public TokenFormat()
		{
			BinaryOperators = new BinaryOperatorBag();
			UnaryOperators = new UnaryBag();
			CloseArrayIndex = "]";
			CloseBlock = " }";
			CloseInvocation = ")";
			CloseNew = ")";
			CloseParenthesis = ")";
			ConditionalFirst = " ? ";
			ConditionalSecond = " : ";
			ItemSeparator = ", ";
			Language = VersionedLanguage.CSharp4;
			OpenArrayIndex = "[";
			OpenBlock = "{ ";
			OpenInvocation = ".Invoke(";
			OpenNew = "(";
			OpenParenthesis = "(";
			PrefixNew = "new ";
			StatementSeparator = "; ";
		}

		public BinaryOperatorBag BinaryOperators { get; protected set; }
		public string CloseArrayIndex { get; protected set; }
		public string CloseBlock { get; protected set; }
		public string CloseInvocation { get; protected set; }
		public string CloseNew { get; protected set; }
		public string CloseParenthesis { get; protected set; }
		public string ConditionalFirst { get; protected set; }
		public string ConditionalSecond { get; protected set; }
		public string ItemSeparator { get; protected set; }
		public VersionedLanguage Language { get; protected set; }
		public string OpenArrayIndex { get; protected set; }
		public string OpenBlock { get; protected set; }
		public string OpenInvocation { get; protected set; }
		public string OpenNew { get; protected set; }
		public string OpenParenthesis { get; protected set; }
		public string PrefixNew { get; protected set; }
		public static TokenFormat Shared
		{
			get { return SharedInstance.Value; }
		}
		public string StatementSeparator { get; protected set; }
		public UnaryBag UnaryOperators { get; protected set; }

		public class BinaryOperatorBag
		{
			public BinaryOperatorBag()
			{
				Add = new BinaryOperatorToken(" + ");
				AddAssign = new BinaryOperatorToken(" += ");
				AddAssignChecked = new BinaryOperatorToken(" += ");
				AddChecked = new BinaryOperatorToken(" + ");
				And = new BinaryOperatorToken(" & ");
				AndAlso = new BinaryOperatorToken(" && ");
				AndAssign = new BinaryOperatorToken(" &= ");
				Assign = new BinaryOperatorToken(" = ");
				Coalesce = new BinaryOperatorToken(" ?? ");
				Divide = new BinaryOperatorToken(" / ");
				DivideAssign = new BinaryOperatorToken(" /= ");
				Equal = new BinaryOperatorToken(" == ");
				ExclusiveOr = new BinaryOperatorToken(" ^ ");
				ExclusiveOrAssign = new BinaryOperatorToken(" ^= ");
				GreaterThan = new BinaryOperatorToken(" > ");
				GreaterThanOrEqual = new BinaryOperatorToken(" >= ");
				LeftShift = new BinaryOperatorToken(" << ");
				LeftShiftAssign = new BinaryOperatorToken(" <<= ");
				LessThan = new BinaryOperatorToken(" < ");
				LessThanOrEqual = new BinaryOperatorToken(" <= ");
				Modulo = new BinaryOperatorToken(" % ");
				ModuloAssign = new BinaryOperatorToken(" %= ");
				Multiply = new BinaryOperatorToken(" * ");
				MultiplyAssign = new BinaryOperatorToken(" *= ");
				MultiplyAssignChecked = new BinaryOperatorToken(" *= ");
				MultiplyChecked = new BinaryOperatorToken(" * ");
				NotEqual = new BinaryOperatorToken(" != ");
				Or = new BinaryOperatorToken(" | ");
				OrAssign = new BinaryOperatorToken(" |= ");
				OrElse = new BinaryOperatorToken(" || ");
				RightShift = new BinaryOperatorToken(" >> ");
				RightShiftAssign = new BinaryOperatorToken(" >>= ");
				Subtract = new BinaryOperatorToken(" - ");
				SubtractAssign = new BinaryOperatorToken(" -= ");
				SubtractAssignChecked = new BinaryOperatorToken(" -= ");
				SubtractChecked = new BinaryOperatorToken(" - ");
			}

			public BinaryOperatorToken Add { get; protected set; }
			public BinaryOperatorToken AddAssign { get; protected set; }
			public BinaryOperatorToken AddAssignChecked { get; protected set; }
			public BinaryOperatorToken AddChecked { get; protected set; }
			public BinaryOperatorToken And { get; protected set; }
			public BinaryOperatorToken AndAlso { get; protected set; }
			public BinaryOperatorToken AndAssign { get; protected set; }
			public BinaryOperatorToken Assign { get; protected set; }
			public BinaryOperatorToken Coalesce { get; protected set; }
			public BinaryOperatorToken Divide { get; protected set; }
			public BinaryOperatorToken DivideAssign { get; protected set; }
			public BinaryOperatorToken Equal { get; protected set; }
			public BinaryOperatorToken ExclusiveOr { get; protected set; }
			public BinaryOperatorToken ExclusiveOrAssign { get; protected set; }
			public BinaryOperatorToken GreaterThan { get; protected set; }
			public BinaryOperatorToken GreaterThanOrEqual { get; protected set; }
			public BinaryOperatorToken LeftShift { get; protected set; }
			public BinaryOperatorToken LeftShiftAssign { get; protected set; }
			public BinaryOperatorToken LessThan { get; protected set; }
			public BinaryOperatorToken LessThanOrEqual { get; protected set; }
			public BinaryOperatorToken Modulo { get; protected set; }
			public BinaryOperatorToken ModuloAssign { get; protected set; }
			public BinaryOperatorToken Multiply { get; protected set; }
			public BinaryOperatorToken MultiplyAssign { get; protected set; }
			public BinaryOperatorToken MultiplyAssignChecked { get; protected set; }
			public BinaryOperatorToken MultiplyChecked { get; protected set; }
			public BinaryOperatorToken NotEqual { get; protected set; }
			public BinaryOperatorToken Or { get; protected set; }
			public BinaryOperatorToken OrAssign { get; protected set; }
			public BinaryOperatorToken OrElse { get; protected set; }
			public BinaryOperatorToken RightShift { get; protected set; }
			public BinaryOperatorToken RightShiftAssign { get; protected set; }
			public BinaryOperatorToken Subtract { get; protected set; }
			public BinaryOperatorToken SubtractAssign { get; protected set; }
			public BinaryOperatorToken SubtractAssignChecked { get; protected set; }
			public BinaryOperatorToken SubtractChecked { get; protected set; }
		}

		public class UnaryBag
		{
			public UnaryBag()
			{
				ArrayLength = new UnaryToken(string.Empty, ".Length");
				Convert = new UnaryToken(string.Empty, string.Empty);
				Decrement = new UnaryToken("Decrement(", ")");
				Increment = new UnaryToken("Increment(", ")");
				IsFalse = new UnaryToken("IsFalse(", ")");
				IsTrue = new UnaryToken("IsTrue(", ")");
				Negate = new UnaryToken("-", string.Empty);
				Not = new UnaryToken("!", string.Empty);
				OnesComplement = new UnaryToken("~", string.Empty);
				PreDecrementAssign = new UnaryToken("(--", ")");
				PreIncrementAssign = new UnaryToken("(++", ")");
				PostDecrementAssign = new UnaryToken("(", "--)");
				PostIncrementAssign = new UnaryToken("(", "++)");
				Quote = new UnaryToken("Expression.Quote(", ")");
				UnaryPlus = new UnaryToken("(+", ")");
			}

			public UnaryToken ArrayLength { get; protected set; }
			public UnaryToken Convert { get; protected set; }
			public UnaryToken Decrement { get; protected set; }
			public UnaryToken Increment { get; protected set; }
			public UnaryToken IsFalse { get; protected set; }
			public UnaryToken IsTrue { get; protected set; }
			public UnaryToken Negate { get; protected set; }
			public UnaryToken Not { get; protected set; }
			public UnaryToken OnesComplement { get; protected set; }
			public UnaryToken PostDecrementAssign { get; protected set; }
			public UnaryToken PostIncrementAssign { get; protected set; }
			public UnaryToken PreDecrementAssign { get; protected set; }
			public UnaryToken PreIncrementAssign { get; protected set; }
			public UnaryToken Quote { get; protected set; }
			public UnaryToken UnaryPlus { get; protected set; }
		}
	}
}
