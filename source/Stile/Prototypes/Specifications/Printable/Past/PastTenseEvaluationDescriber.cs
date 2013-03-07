﻿#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.Builders.OfExpectations;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Has;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Is;
using Stile.Prototypes.Specifications.Builders.OfInstruments;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
using Stile.Resources;
using Stile.Types.Expressions;
#endregion

namespace Stile.Prototypes.Specifications.Printable.Past
{
	public interface IPastTenseEvaluationDescriber : IEvaluationVisitor {}

	public class PastTenseEvaluationDescriber : IPastTenseEvaluationDescriber
	{
		private readonly Func<IPastTenseExpectationFormatVisitor> _expectationFormaterFactory;
		private readonly StringBuilder _stringBuilder;

		public PastTenseEvaluationDescriber(Func<IPastTenseExpectationFormatVisitor> expectationFormatterFactory = null)
		{
			_expectationFormaterFactory = expectationFormatterFactory ?? (()=> new PastTenseExpectationFormatVisitor());
			_stringBuilder = new StringBuilder();
		}

		public void Visit1<TSubject>(IProcedure<TSubject> procedure)
		{
			throw new NotImplementedException();
		}

		public void Visit1<TSubject>(IProcedureBuilder<TSubject> builder)
		{
			throw new NotImplementedException();
		}

		public void Visit1<TSubject>(ISource<TSubject> source)
		{
			source.ValidateArgumentIsNotNull();
		}

		public void Visit2<TSubject,TResult>(IExpectation<TSubject, TResult> expectation)
		{
			var expectationFormater = _expectationFormaterFactory.Invoke();
			expectation.Accept(expectationFormater);
			_stringBuilder.Append(expectationFormater.ToString());
		}

		public virtual void DescribeOverload2<TSubject, TResult>(IEvaluation<TSubject, TResult> evaluation)
		{
			IEvaluationState<TSubject, TResult> state = evaluation.ValidateArgumentIsNotNull().Xray;
			_stringBuilder.Append(PastTenseEvaluations.Expected + " ");
			if (state.Source != null)
			{
				_stringBuilder.AppendFormat("{0} ", PastTenseEvaluations.That);
				string source = state.Source.Xray.Description.Value;
				if (IsSingleToken(source))
				{
					ILazyDescriptionOfLambda lambda = state.Instrument.Xray.Lambda;
					_stringBuilder.AppendFormat("{0}", lambda.AliasParametersIntoBody(source));
				} else
				{
					_stringBuilder.AppendFormat("{0} {1} ", source, PastTenseEvaluations.InstrumentedBy);
					Visit2(state.Instrument);
				}
			}
			Visit2(state.Expectation);
		}

		public void Visit2<TSubject, TResult>(IInstrument<TSubject, TResult> instrument)
		{
			IProcedureState<TSubject> state = instrument.ValidateArgumentIsNotNull().Xray;
			_stringBuilder.Append(state.Lambda.Body);
		}

		public void Visit3<TSpecification, TSubject, TResult>(IHas<TSpecification, TSubject, TResult> has)
			where TSpecification : class, IChainableSpecification
		{
			throw new NotImplementedException();
		}

		public void Visit3<TSpecification, TSubject, TResult>(IIs<TSpecification, TSubject, TResult> @is)
			where TSpecification : class, IChainableSpecification
		{
			throw new NotImplementedException();
		}

		public void Visit3<TSubject, TResult, TExpectationBuilder>(
			ISpecification<TSubject, TResult, TExpectationBuilder> specification)
			where TExpectationBuilder : class, IExpectationBuilder
		{
			throw new NotImplementedException();
		}

		public void Visit3<TSpecification, TSubject, TResult>(
			IExpectationBuilder<TSpecification, TSubject, TResult> builder)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
		{
			throw new NotImplementedException();
		}

		public void Visit4<TSpecification, TSubject, TResult, TItem>(
			IEnumerableExpectationBuilder<TSpecification, TSubject, TResult, TItem> builder)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
			where TResult : class, IEnumerable<TItem>
		{
			throw new NotImplementedException();
		}

		public void Visit2<TSubject, TResult>(IEvaluation<TSubject, TResult> evaluation)
		{
			throw new NotImplementedException();
		}

		public TData Visit2<TSubject, TResult, TData>(IEvaluation<TSubject, TResult> evaluation, TData data = default(TData))
		{
			throw new NotImplementedException();
		}

		public static string Describe<TSubject, TResult>(IEvaluation<TSubject, TResult> evaluation)
		{
			var describer = new PastTenseEvaluationDescriber();
			describer.DescribeOverload2(evaluation);
			return describer.ToString();
		}

		public static bool IsSingleToken(string sourceName)
		{
			return Regex.IsMatch(sourceName.Trim(), @"^@?[A-Z]\w*$", RegexOptions.IgnoreCase);
		}

		public override string ToString()
		{
			return _stringBuilder.ToString();
		}
	}
}
