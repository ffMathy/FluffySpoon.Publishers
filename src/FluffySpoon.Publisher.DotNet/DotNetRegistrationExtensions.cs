using FluffySpoon.Publisher;
using FluffySpoon.Publisher.DotNet;
using Microsoft.Extensions.DependencyInjection;
using FluffySpoon.Publisher.DotNet.Helpers;
using FluffySpoon.Publisher.Local;

// ReSharper disable once CheckNamespace
namespace FluffySpoon
{
	public static class DotNetRegistrationExtensions
	{
		public static void AddDotNetProvider(this ServiceCollection services)
		{
			services.AddTransient<ILocalPackageProcessor, DotNetLocalPackageProcessor>();

			services.AddTransient<IProjectFileParser, ProjectFileParser>();
			services.AddTransient<ISolutionFileParser, SolutionFileParser>();

			services.AddTransient<DotNetLocalPackageProcessor>();
		}
	}
}
