#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NUnit.Framework;
#endregion

namespace Stile.Tests.Types.Expressions
{
	public abstract class ExpressionPrintingFixture
	{
		protected static void DoNothing() {}
	}

	[TestFixture]
	public abstract class ExpressionPrintingFixture<TSubtype> : ExpressionPrintingFixture
		where TSubtype : ExpressionPrintingFixture<TSubtype>
	{
		protected static readonly ConstantExpression Constant3;
		private static readonly ConstantExpression Constant4 = Expression.Constant(4);
		private static readonly ParameterExpression VariableInt = Expression.Variable(typeof(int), "i");
		protected static readonly ConstantExpression ConstantFalse = Expression.Constant(false);
		private static readonly ConstantExpression ConstantStringBar = Expression.Constant("bar");
		private static readonly ConstantExpression ConstantStringFoo = Expression.Constant("foo");
		protected static readonly ConstantExpression ConstantTrue = Expression.Constant(true);
		private static readonly ConstantExpression Constant6Long = Expression.Constant(6L);
		private static readonly ConstantExpression Constant7Double = Expression.Constant(7D);
		protected static readonly int[] Ints;
		protected static readonly MemberExpression ArrayStaticMember;
		protected static readonly MethodCallExpression PredicateMethodCallExpression;
		protected static readonly Func<bool> StaticFuncBool;
		protected static readonly MemberExpression StaticFuncBoolExpression;
		private static readonly ExpressionType[] NotSupportedTypes = new[]
		                                                             {
		                                                             	ExpressionType.DebugInfo, ExpressionType.Dynamic,
		                                                             	ExpressionType.Extension, ExpressionType.Goto,
		                                                             	ExpressionType.Label, ExpressionType.PowerAssign,
		                                                             	ExpressionType.RuntimeVariables, ExpressionType.Loop,
		                                                             	ExpressionType.Switch, ExpressionType.Throw, ExpressionType.Try,
		                                                             	ExpressionType.Unbox,
		                                                             };
		private string _className;

		static ExpressionPrintingFixture()
		{
			Ints = new[] {1, 2, 3, 4};
			ArrayStaticMember = Expression.Field(null, typeof(TSubtype), "Ints");
			PredicateMethodCallExpression = Expression.Call(Expression.Constant(false), "Equals", null, ConstantTrue);
			StaticFuncBool = () => true;
			StaticFuncBoolExpression = Expression.Field(null, typeof(TSubtype), "StaticFuncBool");
			Constant3 = Expression.Constant(3);
		}

		[SetUp]
		public void Init()
		{
			_className = typeof(TSubtype).Name;
			AfterInit();
		}

		[Test]
		public void ExpressionTypes()
		{
			// arrange
			Assert.IsNotNull(Ints, "precondition");
			Assert.IsNotNull(StaticFuncBool, "precondition");
			var expectations = //
				new[]
				{
					new Expectation(Expression.Add(Constant3, Constant4), "3 + 4"),
					new Expectation(Expression.AddAssignChecked(VariableInt, Constant4), "i += 4"),
					new Expectation(Expression.AddAssign(VariableInt, Constant4), "i += 4"),
					new Expectation(Expression.AddChecked(Constant3, Constant4), "3 + 4"),
					new Expectation(Expression.And(Constant3, Constant4), "3 & 4"),
					new Expectation(Expression.AndAlso(ConstantTrue, ConstantFalse), "true && false"),
					new Expectation(Expression.AndAssign(VariableInt, Constant4), "i &= 4"),
					new Expectation(Expression.ArrayAccess(ArrayStaticMember, Constant3), _className + ".Ints[3]"),
					new Expectation(Expression.ArrayIndex(ArrayStaticMember, Constant3), _className + ".Ints[3]"),
					new Expectation(Expression.ArrayLength(ArrayStaticMember), _className + ".Ints.Length"),
					new Expectation(Expression.Assign(VariableInt, Constant4), "i = 4"),
					new Expectation(Expression.Block(Expression.MultiplyAssign(VariableInt, Constant4)), "{ i *= 4 }"),
					new Expectation(
						Expression.Block(Expression.MultiplyAssign(VariableInt, Constant4), Expression.AddAssign(VariableInt, Constant4)),
						"{ i *= 4; i += 4 }"), //
					new Expectation(Expression.Call(null, GetMethodInfo()), "ExpressionPrintingFixture.DoNothing()"),
					new Expectation(Expression.Coalesce(ConstantStringFoo, ConstantStringBar), "\"foo\" ?? \"bar\""),
					new Expectation(Expression.Condition(PredicateMethodCallExpression, ConstantFalse, ConstantTrue),
						"false.Equals(true) ? false : true"), //
					new Expectation(Expression.Constant(5), "5"), //
					new Expectation(Expression.Convert(Constant3, typeof(long)), "3"),
					new Expectation(Expression.ConvertChecked(Constant6Long, typeof(int)), "6"),
					new Expectation(Expression.Decrement(VariableInt), "Decrement(i)"),
					new Expectation(Expression.Default(typeof(string)), "default(string)"),
					//new Expectation(Expression.Dynamic(typeof(string)), "default(System.String)"),
					//new Expectation(Expression.DebugInfo(Constant3, Constant4), "3 / 4"),
					new Expectation(Expression.Divide(Constant3, Constant4), "3 / 4"),
					new Expectation(Expression.DivideAssign(VariableInt, Constant4), "i /= 4"),
					new Expectation(Expression.Equal(Constant3, Constant4), "3 == 4"),
					new Expectation(Expression.ExclusiveOr(Constant3, Constant4), "3 ^ 4"),
					new Expectation(Expression.ExclusiveOrAssign(VariableInt, Constant4), "i ^= 4"),
					new Expectation(Expression.GreaterThan(Constant3, Constant4), "3 > 4"),
					new Expectation(Expression.GreaterThanOrEqual(Constant3, Constant4), "3 >= 4"),
					new Expectation(Expression.Increment(VariableInt), "Increment(i)"),
					new Expectation(Expression.Invoke(StaticFuncBoolExpression), _className + ".StaticFuncBool.Invoke()"),
					new Expectation(Expression.IsFalse(ConstantTrue), "IsFalse(true)"),
					new Expectation(Expression.IsTrue(ConstantTrue), "IsTrue(true)"),
					new Expectation(Expression.Lambda(ConstantTrue), "() => true"),
					new Expectation(Expression.LeftShift(Constant3, Constant4), "3 << 4"),
					new Expectation(Expression.LeftShiftAssign(VariableInt, Constant4), "i <<= 4"),
					new Expectation(Expression.LessThan(Constant3, Constant4), "3 < 4"),
					new Expectation(Expression.LessThanOrEqual(Constant3, Constant4), "3 <= 4"),
					new Expectation(Expression.ListInit(Expression.New(typeof(List<int>)), Constant3, Constant4),
						"new List<int>() {Add(3), Add(4)}"), new Expectation(ArrayStaticMember, _className + ".Ints"), //
					new Expectation(Expression.MemberInit(Expression.New(typeof(int))), "new int()"),
					new Expectation(Expression.Modulo(Constant3, Constant4), "3 % 4"),
					new Expectation(Expression.ModuloAssign(VariableInt, Constant4), "i %= 4"),
					new Expectation(Expression.Multiply(Constant3, Constant4), "3 * 4"),
					new Expectation(Expression.MultiplyAssign(VariableInt, Constant4), "i *= 4"),
					new Expectation(Expression.MultiplyAssignChecked(VariableInt, Constant4), "i *= 4"),
					new Expectation(Expression.MultiplyChecked(Constant3, Constant4), "3 * 4"),
					new Expectation(Expression.Negate(VariableInt), "-i"), new Expectation(Expression.NegateChecked(VariableInt), "-i"),
					new Expectation(Expression.New(typeof(int)), "new int()"), //
					new Expectation(Expression.NewArrayInit(typeof(int)), "new int[]"),
					new Expectation(Expression.NewArrayBounds(typeof(int), Constant3), "new int[3]"),
					new Expectation(Expression.Not(ConstantTrue), "!true"), new Expectation(Expression.NotEqual(Constant3, Constant4), "3 != 4"),
					new Expectation(Expression.OnesComplement(Constant3), "~3"), new Expectation(Expression.Or(Constant3, Constant4), "3 | 4"),
					new Expectation(Expression.OrAssign(VariableInt, Constant4), "i |= 4"),
					new Expectation(Expression.OrElse(ConstantTrue, ConstantFalse), "true || false"),
					new Expectation(Expression.Parameter(typeof(string), "s"), "s"),
					new Expectation(Expression.Power(Constant7Double, Constant7Double), "Math.Pow(7, 7)"),
					new Expectation(Expression.PostDecrementAssign(VariableInt), "(i--)"),
					new Expectation(Expression.PostIncrementAssign(VariableInt), "(i++)"),
					new Expectation(Expression.PreDecrementAssign(VariableInt), "(--i)"),
					new Expectation(Expression.PreIncrementAssign(VariableInt), "(++i)"),
					new Expectation(Expression.Quote(Expression.Lambda(ConstantTrue)), "Expression.Quote(() => true)"),
					new Expectation(Expression.RightShift(Constant3, Constant4), "3 >> 4"),
					new Expectation(Expression.RightShiftAssign(VariableInt, Constant4), "i >>= 4"),
					new Expectation(Expression.Subtract(Constant3, Constant4), "3 - 4"),
					new Expectation(Expression.SubtractAssign(VariableInt, Constant4), "i -= 4"),
					new Expectation(Expression.SubtractAssignChecked(VariableInt, Constant4), "i -= 4"),
					new Expectation(Expression.SubtractChecked(Constant3, Constant4), "3 - 4"),
					new Expectation(Expression.TypeAs(Constant3, typeof(object)), "3 as object"),
					new Expectation(Expression.TypeIs(Constant3, typeof(object)), "3 is object"),
					new Expectation(Expression.TypeEqual(Expression.Constant(typeof(int)), typeof(object)), "int is type equal to object"),
					new Expectation(Expression.UnaryPlus(Constant3), "(+3)")
				};

			// assert
			foreach (Expectation expectation in expectations)
			{
				Assert.That(PrintExpression(expectation.Expression),
					Is.EqualTo(expectation.Expected),
					string.Format("for ExpressionType {0} == '{1}'", expectation.ExpressionType, expectation.Expression));
			}
			Type type = typeof(ExpressionType);
			List<ExpressionType> allExpressionTypes = Enum.GetValues(type).Cast<ExpressionType>() //
				.Except(NotSupportedTypes) //
				.Except(expectations.Select(x => x.ExpressionType)).ToList();
			CollectionAssert.IsEmpty(allExpressionTypes, string.Format("{0} untested {1} values", allExpressionTypes.Count, type.Name));
		}

		protected virtual void AfterInit() {}

		protected MethodInfo GetMethodInfo()
		{
			Action action = DoNothing;
			return action.Method;
		}

		protected abstract string PrintExpression(Expression expression);

		private class Expectation
		{
			public Expectation(Expression expression, string expected)
			{
				Expression = expression;
				Expected = expected;
				ExpressionType = Expression.NodeType;
			}

			public string Expected { get; private set; }
			public Expression Expression { get; private set; }
			public ExpressionType ExpressionType { get; private set; }
		}
	}
}
