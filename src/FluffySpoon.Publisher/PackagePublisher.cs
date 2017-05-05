using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffySpoon.Publisher
{
  class PackagePublisher : IPackagePublisher
  {
    private readonly IEnumerable<IRemoteSourceControlSystem> _sourceControlSystems;

    public PackagePublisher(
      IEnumerable<IRemoteSourceControlSystem> sourceControlSystems)
    {
      _sourceControlSystems = sourceControlSystems;
    }

    public async Task RefreshAllPackagesFromAllRepositoriesAsync()
    {
      foreach(var sourceControlSystem in _sourceControlSystems)
      {
        var allRepositories = await sourceControlSystem.GetCurrentUserRepositoriesAsync();
        var fluffySpoonRepositories = allRepositories
          .Where(x => x
            .Name
            .StartsWith("FluffySpoon."))
          .ToArray();
        await RefreshAllPackagesFromRepositories(fluffySpoonRepositories);
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
