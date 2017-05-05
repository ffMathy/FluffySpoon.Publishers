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
    private readonly IEnumerable<ILocalPackageScanner> _localPackageScanners;
    private readonly IEnumerable<IRemotePackageSystem> _remotePackageSystems;

    public PackagePublisher(
      IEnumerable<IRemoteSourceControlSystem> sourceControlSystems,
      IEnumerable<ILocalPackageScanner> localPackageScanners,
      IEnumerable<IRemotePackageSystem> remotePackageSystems)
    {
      _sourceControlSystems = sourceControlSystems;
      _localPackageScanners = localPackageScanners;
      _remotePackageSystems = remotePackageSystems;
    }

    public async Task RefreshAllPackagesFromAllRepositoriesAsync()
    {
      foreach (var sourceControlSystem in _sourceControlSystems)
      {
        var allRepositories = await sourceControlSystem.GetCurrentUserRepositoriesAsync();
        var fluffySpoonRepositories = allRepositories
          .Where(x => x
            .Name
            .StartsWith("FluffySpoon."))
          .ToArray();
        await RefreshAllPackagesFromRepositoriesAsync(fluffySpoonRepositories);
      }
    }

    private async Task RefreshAllPackagesFromRepositoriesAsync(
      IReadOnlyCollection<IRemoteSourceControlRepository> repositories)
    {
      foreach (var repository in repositories)
      {
        Console.WriteLine("Downloading " + repository.Name);

        var folderPath = Path.Combine("Repositories", repository.Name);
        await repository.DownloadToDirectoryAsync(folderPath);

        await RefreshAllPackagesInDirectoryAsync(folderPath);
      }
    }

    private async Task RefreshAllPackagesInDirectoryAsync(string folderPath)
    {
      foreach (var localPackageScanner in _localPackageScanners)
      {
        var packages = await localPackageScanner.ScanForPackagesInDirectoryAsync(folderPath);
        await RefreshPackagesAsync(packages);
      }
    }

    private async Task RefreshPackagesAsync(IReadOnlyCollection<ILocalPackage> packages)
    {
      foreach (var package in packages)
      {
        await RefreshPackageAsync(package);
      }
    }

    private async Task RefreshPackageAsync(ILocalPackage package)
    {
      foreach (var packageSystem in _remotePackageSystems)
      {
        if (!packageSystem.CanPublishPackage(package))
          continue;

        if (await packageSystem.DoesPackageWithVersionExistAsync(package))
          continue;

        await packageSystem.UpsertPackageAsync(package);
      }
    }
  }
}
