using Microsoft.Extensions.DependencyInjection;
using System;

namespace FluffySpoon.Publishers.Sample
{
  class Program
  {
    static void Main(string[] args)
    {
      var services = new ServiceCollection();
      services.AddPublishers();

      var provider = services.BuildServiceProvider();

      var publisher = provider.GetRequiredService<IPackagePublisher>();
      publisher.RefreshAllPackagesFromAllRepositoriesAsync();
    }
  }
}