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
using System.Text;
using Stile.Readability;
using Stile.Types.Enums;
using Stile.Types.Reflection;
#endregion

namespace Stile.Types.Expressions
{
    public class ExpressionStringBuilder
    {
        private static readonly Dictionary<ExpressionType, Action<ExpressionStringBuilder, BinaryExpression>> BinaryNonstatements
            = new Dictionary<ExpressionType, Action<ExpressionStringBuilder, BinaryExpression>>
              {
                  {ExpressionType.Add, (x, e) => x.AppendAdd(e)},
                  {ExpressionType.AddChecked, (x, e) => x.AppendAdd(e)},
                  {ExpressionType.And, (x, e) => x.AppendAnd(e)},
                  {ExpressionType.AndAlso, (x, e) => x.AppendAndAlso(e)},
                  {ExpressionType.ArrayIndex, (x, e) => x.AppendArrayIndex(e)},
                  {ExpressionType.Assign, (x, e) => x.AppendAssign(e)},
                  {ExpressionType.Coalesce, (x, e) => x.AppendCoalesce(e)},
                  {ExpressionType.Divide, (x, e) => x.AppendDivide(e)},
                  {ExpressionType.Equal, (x, e) => x.AppendEqual(e)},
                  {ExpressionType.ExclusiveOr, (x, e) => x.AppendExclusiveOr(e)},
                  {ExpressionType.GreaterThan, (x, e) => x.AppendGreaterThan(e)},
                  {ExpressionType.GreaterThanOrEqual, (x, e) => x.AppendGreaterThanOrEqual(e)},
                  {ExpressionType.LeftShift, (x, e) => x.AppendLeftShift(e)},
                  {ExpressionType.LessThan, (x, e) => x.AppendLessThan(e)},
                  {ExpressionType.LessThanOrEqual, (x, e) => x.AppendLessThanOrEqual(e)},
                  {ExpressionType.Modulo, (x, e) => x.AppendModulo(e)},
                  {ExpressionType.Multiply, (x, e) => x.AppendMultiply(e)},
                  {ExpressionType.MultiplyChecked, (x, e) => x.AppendMultiply(e)},
                  {ExpressionType.NotEqual, (x, e) => x.AppendNotEqual(e)},
                  {ExpressionType.Or, (x, e) => x.AppendOr(e)},
                  {ExpressionType.OrElse, (x, e) => x.AppendOrElse(e)},
                  {ExpressionType.Power, (x, e) => x.AppendPower(e)},
                  {ExpressionType.RightShift, (x, e) => x.AppendRightShift(e)},
                  {ExpressionType.Subtract, (x, e) => x.AppendSubtract(e)},
                  {ExpressionType.SubtractChecked, (x, e) => x.AppendSubtract(e)}
              };

        private static readonly Dictionary<ExpressionType, Action<ExpressionStringBuilder, BinaryExpression>> BinaryStatements =
            new Dictionary<ExpressionType, Action<ExpressionStringBuilder, BinaryExpression>>
            {
                {ExpressionType.AddAssign, (x, e) => x.AppendAddAssign(e)},
                {ExpressionType.AddAssignChecked, (x, e) => x.AppendAddAssign(e)},
                {ExpressionType.AndAssign, (x, e) => x.AppendAndAssign(e)},
                {ExpressionType.DivideAssign, (x, e) => x.AppendDivideAssign(e)},
                {ExpressionType.ExclusiveOrAssign, (x, e) => x.AppendExclusiveOrAssign(e)},
                {ExpressionType.LeftShiftAssign, (x, e) => x.AppendLeftShiftAssign(e)},
                {ExpressionType.ModuloAssign, (x, e) => x.AppendModuloAssign(e)},
                {ExpressionType.MultiplyAssign, (x, e) => x.AppendMultiplyAssign(e)},
                {ExpressionType.MultiplyAssignChecked, (x, e) => x.AppendMultiplyAssign(e)},
                {ExpressionType.OrAssign, (x, e) => x.AppendOrAssign(e)},
                {ExpressionType.RightShiftAssign, (x, e) => x.AppendRightShiftAssign(e)},
                {ExpressionType.SubtractAssign, (x, e) => x.AppendSubtractAssign(e)},
                {ExpressionType.SubtractAssignChecked, (x, e) => x.AppendSubtractAssign(e)}
            };

        private static readonly Dictionary<ExpressionType, Action<ExpressionStringBuilder, UnaryExpression>> Unaries =
            new Dictionary<ExpressionType, Action<ExpressionStringBuilder, UnaryExpression>>
            {
                {ExpressionType.ArrayLength, (x, e) => x.AppendArrayLength(e)},
                {ExpressionType.Convert, (x, e) => x.AppendConvert(e)},
                {ExpressionType.ConvertChecked, (x, e) => x.AppendConvert(e)},
                {ExpressionType.Decrement, (x, e) => x.AppendDecrement(e)},
                {ExpressionType.Increment, (x, e) => x.AppendIncrement(e)},
                {ExpressionType.IsFalse, (x, e) => x.AppendIsFalse(e)},
                {ExpressionType.IsTrue, (x, e) => x.AppendIsTrue(e)},
                {ExpressionType.Negate, (x, e) => x.AppendNegate(e)},
                {ExpressionType.NegateChecked, (x, e) => x.AppendNegate(e)},
                {ExpressionType.Not, (x, e) => x.AppendNot(e)},
                {ExpressionType.OnesComplement, (x, e) => x.AppendOnesComplement(e)},
                {ExpressionType.PreDecrementAssign, (x, e) => x.AppendPreDecrementAssign(e)},
                {ExpressionType.PreIncrementAssign, (x, e) => x.AppendPreIncrementAssign(e)},
                {ExpressionType.PostDecrementAssign, (x, e) => x.AppendPostDecrementAssign(e)},
                {ExpressionType.PostIncrementAssign, (x, e) => x.AppendPostIncrementAssign(e)},
                {ExpressionType.Quote, (x, e) => x.AppendQuote(e)},
                {ExpressionType.TypeAs, (x, e) => x.AppendTypeAs(e)},
                {ExpressionType.UnaryPlus, (x, e) => x.AppendUnaryPlus(e)}
            };

        private static readonly Dictionary<ExpressionType, Action<ExpressionStringBuilder, Expression>> Various =
            new Dictionary<ExpressionType, Action<ExpressionStringBuilder, Expression>>
            {
                {ExpressionType.Call, (x, e) => x.AppendMethodCall((MethodCallExpression) e)},
                {ExpressionType.Constant, (x, e) => x.AppendConstant((ConstantExpression) e)},
                {ExpressionType.Default, (x, e) => x.AppendDefault((DefaultExpression) e)},
                {ExpressionType.Lambda, (x, e) => x.AppendLambda((LambdaExpression) e)},
                {ExpressionType.ListInit, (x, e) => x.AppendListInit((ListInitExpression) e)},
                {ExpressionType.MemberInit, (x, e) => x.AppendMemberInit((MemberInitExpression) e)}
            };

        private readonly StringBuilder _stringBuilder;
        private bool _suppressParenthesesOnce;

        public ExpressionStringBuilder()
            : this(new StringBuilder()) {}

        private ExpressionStringBuilder(StringBuilder stringBuilder)
        {
            _stringBuilder = stringBuilder;
        }

        public void AppendExpression(Expression expression)
        {
            if (IsBinaryExpressionAndNotAStatement(expression.NodeType))
            {
                _suppressParenthesesOnce = true;
            }
            Append(expression);
        }

        public override string ToString()
        {
            return _stringBuilder.ToString();
        }

        private void Append(string value)
        {
            _stringBuilder.Append(value);
        }

        private void Append(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.MemberAccess:
                    AppendMember(expression as MemberExpression);
                    return;
                case ExpressionType.Parameter:
                    AppendParameter((ParameterExpression) expression);
                    return;
                case ExpressionType.Index:
                    AppendIndex((IndexExpression) expression);
                    return;
                case ExpressionType.Block:
                    AppendBlock((BlockExpression) expression);
                    return;
                case ExpressionType.Conditional:
                    AppendConditional((ConditionalExpression) expression);
                    return;
                case ExpressionType.Invoke:
                    AppendInvoke((InvocationExpression) expression);
                    return;
                case ExpressionType.New:
                    AppendNew((NewExpression) expression);
                    return;
                case ExpressionType.NewArrayBounds:
                    AppendNewArrayBounds((NewArrayExpression) expression);
                    return;
                case ExpressionType.NewArrayInit:
                    AppendNewArray((NewArrayExpression) expression);
                    return;
                case ExpressionType.TypeEqual:
                    AppendTypeEqual((TypeBinaryExpression) expression);
                    return;
                case ExpressionType.TypeIs:
                    AppendTypeBinary((TypeBinaryExpression) expression);
                    return;

                default:
                    if (CanHandle(expression as BinaryExpression, BinaryStatements, BinaryNonstatements))
                    {
                        return;
                    }
                    if (CanHandle(expression as UnaryExpression, Unaries))
                    {
                        return;
                    }
                    if (CanHandle(expression, Various))
                    {
                        return;
                    }
                    throw Enumeration.FailedToRecognize(() => expression.NodeType);
            }
        }

        private void AppendAdd(BinaryExpression expression)
        {
            AppendBinary(expression, "+");
        }

        private void AppendAddAssign(BinaryExpression expression)
        {
            AppendAssign(expression, "+");
        }

        private void AppendAnd(BinaryExpression expression)
        {
            AppendBinary(expression, "&");
        }

        private void AppendAndAlso(BinaryExpression expression)
        {
            AppendBinary(expression, "&&");
        }

        private void AppendAndAssign(BinaryExpression expression)
        {
            AppendBinary(expression, "&=", Parentheses.No);
        }

        private void AppendArrayIndex(BinaryExpression expression)
        {
            AppendIndex(expression.Left, expression.Right);
        }

        private void AppendArrayLength(UnaryExpression expression)
        {
            Append(expression.Operand);
            Append(".Length");
        }

        private void AppendAssign(BinaryExpression expression)
        {
            AppendAssign(expression, string.Empty);
        }

        private void AppendAssign(BinaryExpression expression, string assignmentPrefix)
        {
            AppendBinary(expression, string.Format("{0}=", assignmentPrefix), Parentheses.No);
        }

        private void AppendBinary(BinaryExpression expression, string operatorChar)
        {
            Parentheses surroundWithParentheses;
            if (_suppressParenthesesOnce)
            {
                _suppressParenthesesOnce = false;
                surroundWithParentheses = Parentheses.No;
            }
            else
            {
                surroundWithParentheses = Parentheses.Yes;
            }
            AppendBinary(expression, operatorChar, surroundWithParentheses);
        }

        private void AppendBinary(BinaryExpression expression, string operatorChar, Parentheses surroundWithParentheses)
        {
            if (surroundWithParentheses == Parentheses.Yes)
            {
                Append("(");
            }
            Append(expression.Left);
            Append(string.Format(" {0} ", operatorChar));
            Append(expression.Right);
            if (surroundWithParentheses == Parentheses.Yes)
            {
                Append(")");
            }
        }

        private void AppendBlock(BlockExpression expression)
        {
            Append("{ ");
            AppendSequence("; ", expression.Expressions.ToArray());
            Append(" }");
        }

        private void AppendCoalesce(BinaryExpression binaryExpression)
        {
            AppendBinary(binaryExpression, "??");
        }

        private void AppendConditional(ConditionalExpression expression)
        {
            Append(expression.Test);
            Append(" ? ");
            Append(expression.IfTrue);
            Append(" : ");
            Append(expression.IfFalse);
        }

        private void AppendConstant(ConstantExpression expression)
        {
            if (expression.Type != typeof(string))
            {
                Append(expression.Value.ToDebugString());
            }
            else
            {
                Append(expression.ToString());
            }
        }

        private void AppendConvert(UnaryExpression expression)
        {
            AppendUnary(string.Empty, expression);
        }

        private void AppendDecrement(UnaryExpression expression)
        {
            Append("Decrement(");
            Append(expression.Operand);
            Append(")");
        }

        private void AppendDefault(DefaultExpression expression)
        {
            Append("default(");
            Append(expression.Type.ToDebugString());
            Append(")");
        }

        private void AppendDivide(BinaryExpression expression)
        {
            AppendBinary(expression, "/");
        }

        private void AppendDivideAssign(BinaryExpression expression)
        {
            AppendAssign(expression, "/");
        }

        private void AppendElementInit(ElementInit expression)
        {
            Append(expression.AddMethod.Name);
            Append("(");
            AppendSequence(expression.Arguments.ToArray());
            Append(")");
        }

        private void AppendEqual(BinaryExpression expression)
        {
            AppendBinary(expression, "==");
        }

        private void AppendExclusiveOr(BinaryExpression expression)
        {
            AppendBinary(expression, "^");
        }

        private void AppendExclusiveOrAssign(BinaryExpression expression)
        {
            AppendAssign(expression, "^");
        }

        private void AppendGreaterThan(BinaryExpression expression)
        {
            AppendBinary(expression, ">");
        }

        private void AppendGreaterThanOrEqual(BinaryExpression expression)
        {
            AppendBinary(expression, ">=");
        }

        private void AppendIncrement(UnaryExpression expression)
        {
            Append("Increment(");
            Append(expression.Operand);
            Append(")");
        }

        private void AppendIndex(Expression left, params Expression[] right)
        {
            Append(left);
            Append("[");
            AppendSequence(right);
            Append("]");
        }

        private void AppendIndex(IndexExpression expression)
        {
            AppendIndex(expression.Object, expression.Arguments.ToArray());
        }

        private void AppendInvoke(InvocationExpression expression)
        {
            Append(expression.Expression);
            Append(".Invoke(");
            AppendSequence(expression.Arguments.ToArray());
            Append(")");
        }

        private void AppendIsFalse(UnaryExpression expression)
        {
            AppendUnary("IsFalse(", expression, ")");
        }

        private void AppendIsTrue(UnaryExpression expression)
        {
            AppendUnary("IsTrue(", expression, ")");
        }

        private void AppendLambda(LambdaExpression expression)
        {
            int parameterCount = expression.Parameters.Count;
            if (parameterCount == 0)
            {
                Append("()");
            }
            else
            {
                bool addParentheses = parameterCount != 1;
                if (addParentheses)
                {
                    Append("(");
                }
                AppendSequence(expression.Parameters.ToArray());
                if (addParentheses)
                {
                    Append(")");
                }
            }
            AppendLambdaBody(expression);
        }

        private void AppendLambdaBody(LambdaExpression expression)
        {
            Append(" => ");
            Expression body = expression.Body;
            if (IsBinaryExpressionAndNotAStatement(body.NodeType))
            {
                _suppressParenthesesOnce = true;
            }
            Append(body);
        }

        private void AppendLeftShift(BinaryExpression expression)
        {
            AppendBinary(expression, "<<");
        }

        private void AppendLeftShiftAssign(BinaryExpression expression)
        {
            AppendAssign(expression, "<<");
        }

        private void AppendLessThan(BinaryExpression expression)
        {
            AppendBinary(expression, "<");
        }

        private void AppendLessThanOrEqual(BinaryExpression expression)
        {
            AppendBinary(expression, "<=");
        }

        private void AppendListInit(ListInitExpression expression)
        {
            Append(expression.NewExpression);
            Append(" {");
            if (expression.Initializers.Any())
            {
                AppendElementInit(expression.Initializers.First());
                foreach (ElementInit elementInit in expression.Initializers.Skip(1))
                {
                    Append(", ");
                    AppendElementInit(elementInit);
                }
            }
            Append("}");
        }

        private void AppendMember(MemberExpression expression)
        {
            var constantExpression = expression.Expression as ConstantExpression;
            bool suppressName;
            if (constantExpression != null)
            {
                if (constantExpression.Type.IsCapturingClosure())
                {
                    suppressName = true;
                }
                else
                {
                    suppressName = constantExpression.Type.IsOrDerivesFrom(expression.Member.ReflectedType);
                }
            }
            else
            {
                suppressName = false;
            }
            if (!suppressName)
            {
                if (expression.MemberIsStatic())
                {
                    Append(expression.Member.ReflectedType.Name);
                }
                else
                {
                    Append(expression.Expression);
                }
                Append(".");
            }
            Append(expression.Member.Name);
        }

        private void AppendMemberInit(MemberInitExpression expression)
        {
            AppendNew(expression.NewExpression);
        }

        private void AppendMethodCall(MethodCallExpression expression)
        {
            if (expression.Object == null)
            {
                if (expression.Method.IsExtensionMethod())
                {
                    Append(expression.Arguments.First());
                    Append(".");
                    Append(expression.Method.Name);
                    Append("(");
                    foreach (Expression argument in expression.Arguments.Skip(1))
                    {
                        Append(", ");
                        Append(argument);
                    }
                    Append(")");
                    return;
                }
                Append(expression.Method.ReflectedType.Name);
            }
            else
            {
                Append(expression.Object);
            }
            Append(".");
            Append(expression.Method.Name);
            Append("(");
            if (expression.Arguments.Any())
            {
                Append(expression.Arguments.First());
                foreach (Expression argument in expression.Arguments.Skip(1))
                {
                    Append(", ");
                    Append(argument);
                }
            }
            Append(")");
        }

        private void AppendModulo(BinaryExpression expression)
        {
            AppendBinary(expression, "%");
        }

        private void AppendModuloAssign(BinaryExpression expression)
        {
            AppendAssign(expression, "%");
        }

        private void AppendMultiply(BinaryExpression expression)
        {
            AppendBinary(expression, "*");
        }

        private void AppendMultiplyAssign(BinaryExpression expression)
        {
            AppendAssign(expression, "*");
        }

        private void AppendNegate(UnaryExpression expression)
        {
            AppendUnary("-", expression);
        }

        private void AppendNew(NewExpression expression)
        {
            Type type = expression.Type;
// ReSharper disable ConditionIsAlwaysTrueOrFalse
            if (expression.Constructor != null)
// ReSharper restore ConditionIsAlwaysTrueOrFalse
            {
                type = expression.Constructor.ReflectedType;
            }
            Append(string.Format("new {0}(", type.ToDebugString()));
            AppendSequence(expression.Arguments.ToArray());
            Append(")");
        }

        private void AppendNewArray(NewArrayExpression expression)
        {
            Append(string.Format("new {0}", expression.Type.ToDebugString()));
            if (expression.Expressions.Any())
            {
                Append(" { ");
                AppendSequence(expression.Expressions.ToArray());
                Append(" }");
            }
        }

        private void AppendNewArrayBounds(NewArrayExpression expression)
        {
            Append(string.Format("new {0}[", expression.Type.GetElementType().ToDebugString()));
            AppendSequence(", ", expression.Expressions.ToArray());
            Append("]");
        }

        private void AppendNot(UnaryExpression expression)
        {
            AppendUnary("!", expression);
        }

        private void AppendNotEqual(BinaryExpression expression)
        {
            AppendBinary(expression, "!=");
        }

        private void AppendOnesComplement(UnaryExpression expression)
        {
            AppendUnary("~", expression);
        }

        private void AppendOr(BinaryExpression expression)
        {
            AppendBinary(expression, "|");
        }

        private void AppendOrAssign(BinaryExpression expression)
        {
            AppendAssign(expression, "|");
        }

        private void AppendOrElse(BinaryExpression expression)
        {
            AppendBinary(expression, "||");
        }

        private void AppendParameter(ParameterExpression expression)
        {
            Append(expression.Name);
        }

        private void AppendPostDecrementAssign(UnaryExpression expression)
        {
            AppendUnary("(", expression, "--)");
        }

        private void AppendPostIncrementAssign(UnaryExpression expression)
        {
            AppendUnary("(", expression, "++)");
        }

        private void AppendPower(BinaryExpression expression)
        {
            Append("Math.Pow(");
            Append(expression.Left);
            Append(", ");
            Append(expression.Right);
            Append(")");
        }

        private void AppendPreDecrementAssign(UnaryExpression expression)
        {
            AppendUnary("(--", expression, ")");
        }

        private void AppendPreIncrementAssign(UnaryExpression expression)
        {
            AppendUnary("(++", expression, ")");
        }

        private void AppendQuote(UnaryExpression expression)
        {
            Append("Expression.Quote(");
            Append(expression.Operand);
            Append(")");
        }

        private void AppendRightShift(BinaryExpression expression)
        {
            AppendBinary(expression, ">>");
        }

        private void AppendRightShiftAssign(BinaryExpression expression)
        {
            AppendAssign(expression, ">>");
        }

        private void AppendSequence(params Expression[] sequence)
        {
            AppendSequence(", ", sequence);
        }

        private void AppendSequence(string separator, params Expression[] sequence)
        {
            if (!sequence.Any())
            {
                return;
            }
            Append(sequence.First());
            foreach (Expression expression in sequence.Skip(1))
            {
                Append(separator);
                Append(expression);
            }
        }

        private void AppendSubtract(BinaryExpression expression)
        {
            AppendBinary(expression, "-");
        }

        private void AppendSubtractAssign(BinaryExpression expression)
        {
            AppendAssign(expression, "-");
        }

        private void AppendTypeAs(UnaryExpression expression)
        {
            Append(expression.Operand);
            Append(" as ");
            Append(expression.Type.ToDebugString());
        }

        private void AppendTypeBinary(TypeBinaryExpression expression)
        {
            Append(expression.Expression);
            Append(" is ");
            Append(expression.TypeOperand.ToDebugString());
        }

        private void AppendTypeEqual(TypeBinaryExpression expression)
        {
            Append(expression.Expression);
            Append(" is type equal to ");
            Append(expression.TypeOperand.ToDebugString());
        }

        private void AppendUnary(string prefix, UnaryExpression expression)
        {
            AppendUnary(prefix, expression, string.Empty);
        }

        private void AppendUnary(string prefix, UnaryExpression expression, string suffix)
        {
            Append(prefix);
            Append(expression.Operand);
            Append(suffix);
        }

        private void AppendUnaryPlus(UnaryExpression expression)
        {
            AppendUnary("(+", expression, ")");
        }

        private bool CanHandle<TExpression>(TExpression expression,
            params Dictionary<ExpressionType, Action<ExpressionStringBuilder, TExpression>>[] dictionaries)
            where TExpression : Expression
        {
            if (expression == null)
            {
                return false;
            }

            Action<ExpressionStringBuilder, TExpression> action;
            foreach (Dictionary<ExpressionType, Action<ExpressionStringBuilder, TExpression>> dictionary in dictionaries)
            {
                if (dictionary.TryGetValue(expression.NodeType, out action))
                {
                    action.Invoke(this, expression);
                    return true;
                }
            }
            return false;
        }

        private static bool IsBinaryExpressionAndNotAStatement(ExpressionType expressionType)
        {
            return BinaryNonstatements.ContainsKey(expressionType);
        }

        private enum Parentheses
        {
            Yes,
            No
        }
    }
}
