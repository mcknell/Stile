#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Patterns.Behavioral.Visitor;
using Stile.Patterns.Structural.Hierarchy;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Types.Expressions;
#endregion

namespace Stile.Prototypes.Specifications.Printable
{
	public interface IDescriber
	{
		void Append(string value);

		[StringFormatMethod("format")]
		void AppendFormat(string format, params object[] parameters);

		bool CanBeInlined(Expression expression);
		bool IsSingleToken(string sourceName);
	}

	public interface IDescriber<TVisitor, TTerm> : IDescriber
		where TVisitor : class
		where TTerm : class, IAcceptVisitors<TVisitor>
	{
		void FillStackAndUnwind([NotNull] TTerm lastTerm);
		TTerm Pop();
		void Push([NotNull] TTerm term);
	}

	public abstract class Describer : IDescriber
	{
		/*
		 * 
      string pattern = "^[^<>]*" +
                       "(" + 
                       "((?'Open'<)[^<>]*)+" +
                       "((?'Close-Open'>)[^<>]*)+" +
                       ")*" +
                       "(?(Open)(?!))$";
		 */
		/// <summary>
		/// Docs at <seealso cref="http://msdn.microsoft.com/en-us/library/bs2twtah.aspx#balancing_group_definition"/>
		/// </summary>
		private static readonly Regex SingleToken = new Regex(@"^@?[A-Z]\w*" //
			+ "(" // balancing construct; see 
			+ @"((?'Open'<)[A-Z]\w*(\,\s*[A-Z]\w*)*)*" // [A-Z]\w*(\,\s*[A-Z]\w*)*
			+ @"((?'Close-Open'>)([A-Z]\w*)?(\,\s*[A-Z]\w*)*)+" //
			+ ")*" //
			+ "(?(Open)(?!))" // fails on imbalance
			+ "$",
			RegexOptions.IgnoreCase | RegexOptions.Compiled,
			TimeSpan.FromSeconds(1));
		private readonly StringBuilder _stringBuilder;

		protected Describer()
		{
			_stringBuilder = new StringBuilder();
		}

		public void Append(string value)
		{
			_stringBuilder.Append(value);
		}

		public void AppendFormat(string format, params object[] parameters)
		{
			_stringBuilder.AppendFormat(format, parameters);
		}

		public bool CanBeInlined(Expression expression)
		{
			return expression is MethodCallExpression || expression is NewExpression;
		}

		public bool IsSingleToken(string sourceName)
		{
			return SingleToken.IsMatch(sourceName.Trim());
		}

		public override sealed string ToString()
		{
			return _stringBuilder.ToString();
		}
	}

	public abstract class Describer<TVisitor, TTerm> : Describer,
		IDescriber<TVisitor, TTerm>
		where TVisitor : class
		where TTerm : class, IAcceptVisitors<TVisitor>, IHasParent<TTerm>
	{
		protected readonly ISource _source;
		private readonly Stack<TTerm> _terms;

		protected Describer([CanBeNull] ISource source)
		{
			_source = source;
			_terms = new Stack<TTerm>();
		}

		public void FillStackAndUnwind(TTerm lastTerm)
		{
			Push(lastTerm.ValidateArgumentIsNotNull());
			TTerm parent = lastTerm.Parent;
			while (parent != null)
			{
				Push(parent);
				parent = parent.Parent;
			}
			while (Unwind())
			{
				// keep going until stack is empty
			}
		}

		public TTerm Pop()
		{
			return _terms.Pop();
		}

		public void Push(TTerm term)
		{
			_terms.Push(term.ValidateArgumentIsNotNull());
		}

		public static void DescribeSourceAndProcedure<TSubject>(IDescriber describer,
			IProcedure<TSubject> procedure,
			[NotNull] ISource<TSubject> source,
			string instrumentedBy,
			Action continuation = null)
		{
			ISourceState<TSubject> sourceState = source.Xray;
			string sourceDescription = sourceState.Description.Value;
			if (describer.IsSingleToken(sourceDescription))
			{
				ILazyDescriptionOfLambda lambda = procedure.Xray.Lambda;
				describer.AppendFormat("{0}", lambda.AliasParametersIntoBody(sourceDescription));
			}
			else if (describer.CanBeInlined(sourceState.Expression.Body))
			{
				ILazyDescriptionOfLambda lambda = procedure.Xray.Lambda;
				describer.AppendFormat("{0}", lambda.AliasParametersIntoBody(sourceState.Expression.Body.ToDebugString()));
			}
			else
			{
				describer.AppendFormat("{0} {1} ", sourceDescription, instrumentedBy);
				if (continuation != null)
				{
					continuation.Invoke();
				}
			}
		}

		private bool Unwind()
		{
			if (_terms.Count != 0)
			{
				TTerm next = _terms.Pop();
				next.Accept(this as TVisitor);
				return true;
			}
			return false;
		}
	}
}
