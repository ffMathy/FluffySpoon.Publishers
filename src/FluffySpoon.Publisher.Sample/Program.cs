using System;
using FluffySpoon;
using Microsoft.Extensions.DependencyInjection;

namespace FluffySpoon.Publisher.Sample
{
  class Program
  {
    static void Main()
    {
      var projectPrefix = AskFor("Project prefix", "FluffySpoon.", "ProjectNamePrefix");
      var githubCredentials = AskForGitHubCredentials();
      var nugetKey = AskFor("NuGet API key", null, "NuGetKey");

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
    }

    private static (string username, string password) AskForGitHubCredentials()
    {
      string githubUsername = AskFor("GitHub username", "ffMathy", "GitHubUsername");
      string githubPassword = AskFor("GitHub password", null, "GitHubPassword");

      return (githubUsername, githubPassword);
    }

    private static string AskFor(string phrase, string defaultValue, string environmentVariableKey)
    {
      var environmentValue = Environment.GetEnvironmentVariable(environmentVariableKey);
      if (environmentValue != null)
      {
        return environmentValue;
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