using FluffySpoon.Publisher;
using FluffySpoon.Publisher.DotNet;
using Microsoft.Extensions.DependencyInjection;
using FluffySpoon.Publisher.DotNet.Helpers;

// ReSharper disable once CheckNamespace
namespace FluffySpoon
{
  public static class DotNetRegistrationExtensions
  {
    public static void AddDotNetProvider(this ServiceCollection services)
    {
      RegistrationExtensions.AddPackageProcessor<DotNetLocalPackageProcessor>();

      services.AddTransient<IProjectFileParser, ProjectFileParser>();
      services.AddTransient<ISolutionFileParser, SolutionFileParser>();

      services.AddTransient<DotNetLocalPackageProcessor>();
    }
  }
}
