using Microsoft.Extensions.DependencyInjection;
using Octokit;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluffySpoon.Publisher.GitHub
{
  public static class GitHubRegistrationExtensions
  {
    public static void AddGitHubProvider(this ServiceCollection services)
    {
      RegistrationExtensions.AddSourceControlSystem<GitHubSourceControlSystem>();

      services.AddSingleton<IGitHubClient>(
        new GitHubClient(
          new ProductHeaderValue("FluffySpoon.Publisher.GitHub")));
    }
  }
}
