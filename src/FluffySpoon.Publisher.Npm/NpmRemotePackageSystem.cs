using System;
using System.IO;
using System.Threading.Tasks;
using FluffySpoon.Publisher.Local;
using FluffySpoon.Publisher.NodeJs;
using FluffySpoon.Publisher.Remote;

namespace FluffySpoon.Publisher.Npm;

class NpmRemotePackageSystem : IRemotePackageSystem
{
    private readonly ISettings _repositoryFilter;
    private readonly INpmSettings _npmSettings;

    public NpmRemotePackageSystem(
        INpmSettings nuGetSettings,
        ISettings repositoryFilter)
    {
        _npmSettings = nuGetSettings;
        _repositoryFilter = repositoryFilter;
    }

    public bool CanPublishPackage(ILocalPackage package)
    {
        var folderPathName = Path.GetFileName(package.FolderPath);
        return package is INodeJsLocalPackage &&
               folderPathName.StartsWith(_repositoryFilter.ProjectPrefix);
    }

    public Task<bool?> DoesPackageWithVersionExistAsync(ILocalPackage package)
    {
        return Task.FromResult<bool?>(null);
    }

    public async Task UpsertPackageAsync(ILocalPackage package)
    {
        Console.WriteLine("Upserting package " + package.PublishName + " version " + package.Version + " to remote package repository.");

        await NpmHelper.PublishAsync(
            package.FolderPath, 
            _npmSettings.AuthToken);

        package.PublishUrl = $"https://www.npmjs.com/package/{package.PublishName}/v/{package.Version}";
    }
}