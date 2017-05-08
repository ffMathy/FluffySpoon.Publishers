using FluffySpoon.Publisher.Remote;
using Microsoft.Extensions.DependencyInjection;
using Octokit;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluffySpoon.Publisher.DotNet
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
