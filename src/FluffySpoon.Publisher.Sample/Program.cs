using System;
using System.IO;
using FluffySpoon;
using Microsoft.Extensions.DependencyInjection;

namespace FluffySpoon.Publisher.Sample
{
	class Program
	{
		static void Main()
		{
			var projectPrefix = AskFor("Project prefix", "FluffySpoon.", "ProjectNamePrefix");
			var githubAccessToken = AskFor("GitHub personal access token", null, "GitHubPersonalAccessToken");
			var nugetKey = AskFor("NuGet API key", null, "NuGetKey");
			var npmKey = AskFor("NPM auth token", null, "NpmAuthToken");

			var services = new ServiceCollection();
			services.AddRepositoryToPackagePublisher(
				projectPrefix);
			services.AddGitHubProvider(
				githubAccessToken);
			services.AddNuGetProvider(
				nugetKey);
			services.AddDotNetProvider();
			services.AddNodeJsProvider();
			services.AddNpmProvider(
				npmKey);

			var provider = services.BuildServiceProvider();

			var publisher = provider.GetRequiredService<IRepositoryToPackagePublisher>();
			publisher.RefreshAllPackagesFromAllRepositoriesAsync().Wait();

			Console.WriteLine("All done!");
		}

		private static string AskFor(string phrase, string defaultValue, string environmentVariableKey)
		{
			var environmentValue = Environment.GetEnvironmentVariable(environmentVariableKey);
			if (environmentValue != null)
			{
				return environmentValue;
			}

			var fileLocation = Path.Combine(
				Environment.CurrentDirectory,
				environmentVariableKey + ".secret");
			if (File.Exists(fileLocation))
				return File.ReadAllText(fileLocation);

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