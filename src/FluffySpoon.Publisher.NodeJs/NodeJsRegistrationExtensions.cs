using FluffySpoon.Publisher;
using FluffySpoon.Publisher.Local;
using FluffySpoon.Publisher.NodeJs;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace FluffySpoon;

public static class NodeJsRegistrationExtensions
{
  public static void AddNodeJsProvider(this ServiceCollection services)
  {
    services.AddTransient<ILocalPackageProcessor, NodeJsLocalPackageProcessor>();

    services.AddTransient<NodeJsLocalPackageProcessor>();
  }
}