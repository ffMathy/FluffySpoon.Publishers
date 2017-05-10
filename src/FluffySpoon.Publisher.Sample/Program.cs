using FluffySpoon.Publisher;
using FluffySpoon.Publisher.DotNet;
using FluffySpoon.Publisher.GitHub;
using FluffySpoon.Publisher.NuGet;
using Microsoft.Extensions.DependencyInjection;
using Octokit;
using System;

namespace FluffySpoon.Publishers.Sample
{
  class Program
  {
    static void Main(string[] args)
    {
      var githubCredentials = AskForGitHubCredentials();
      var nugetKey = AskFor("NuGet API key");

      var services = new ServiceCollection();
      services.AddRepositoryToPackagePublisher();
      services.AddGitHubProvider(
        githubCredentials.username,
        githubCredentials.password);
      services.AddNuGetProvider(nugetKey);
      services.AddDotNetProvider();

      var provider = services.BuildServiceProvider();

      var publisher = provider.GetRequiredService<IRepositoryToPackagePublisher>();
      publisher.RefreshAllPackagesFromAllRepositoriesAsync().Wait();

      Console.WriteLine("All done!");
      Console.ReadLine();
    }

    private static (string username, string password) AskForGitHubCredentials()
    {
      string githubUsername = AskFor("GitHub username", "ffMathy");
      string githubPassword = AskFor("GitHub password");

      return (githubUsername, githubPassword);
    }

    private static string AskFor(string phrase, string defaultValue = null)
    {
      var oldConsoleColor = Console.ForegroundColor;

      Console.ForegroundColor = ConsoleColor.Green;

      Console.Write(phrase);
      if(defaultValue != null)
      {
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.Write(" (" + defaultValue + ")");
        Console.ForegroundColor = ConsoleColor.Green;
      }
      Console.Write(": ");

      Console.ForegroundColor = oldConsoleColor;

      var result = Console.ReadLine();
      Console.Clear();

      if (string.IsNullOrWhiteSpace(result))
        return defaultValue;

      return result;
    }
  }
}