#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Stile.Prototypes.Compilation.Grammars.ContextFree.Builders;
using Stile.Readability;
using Stile.Types.Reflection;
#endregion

namespace Stile.Tests.Prototypes.Compilation.Grammars.ContextFree.Builders
{
	[TestFixture]
	public class TokenFixture
	{
		[Test]
		public void DefaultCtorCreatesInvalidObject()
		{
			const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;
			var token = new Token();
			foreach (PropertyInfo propertyInfo in typeof(Token).GetProperties(flags).Where(x => x.CanRead))
			{
				PropertyInfo copy = propertyInfo;
				AssertThrowsInvalidOperation(() => copy.GetValue(token));
			}
			foreach (
				MethodInfo methodInfo in
					typeof(Token).GetMethods(flags)
						.Where(x => x.IsGenericMethod == false && x.DeclaringType == typeof(Token)))
			{
				object[] parameters = methodInfo.GetParameters().Select(x => x.ParameterType.GetDefault()).ToArray();
				MethodInfo copy = methodInfo;
				Console.WriteLine(copy.Name);
				AssertThrowsInvalidOperation(() => copy.Invoke(token, parameters));
			}
		}

		private static void AssertThrowsInvalidOperation(TestDelegate testDelegate)
		{
			var exception = Assert.Throws<TargetInvocationException>(testDelegate, testDelegate.ToDebugString());
			Assert.That(exception.InnerException, Is.InstanceOf<InvalidOperationException>());
			Assert.That(exception.InnerException.Message, Contains.Substring(ErrorMessages.Token_DefaultCtorInvalid));
		}
	}
}
