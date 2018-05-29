using FluffySpoon.Publisher.GitHub;
using Microsoft.Extensions.DependencyInjection;
using Octokit;
using Octokit.Internal;

// ReSharper disable once CheckNamespace
namespace FluffySpoon
{
    public static class GitHubRegistrationExtensions
    {
        public static void AddGitHubProvider(
            this ServiceCollection services,
            string accessToken)
        {
            Setup(services,
                new Credentials(
                    accessToken));
        }

        public static void AddGitHubProvider(
          this ServiceCollection services,
          string username,
          string password)
        {
            Setup(services,
                new Credentials(
                    username,
                    password));
        }

        private static void Setup(ServiceCollection services, Credentials credentials)
        {
            RegistrationExtensions.AddSourceControlSystem<GitHubSourceControlSystem>();

            services.AddTransient<GitHubSourceControlSystem>();
            services.AddTransient<IGitHubSourceControlRepositoryFactory, GitHubSourceControlRepositoryFactory>();

            services.AddSingleton<IGitHubClient>(
                new GitHubClient(
                    new ProductHeaderValue("FluffySpoon.Publisher.GitHub"),
                    new InMemoryCredentialStore(
                        credentials)));
        }
    }
}
