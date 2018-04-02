using FluffySpoon.Publisher;
using FluffySpoon.Publisher.DotNet;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace FluffySpoon
{
  public static class NodeJsRegistrationExtensions
  {
    public static void AddNodeJsProvider(this ServiceCollection services)
    {
      RegistrationExtensions.AddPackageProcessor<NodeJsLocalPackageProcessor>();

      services.AddTransient<NodeJsLocalPackageProcessor>();
    }
  }
}
