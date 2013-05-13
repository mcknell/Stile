#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
#endregion

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.

[assembly : AssemblyTitle("Stile")]
[assembly : AssemblyDescription("Syntax helpers and pattern templates for a certain C# idiomatic style.")]
[assembly : AssemblyConfiguration("")]
//[assembly: AssemblyCompany("Mark Knell")]
//[assembly: AssemblyProduct("Stile")]
//[assembly: AssemblyCopyright("Copyright © 2010-2012 Mark Knell")]
//[assembly: AssemblyTrademark("")]

[assembly : AssemblyCulture("")]
[assembly : NeutralResourcesLanguage("en")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
//[assembly: AssemblyVersion("0.1.0.1")]
//[assembly: AssemblyFileVersion("0.1.0.1")]

#if DEBUG

[assembly : InternalsVisibleTo("Stile.Tests,PublicKey=" //
	+ "0024000004800000940000000602000000240000525341310004000001000100c5459727402622"
	+ "d8a5e2b3ba7878a9d573c9de24b6777f8551e64ff2fee7783a0ffd292d81df99121d267e746b26"
	+ "5e4b930d6ef204c788186639d013bf67fd3b7ea6acf6b03e84cb5271e10c2dc171ed24b3a2ce5f"
	+ "e99e500a09565a905ddcb4c95d8a7ad527df8eea62327795eafc686ba238f507768636d6669899" //
	+ "823eacd3")]
#endif

[assembly : CLSCompliant(true)]
