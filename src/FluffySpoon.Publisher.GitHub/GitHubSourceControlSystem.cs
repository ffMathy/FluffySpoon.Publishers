using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffySpoon.Publisher.GitHub
{
  class GitHubSourceControlSystem : IRemoteSourceControlSystem
  {
    private readonly IGitHubClient _client;

    public GitHubSourceControlSystem(
      IGitHubClient client)
    {
      _client = client;
    }

    public async Task<IReadOnlyCollection<IRemoteSourceControlRepository>> GetCurrentUserRepositoriesAsync()
    {
      var repositories = await _client.Repository.GetAllForUser(_client.Connection.Credentials.Login);
      return repositories
        .Select(x => new GitHubSourceControlRepository()
        {
          Name = x.Name,
          Owner = x.Owner.Login
        })
        .ToArray();
    }
  }
}
