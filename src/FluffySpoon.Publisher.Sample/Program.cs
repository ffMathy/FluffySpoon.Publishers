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
      var projectPrefix = AskFor("Project prefix", "FluffySpoon.", args, 0);
      var githubCredentials = AskForGitHubCredentials(args);
      var nugetKey = AskFor("NuGet API key", null, args, 3);

      var services = new ServiceCollection();
      services.AddRepositoryToPackagePublisher(
          projectPrefix);
      services.AddGitHubProvider(
          githubCredentials.username,
          githubCredentials.password);
      services.AddNuGetProvider(
          nugetKey);
      services.AddDotNetProvider();

      var provider = services.BuildServiceProvider();

      var publisher = provider.GetRequiredService<IRepositoryToPackagePublisher>();
      publisher.RefreshAllPackagesFromAllRepositoriesAsync().Wait();

      Console.WriteLine("All done!");
      Console.ReadLine();
    }

    private static (string username, string password) AskForGitHubCredentials(string[] args)
    {
      string githubUsername = AskFor("GitHub username", "ffMathy", args, 1);
      string githubPassword = AskFor("GitHub password", null, args, 2);

      return (githubUsername, githubPassword);
    }

    private static string AskFor(string phrase, string defaultValue, string[] args, int argumentOffset)
    {
      if (argumentOffset < args.Length)
      {
        return args[argumentOffset];
      }

      var oldConsoleColor = Console.ForegroundColor;

      Console.ForegroundColor = ConsoleColor.Blue;

      Console.Write(phrase);
      if (defaultValue != null)
      {
        Console.Write(" (");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(defaultValue);
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(")");
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