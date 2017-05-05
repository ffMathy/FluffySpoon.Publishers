using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FluffySpoon.Publishers
{
  class PackagePublisher : IPackagePublisher
  {
    private readonly IEnumerable<IRemoteSourceControlSystem> _sourceControlSystems;

    public Publisher(
      IEnumerable<IRemoteSourceControlSystem> sourceControlSystems)
    {
      _sourceControlSystems = sourceControlSystems;
    }

    public async Task RefreshAllPackagesFromAllRepositoriesAsync()
    {
      foreach(var sourceControlSystem in _sourceControlSystems)
      {
        var allRepositories = await sourceControlSystem.GetAllRepositoriesAsync();
        await RefreshAllPackagesFromRepositories(allRepositories);
      }
    }

    private async Task RefreshAllPackagesFromRepositories(
      IReadOnlyCollection<IRemoteSourceControlRepository> repositories)
    {
      foreach(var repository in repositories)
      {
        Console.WriteLine("Downloading " + repository.Name);

        var folderPath = Path.Combine("Repositories", repository.Name);
        await repository.DownloadToDirectoryAsync(folderPath);
      }
    }
  }
}
