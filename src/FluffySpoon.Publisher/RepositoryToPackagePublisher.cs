﻿using FluffySpoon.Publisher.Local;
using FluffySpoon.Publisher.Remote;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FluffySpoon.Publisher
{
    class RepositoryToPackagePublisher : IRepositoryToPackagePublisher
    {
        private readonly IEnumerable<IRemoteSourceControlSystem> _sourceControlSystems;
        private readonly IEnumerable<ILocalPackageProcessor> _localPackageProcessors;
        private readonly IEnumerable<IRemotePackageSystem> _remotePackageSystems;

        private readonly ISettings _repositoryFilter;

        public RepositoryToPackagePublisher(
          ISettings repositoryFilter,
          IEnumerable<IRemoteSourceControlSystem> sourceControlSystems,
          IEnumerable<ILocalPackageProcessor> localPackageScanners,
          IEnumerable<IRemotePackageSystem> remotePackageSystems)
        {
            _repositoryFilter = repositoryFilter;
            _sourceControlSystems = sourceControlSystems;
            _localPackageProcessors = localPackageScanners;
            _remotePackageSystems = remotePackageSystems;
        }

        public async Task RefreshAllPackagesFromAllRepositoriesAsync()
        {
            try
            {
                foreach (var sourceControlSystem in _sourceControlSystems)
                {
                    var allRepositories = await sourceControlSystem.GetCurrentUserRepositoriesAsync();
                    var fluffySpoonRepositories = allRepositories
                      .Where(x => x.Name.StartsWith(_repositoryFilter.ProjectPrefix))
                      .ToArray();
                    await RefreshAllPackagesFromRepositoriesAsync(fluffySpoonRepositories);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
            }
        }

        private async Task RefreshAllPackagesFromRepositoriesAsync(
          IReadOnlyCollection<IRemoteSourceControlRepository> repositories)
        {
            var timestamp = DateTime.Now - new DateTime(2016, 1, 1);
            foreach (var repository in repositories)
            {
                Console.WriteLine("Downloading " + repository.Name);

                var folderPath = Path.Combine(
                  PathHelper.GetFluffySpoonWorkingDirectory(),
                  (long)timestamp.TotalSeconds + "",
                  repository.Name);
                await repository.DownloadToDirectoryAsync(folderPath);

                await RefreshAllPackagesInDirectoryAsync(
                  repository,
                  folderPath);
            }
        }

        private async Task RefreshAllPackagesInDirectoryAsync(
          IRemoteSourceControlRepository repository,
          string folderPath)
        {
            foreach (var localPackageProcessor in _localPackageProcessors)
            {
                Console.WriteLine("Scanning for local packages to refresh using " + localPackageProcessor.GetType().Name);

                await RefreshPackagesAsync(
                  localPackageProcessor,
                  repository,
                  folderPath);
            }
        }

        private async Task RefreshPackagesAsync(
          ILocalPackageProcessor processor,
          IRemoteSourceControlRepository repository,
          string folderPath)
        {
            var packages = await processor.ScanForPackagesInDirectoryAsync(folderPath);
            foreach (var package in packages)
            {
                Console.WriteLine("Refreshing package " + package.PublishName);

                await RefreshPackageAsync(
                  package,
                  repository);
            }
        }

        private async Task RefreshPackageAsync(
          ILocalPackage package,
          IRemoteSourceControlRepository repository)
        {
            foreach (var remotePackageSystem in _remotePackageSystems)
            {
                var processor = package.Processor;

                if (!remotePackageSystem.CanPublishPackage(package)) {
                    continue;
                }

                await processor.BuildPackageAsync(
                  package,
                  repository);

		var doesPackageExist = await remotePackageSystem.DoesPackageWithVersionExistAsync(package);
		if (doesPackageExist.HasValue && doesPackageExist.Value) {
                    Console.WriteLine("Can't publish package " + package.PublishName + " because it already exists");
                    continue;
                }

                await remotePackageSystem.UpsertPackageAsync(package);
            }
        }
    }
}
