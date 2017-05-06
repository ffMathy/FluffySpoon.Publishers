using FluffySpoon.Publisher.Local;
using FluffySpoon.Publisher.Remote;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffySpoon.Publisher
{
  class RepositoryToPackagePublisher : IRepositoryToPackagePublisher
  {
    private readonly IEnumerable<IRemoteSourceControlSystem> _sourceControlSystems;
    private readonly IEnumerable<ILocalPackageProcessor> _localPackageProcessors;
    private readonly IEnumerable<IRemotePackageSystem> _remotePackageSystems;

    public RepositoryToPackagePublisher(
      IEnumerable<IRemoteSourceControlSystem> sourceControlSystems,
      IEnumerable<ILocalPackageProcessor> localPackageScanners,
      IEnumerable<IRemotePackageSystem> remotePackageSystems)
    {
      _sourceControlSystems = sourceControlSystems;
      _localPackageProcessors = localPackageScanners;
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
            .StartsWith($"{nameof(FluffySpoon)}."))
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
      foreach (var localPackageProcessor in _localPackageProcessors)
      {
        await RefreshPackagesAsync(
          localPackageProcessor,
          folderPath);
      }
    }

    private async Task RefreshPackagesAsync(
      ILocalPackageProcessor processor,
      string folderPath)
    {
      var packages = await processor.ScanForPackagesInDirectoryAsync(folderPath);
      foreach (var package in packages)
      {
        await RefreshPackageAsync(package);
      }
    }

    private async Task RefreshPackageAsync(ILocalPackage package)
    {
      foreach (var remotePackageSystem in _remotePackageSystems)
      {
        if (!remotePackageSystem.CanPublishPackage(package))
          continue;

        if (await remotePackageSystem.DoesPackageWithVersionExistAsync(package))
          continue;

        var processor = package.Processor;
        
        //TODO: hmmm... how to bump version number, and who should be responsible?
        //TODO: parse repository in as well, and use its system to fetch the revision. then parse that revision to the BuildPackageAsync method!
        wqdkqwd
        await processor.BuildPackageAsync(package);

        await remotePackageSystem.UpsertPackageAsync(package);
      }
    }
  }
}
