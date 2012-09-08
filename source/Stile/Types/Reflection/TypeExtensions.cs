#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using System;
using System.Linq;
#endregion

namespace Stile.Types.Reflection
{
	public static class TypeExtensions
	{
		public static bool Implements<TType>(this Type type)
		{
			return type.Implements(typeof(TType));
		}

		public static bool Implements(this Type type, Type target)
		{
			Type implementer;
			return Implements(type, target, out implementer);
		}

		public static bool Implements(this Type type, Type target, out Type implementer)
		{
			if (!target.IsInterface)
			{
				throw new ArgumentOutOfRangeException("target", "Must be an interface.");
			}
			Func<Type, bool> predicate;
			if (target.IsGenericTypeDefinition)
			{
				predicate = x => x.IsGenericType && x.GetGenericTypeDefinition() == target;
			}
			else
			{
				predicate = x => x == target;
			}
			implementer = type.GetInterfaces().FirstOrDefault(predicate);
			return implementer != null;
		}

		/// <summary>
		///   Indicates whether a type is a closure that captures a variable (local variable or parameter).
		/// </summary>
		/// <param name = "type"></param>
		/// <returns></returns>
		/// <remarks>
		///   From the C#4 spec, Copyright 1999-2010 Microsoft Corporation: 
		///   "7.15.5 Outer variables
		///   Any local variable, value parameter, or parameter array whose scope includes the lambda-expression 
		///   or anonymous-method-expression is called an outer variable of the anonymous function. In an instance 
		///   function member of a class, the this value is considered a value parameter and is an outer variable 
		///   of any anonymous function contained within the function member.
		///   7.15.5.1 Captured outer variables
		///   When an outer variable is referenced by an anonymous function, the outer variable is said to have been 
		///   captured by the anonymous function. Ordinarily, the lifetime of a local variable is limited to 
		///   execution of the block or statement with which it is associated (§5.1.7). However, the lifetime of a 
		///   captured outer variable is extended at least until the delegate or expression tree created from the 
		///   anonymous function becomes eligible for garbage collection."
		/// </remarks>
		public static bool IsCapturingClosure(this Type type)
		{
			return type.IsNested && type.Name.StartsWith("<>c", StringComparison.Ordinal);
		}

		public static bool IsNullable(this Type type)
		{
			if (Nullable.GetUnderlyingType(type) != null)
				return true;
			return false;
		}

		public static bool IsOrDerivesFrom<TItem>(this Type type)
		{
			return type.IsOrDerivesFrom(typeof(TItem));
		}

		public static bool IsOrDerivesFrom(this Type type, Type other)
		{
			if (ReferenceEquals(null, type))
			{
				return false;
			}
			if (ReferenceEquals(null, other))
			{
				return false;
			}
			if (type.Equals(other))
			{
				return true;
			}
			if (type.IsSubclassOf(other))
			{
				return true;
			}
			return false;
		}

		public static string ToDebugString(this Type type)
		{
			return new TypeStringBuilder(type).ToString();
		}
	}
}
