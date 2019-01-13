using FluffySpoon.Publisher.GitHub;
using FluffySpoon.Publisher.Remote;
using Microsoft.Extensions.DependencyInjection;
using Octokit;
using Octokit.Internal;

// ReSharper disable once CheckNamespace
namespace FluffySpoon
{
    public static class GitHubRegistrationExtensions
    {
        public static void AddGitHubProviderForAccessToken(
            this ServiceCollection services,
            string username,
            string accessToken)
        {
            Setup(services,
                username,
                new Credentials(
                    accessToken));
        }

        public static void AddGitHubProviderUsingCredentials(
          this ServiceCollection services,
          string username,
          string password)
        {
            Setup(services,
                username,
                new Credentials(
                    username,
                    password));
        }

        private static void Setup(ServiceCollection services, string username, Credentials credentials)
        {
			services.AddTransient<IRemoteSourceControlSystem>(p => p.GetRequiredService<GitHubSourceControlSystem>());

            services.AddTransient(provider => new GitHubSourceControlSystem(
                username,
                provider.GetService<IGitHubClient>(),
                provider.GetService<IGitHubSourceControlRepositoryFactory>()));

            services.AddTransient<IGitHubSourceControlRepositoryFactory, GitHubSourceControlRepositoryFactory>();

            services.AddSingleton<IGitHubClient>(
                new GitHubClient(
                    new ProductHeaderValue("FluffySpoon.Publisher.GitHub"),
                    new InMemoryCredentialStore(
                        credentials)));
        }
    }
}
