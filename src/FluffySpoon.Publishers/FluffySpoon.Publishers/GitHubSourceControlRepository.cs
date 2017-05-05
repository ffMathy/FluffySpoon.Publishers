using Octokit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace FluffySpoon.Publishers
{
  class GitHubSourceControlRepository : IRemoteSourceControlRepository
  {
    public string Name { get; set; }
    public string Owner { get; set; }

    public async Task DownloadToDirectoryAsync(string folderPath)
    {
      var client = new GitHubClient(new ProductHeaderValue("FluffySpoon.Publishers"));
      var repository = await client.Repository.Get(Owner, Name);

      GitHelper.Clone(folderPath, repository);
    }
  }
}
