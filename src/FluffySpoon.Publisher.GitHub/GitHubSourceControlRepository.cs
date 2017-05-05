using FluffySpoon.Publishers.GitHub;
using Octokit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace FluffySpoon.Publisher.GitHub
{
  class GitHubSourceControlRepository : IGitHubSourceControlRepository
  {
    private readonly IGitHubClient _client;

    public string Name { get; set; }
    public string Owner { get; set; }

    public GitHubSourceControlRepository(
      IGitHubClient client)
    {
      _client = client;
    }

    public async Task DownloadToDirectoryAsync(string folderPath)
    {
      var repository = await _client.Repository.Get(Owner, Name);
      GitHelper.Clone(folderPath, repository);
    }
  }
}
