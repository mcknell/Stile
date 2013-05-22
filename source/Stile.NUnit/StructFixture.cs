#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Stile.Readability;
using Stile.Types.Reflection;
#endregion

namespace Stile.NUnit
{
	[TestFixture]
	public abstract class StructFixture<TStruct>
		where TStruct : struct
	{
		[Test]
		public void DefaultCtorCreatesInvalidObject()
		{
			const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;
			var @struct = new TStruct();
			foreach (PropertyInfo propertyInfo in typeof(TStruct).GetProperties(flags).Where(x => x.CanRead))
			{
				PropertyInfo copy = propertyInfo;
				AssertThrowsInvalidOperation(() => copy.GetValue(@struct));
			}
			foreach (MethodInfo methodInfo in
				typeof(TStruct).GetMethods(flags)
					.Where(x => x.IsGenericMethod == false && x.DeclaringType == typeof(TStruct)))
			{
				object[] parameters = methodInfo.GetParameters().Select(x => x.ParameterType.GetDefault()).ToArray();
				MethodInfo copy = methodInfo;
				Console.WriteLine(copy.Name);
				AssertThrowsInvalidOperation(() => copy.Invoke(@struct, parameters));
			}
		}

		protected abstract string ExpectedCtorMessage { get; }

		protected void AssertThrowsInvalidOperation(TestDelegate testDelegate)
		{
			var exception = Assert.Throws<TargetInvocationException>(testDelegate, testDelegate.ToDebugString());
			Assert.That(exception.InnerException, Is.InstanceOf<InvalidOperationException>());
			Assert.That(exception.InnerException.Message, Contains.Substring(ExpectedCtorMessage));
		}
	}
}
