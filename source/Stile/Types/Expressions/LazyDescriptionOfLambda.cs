#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using JetBrains.Annotations;
#endregion

namespace Stile.Types.Expressions
{
	public interface ILazyDescriptionOfLambda
	{
		string Body { get; }
		string[] SubjectTokens { get; }
		string AliasParametersIntoBody(params string[] aliases);
	}

	public class LazyDescriptionOfLambda : ILazyDescriptionOfLambda
	{
		private readonly Lazy<string> _body;
		private readonly LambdaExpression _lambda;

		public LazyDescriptionOfLambda([NotNull] LambdaExpression lambda)
		{
			_lambda = lambda;
			SubjectTokens = _lambda.Parameters.Select(x => x.Name).ToArray();
			_body = _lambda.Body.ToLazyDebugString();
		}

		[NotNull]
		public string Body
		{
			get
			{
				string value = _body.Value;
				return value;
			}
		}
		public string[] SubjectTokens { get; private set; }

		public string AliasParametersIntoBody(params string[] aliases)
		{
			var expression = (MethodCallExpression) _lambda.Body;
			int i = 0;
			Dictionary<string, string> dictionary = aliases.ToDictionary(alias => SubjectTokens[i++]);
			Lazy<string> lazyDebugString = expression.ToLazyDebugString(dictionary);
			string value = lazyDebugString.Value;
			return value;
		}
	}
}
