using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluffySpoon.Publishers
{
  class GitHubSourceControlRepository : IRemoteSourceControlRepository
  {
    public string Name { get; set; }

    public Task DownloadToDirectoryAsync(string folderPath)
    {
      throw new NotImplementedException();
    }
  }
}
