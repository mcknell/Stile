#region License info...
// Propter for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Propter project on GitHub
#endregion

#region using...
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
#endregion

namespace Stile.DocumentationGeneration
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            Console.WriteLine("Beginning "+ typeof(Program).Assembly.GetName().Name);
            Assembly stile = typeof(VersionedLanguage).Assembly;
            Assembly[] others = args.Select(Assembly.ReflectionOnlyLoadFrom).ToArray();
            var generator = new Generator(stile, ProductionRuleExtensions.Lexicon, others);
            string generated = generator.Generate();
            Debugger.Break();
            Console.WriteLine(generated);
            return 0;
        }
    }
}
