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
#endregion

namespace Stile.Types
{
    public static class CSharp4Types
    {
// ReSharper disable MemberCanBePrivate.Global
        public static readonly HashSet<Type> EnumUnderlyingTypes;
        public static readonly HashSet<Type> SimpleTypes;
        public static readonly HashSet<Type> Primitives;
        public static readonly HashSet<Type> IntuitivePrimitives;
        public static readonly HashSet<Type> RootTypes;
        public static readonly HashSet<Type> GenericCollections;
        public static readonly HashSet<Type> SystemTypes;
        // ReSharper restore MemberCanBePrivate.Global

        public static readonly Dictionary<Type, string> TypeAliases;

        static CSharp4Types()
        {
            EnumUnderlyingTypes = new HashSet<Type>
                                  {
                                      typeof(int),
                                      typeof(uint),
                                      typeof(short),
                                      typeof(ushort),
                                      typeof(long),
                                      typeof(ulong),
                                      typeof(byte),
                                      typeof(sbyte)
                                  };

            SimpleTypes =
                new HashSet<Type>(
                    EnumUnderlyingTypes.Union(new[] {typeof(char), typeof(float), typeof(double), typeof(decimal), typeof(bool)}));

            Primitives = new HashSet<Type>(SimpleTypes.Union(new[] {typeof(IntPtr), typeof(UIntPtr)}));

            RootTypes = new HashSet<Type>
                        {typeof(object), typeof(Enum), typeof(ValueType), typeof(Array), typeof(Delegate), typeof(Exception)};

            GenericCollections = new HashSet<Type>
                                 {
                                     typeof(Comparer<>),
                                     typeof(Dictionary<,>),
                                     typeof(EqualityComparer<>),
                                     typeof(HashSet<>),
                                     typeof(ICollection<>),
                                     typeof(IComparer<>),
                                     typeof(IDictionary<,>),
                                     typeof(IEnumerable<>),
                                     typeof(IEnumerator<>),
                                     typeof(IEqualityComparer<>),
                                     typeof(IList<>),
                                     typeof(ISet<>),
                                     typeof(KeyValuePair<,>),
                                     typeof(LinkedList<>),
                                     typeof(LinkedListNode<>),
                                     typeof(List<>),
                                     typeof(Queue<>),
                                     //typeof(SortedDictionary<,>),
                                     //typeof(SortedList<,>),
                                     //typeof(SortedSet<>),
                                     typeof(Stack<>),
                                 };

            IntuitivePrimitives =
                new HashSet<Type>(
                    Primitives.Union(new[] {typeof(DateTime), typeof(string), typeof(Type), typeof(Expression)}).Union(RootTypes));

            SystemTypes = new HashSet<Type>(IntuitivePrimitives.Union(GenericCollections));

            TypeAliases = new Dictionary<Type, string>
                          {
                              {typeof(byte), "byte"},
                              {typeof(sbyte), "sbyte"},
                              {typeof(short), "short"},
                              {typeof(ushort), "ushort"},
                              {typeof(int), "int"},
                              {typeof(uint), "uint"},
                              {typeof(long), "long"},
                              {typeof(ulong), "ulong"},
                              {typeof(char), "char"},
                              {typeof(float), "float"},
                              {typeof(double), "double"},
                              {typeof(decimal), "decimal"},
                              {typeof(bool), "bool"},
                              {typeof(object), "object"},
                              {typeof(string), "string"}
                          };
        }
    }
}
