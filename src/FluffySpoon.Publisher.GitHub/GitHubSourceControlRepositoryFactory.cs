using FluffySpoon.Publisher.Remote;
using Octokit;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluffySpoon.Publisher.GitHub
{
  class GitHubSourceControlRepositoryFactory : IGitHubSourceControlRepositoryFactory
  {
    private readonly IGitHubClient _client;

    public GitHubSourceControlRepositoryFactory(
      IGitHubClient client)
    {
      _client = client;
    }

    public GitHubSourceControlRepository Create()
    {
      return new GitHubSourceControlRepository(_client);
    }
  }
}
