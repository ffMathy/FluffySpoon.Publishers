using FluffySpoon.Publisher.Remote;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffySpoon.Publisher.GitHub
{
  class GitHubSourceControlSystem : IGitHubSourceControlSystem
  {
    private readonly IGitHubClient _client;
    private readonly IGitHubSourceControlRepositoryFactory _repositoryFactory;

    public GitHubSourceControlSystem(
      IGitHubClient client,
      IGitHubSourceControlRepositoryFactory repositoryFactory)
    {
      _client = client;
      _repositoryFactory = repositoryFactory;
    }

    public async Task<IReadOnlyCollection<IRemoteSourceControlRepository>> GetCurrentUserRepositoriesAsync()
    {
      var repositories = await _client.Repository.GetAllForUser(_client.Connection.Credentials.Login);
      return repositories
        .Select(Map)
        .ToArray();
    }

    private IGitHubSourceControlRepository Map(
      Repository githubClientRepository)
    {
      var repository = _repositoryFactory.Create();
      repository.System = this;
      repository.Name = githubClientRepository.Name;
      repository.Owner = githubClientRepository.Owner.Login;

      return repository;
    }
  }
}
