#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using System;
using System.Linq.Expressions;
using Stile.Types.Enums;
using Stile.Types.Expressions.Printing.ExpressionParsers;
using Stile.Types.Expressions.Printing.Tokens;
#endregion

namespace Stile.Types.Expressions.Printing
{
	public class ParserFactory
	{
		public ExpressionParser Make(Expression expression, IPrintStrategy printStrategy)
		{
			Func<BinaryOperatorToken, BinaryExpressionParser> makeBinary =
				token => new BinaryExpressionParser((BinaryExpression) expression, printStrategy, token);
			Func<UnaryToken, UnaryExpressionParser> makeUnary =
				syntax => new UnaryExpressionParser((UnaryExpression) expression, printStrategy, syntax);

			switch (expression.NodeType)
			{
				case ExpressionType.MemberAccess:
					return new MemberExpressionParser((MemberExpression) expression, printStrategy);
				case ExpressionType.Parameter:
					return new ParameterExpressionParser((ParameterExpression) expression, printStrategy);
				case ExpressionType.Index:
					return new IndexExpressionParser((IndexExpression) expression, printStrategy);
				case ExpressionType.Block:
					return new BlockExpressionParser((BlockExpression) expression, printStrategy);
				case ExpressionType.Conditional:
					return new ConditionalExpressionParser((ConditionalExpression) expression, printStrategy);
				case ExpressionType.Invoke:
					return new InvocationExpressionParser((InvocationExpression) expression, printStrategy);
				case ExpressionType.New:
					return new NewExpressionParser((NewExpression) expression, printStrategy);
				case ExpressionType.NewArrayBounds:
					return new NewArrayBoundsExpressionParser((NewArrayExpression) expression, printStrategy);
				case ExpressionType.NewArrayInit:
					return new NewArrayInitExpressionParser((NewArrayExpression) expression, printStrategy);
				case ExpressionType.TypeEqual:
					return new TypeEqualExpressionParser((TypeBinaryExpression) expression, printStrategy);
				case ExpressionType.TypeIs:
					return new TypeIsExpressionParser((TypeBinaryExpression) expression, printStrategy);
				case ExpressionType.MemberInit:
					return new MemberInitExpressionParser((MemberInitExpression) expression, printStrategy);
				case ExpressionType.Call:
					return new MethodCallExpressionParser((MethodCallExpression) expression, printStrategy);
				case ExpressionType.Constant:
					return new ConstantExpressionParser((ConstantExpression) expression, printStrategy);
				case ExpressionType.Default:
					return new DefaultExpressionParser((DefaultExpression) expression, printStrategy);
				case ExpressionType.Lambda:
					return new LambdaExpressionParser((LambdaExpression) expression, printStrategy);
				case ExpressionType.ListInit:
					return new ListInitExpressionParser((ListInitExpression) expression, printStrategy);

					// binary 
				case ExpressionType.Add:
					return makeBinary(printStrategy.Format.BinaryOperators.Add);
				case ExpressionType.AddAssign:
					return makeBinary(printStrategy.Format.BinaryOperators.AddAssign);
				case ExpressionType.AddAssignChecked:
					return makeBinary(printStrategy.Format.BinaryOperators.AddAssignChecked);
				case ExpressionType.AddChecked:
					return makeBinary(printStrategy.Format.BinaryOperators.AddChecked);
				case ExpressionType.And:
					return makeBinary(printStrategy.Format.BinaryOperators.And);
				case ExpressionType.AndAlso:
					return makeBinary(printStrategy.Format.BinaryOperators.AndAlso);
				case ExpressionType.AndAssign:
					return makeBinary(printStrategy.Format.BinaryOperators.AndAssign);
				case ExpressionType.ArrayIndex:
					return new ArrayIndexExpressionParser((BinaryExpression) expression, printStrategy);
				case ExpressionType.Assign:
					return makeBinary(printStrategy.Format.BinaryOperators.Assign);
				case ExpressionType.Coalesce:
					return makeBinary(printStrategy.Format.BinaryOperators.Coalesce);
				case ExpressionType.Divide:
					return makeBinary(printStrategy.Format.BinaryOperators.Divide);
				case ExpressionType.DivideAssign:
					return makeBinary(printStrategy.Format.BinaryOperators.DivideAssign);
				case ExpressionType.Equal:
					return makeBinary(printStrategy.Format.BinaryOperators.Equal);
				case ExpressionType.ExclusiveOr:
					return makeBinary(printStrategy.Format.BinaryOperators.ExclusiveOr);
				case ExpressionType.ExclusiveOrAssign:
					return makeBinary(printStrategy.Format.BinaryOperators.ExclusiveOrAssign);
				case ExpressionType.GreaterThan:
					return makeBinary(printStrategy.Format.BinaryOperators.GreaterThan);
				case ExpressionType.GreaterThanOrEqual:
					return makeBinary(printStrategy.Format.BinaryOperators.GreaterThanOrEqual);
				case ExpressionType.LeftShift:
					return makeBinary(printStrategy.Format.BinaryOperators.LeftShift);
				case ExpressionType.LeftShiftAssign:
					return makeBinary(printStrategy.Format.BinaryOperators.LeftShiftAssign);
				case ExpressionType.LessThan:
					return makeBinary(printStrategy.Format.BinaryOperators.LessThan);
				case ExpressionType.LessThanOrEqual:
					return makeBinary(printStrategy.Format.BinaryOperators.LessThanOrEqual);
				case ExpressionType.Modulo:
					return makeBinary(printStrategy.Format.BinaryOperators.Modulo);
				case ExpressionType.ModuloAssign:
					return makeBinary(printStrategy.Format.BinaryOperators.ModuloAssign);
				case ExpressionType.Multiply:
					return makeBinary(printStrategy.Format.BinaryOperators.Multiply);
				case ExpressionType.MultiplyAssign:
					return makeBinary(printStrategy.Format.BinaryOperators.MultiplyAssign);
				case ExpressionType.MultiplyAssignChecked:
					return makeBinary(printStrategy.Format.BinaryOperators.MultiplyAssignChecked);
				case ExpressionType.MultiplyChecked:
					return makeBinary(printStrategy.Format.BinaryOperators.MultiplyChecked);
				case ExpressionType.NotEqual:
					return makeBinary(printStrategy.Format.BinaryOperators.NotEqual);
				case ExpressionType.Or:
					return makeBinary(printStrategy.Format.BinaryOperators.Or);
				case ExpressionType.OrAssign:
					return makeBinary(printStrategy.Format.BinaryOperators.OrAssign);
				case ExpressionType.OrElse:
					return makeBinary(printStrategy.Format.BinaryOperators.OrElse);
				case ExpressionType.Power:
					return new PowerExpressionParser((BinaryExpression) expression, printStrategy);
				case ExpressionType.RightShift:
					return makeBinary(printStrategy.Format.BinaryOperators.RightShift);
				case ExpressionType.RightShiftAssign:
					return makeBinary(printStrategy.Format.BinaryOperators.RightShiftAssign);
				case ExpressionType.Subtract:
					return makeBinary(printStrategy.Format.BinaryOperators.Subtract);
				case ExpressionType.SubtractAssign:
					return makeBinary(printStrategy.Format.BinaryOperators.SubtractAssign);
				case ExpressionType.SubtractAssignChecked:
					return makeBinary(printStrategy.Format.BinaryOperators.SubtractAssignChecked);
				case ExpressionType.SubtractChecked:
					return makeBinary(printStrategy.Format.BinaryOperators.SubtractChecked);

					// unaries
				case ExpressionType.ArrayLength:
					return makeUnary(printStrategy.Format.UnaryOperators.ArrayLength);
				case ExpressionType.Convert:
				case ExpressionType.ConvertChecked:
					return makeUnary(printStrategy.Format.UnaryOperators.Convert);
				case ExpressionType.Decrement:
					return makeUnary(printStrategy.Format.UnaryOperators.Decrement);
				case ExpressionType.Increment:
					return makeUnary(printStrategy.Format.UnaryOperators.Increment);
				case ExpressionType.IsFalse:
					return makeUnary(printStrategy.Format.UnaryOperators.IsFalse);
				case ExpressionType.IsTrue:
					return makeUnary(printStrategy.Format.UnaryOperators.IsTrue);
				case ExpressionType.Negate:
				case ExpressionType.NegateChecked:
					return makeUnary(printStrategy.Format.UnaryOperators.Negate);
				case ExpressionType.Not:
					return makeUnary(printStrategy.Format.UnaryOperators.Not);
				case ExpressionType.OnesComplement:
					return makeUnary(printStrategy.Format.UnaryOperators.OnesComplement);
				case ExpressionType.PreDecrementAssign:
					return makeUnary(printStrategy.Format.UnaryOperators.PreDecrementAssign);
				case ExpressionType.PreIncrementAssign:
					return makeUnary(printStrategy.Format.UnaryOperators.PreIncrementAssign);
				case ExpressionType.PostDecrementAssign:
					return makeUnary(printStrategy.Format.UnaryOperators.PostDecrementAssign);
				case ExpressionType.PostIncrementAssign:
					return makeUnary(printStrategy.Format.UnaryOperators.PostIncrementAssign);
				case ExpressionType.Quote:
					return makeUnary(printStrategy.Format.UnaryOperators.Quote);
				case ExpressionType.TypeAs:
					return new TypeAsExpressionParser((UnaryExpression) expression, printStrategy);
				case ExpressionType.UnaryPlus:
					return makeUnary(printStrategy.Format.UnaryOperators.UnaryPlus);
			}
			throw Enumeration.FailedToRecognize(() => expression.NodeType);
		}
	}
}
