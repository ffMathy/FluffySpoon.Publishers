using FluffySpoon.Publisher;
using FluffySpoon.Publisher.GitHub;
using Microsoft.Extensions.DependencyInjection;
using Octokit;
using System;

namespace FluffySpoon.Publishers.Sample
{
  class Program
  {
    static void Main(string[] args)
    {
      var services = new ServiceCollection();
      services.AddRepositoryToPackagePublisher();
      services.AddGitHubProvider();

      var provider = services.BuildServiceProvider();

      var publisher = provider.GetRequiredService<IRepositoryToPackagePublisher>();
      publisher.RefreshAllPackagesFromAllRepositoriesAsync();
    }
  }
}