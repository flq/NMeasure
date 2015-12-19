using System.Reflection;

[assembly: AssemblyTitle("NMeasure.Tests")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Microsoft")]
[assembly: AssemblyProduct("NMeasure.Tests")]
[assembly: AssemblyCopyright("Copyright © Microsoft 2010")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

// Unfortunately the UnitConfiguration is designed static and cannot deal with
// tests running in parallel.
[assembly: Xunit.CollectionBehavior(Xunit.CollectionBehavior.CollectionPerAssembly)]